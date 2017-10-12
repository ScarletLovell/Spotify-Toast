using System;
using System.Windows.Forms;
using ToastTest;

namespace ToastTest
{
    using SpotifyAPI.Local;
    using SpotifyAPI.Local.Enums;
    using SpotifyAPI.Web;
    using SpotifyAPI.Web.Auth;
    using SpotifyAPI.Web.Enums;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Drawing;
    using System.Linq;

    public partial class Form1 : Form
    {
        /// <summary>The way to make the app force ontop of all apps</summary>
        interface IMouseClickable {
            void HandleMouseClick(object sender, MouseEventArgs e);
        }
        int width = SystemInformation.VirtualScreen.Width;
        int height = SystemInformation.VirtualScreen.Height;
        static Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        KeyValueConfigurationCollection confFile = configManager.AppSettings.Settings;
        protected override CreateParams CreateParams
        { // https://stackoverflow.com/a/26607006 , I couldn't figure this out on my own.
            get {
                var cp = base.CreateParams;
                if(confFile["alwaysOnTop"] != null && confFile["alwaysOnTop"].Value.ToLower().Equals("false")) {
                    return cp;
                }
                cp.ExStyle |= 8;  // Turn on WS_EX_TOPMOST
                return cp;
            }
        }
        public static Options options;
        private int trackNameLength;
        private string spaces = "              ";
        private string trackName;
        private int scroll = 0;
        Spotify Spotify = new Spotify();
        public SpotifyLocalAPI _spotify { get { return Spotify._spotify; } set { Spotify._spotify = value; } }
        public SpotifyWebAPI spotifyWeb { get { return Spotify._spotifyWeb; } set { Spotify._spotifyWeb = value; }  }
        public Form1() {
            InitializeComponent();
        }

        public bool toast_isEnabled = false;
        public bool toast_fadeIn = false;
        public object newLocation = null;
        private static NotifyIcon notify = new NotifyIcon() {
            Icon = new Icon(@".\Resources\spotify-icon.ico"),
            Text = "Spotify Toast",
            Visible = true            
        };
        private ContextMenu notify_contextMenu = new System.Windows.Forms.ContextMenu();
        private MenuItem notify_menu1 = new MenuItem();
        private MenuItem notify_menu2 = new MenuItem();
        public ProgressBar progressBar1 = new ProgressBar() {
            Location = new Point(0, 69),
            BackColor = Color.FromArgb(255, 0, 0, 0),
            BarColor = Color.FromArgb(255, 100, 235, 100),
        };
        private void Form1_Load(object sender, EventArgs e) {
            if(!Spotify.Spotify_Load(this))
                return;
            progressBar1.Size = new Size(this.Size.Width, 3);
            progressBar1.Click += progressBar1_Click;
            progressBar1.Parent = this;
            if(notify.ContextMenu == null) {
                notify.DoubleClick += new EventHandler(this.notify_click);
                this.notify_contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.notify_menu1 });
                this.notify_contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.notify_menu2 });
                this.notify_menu1.Index = 0;
                this.notify_menu1.Text = "Open Options";
                this.notify_menu1.Click += new System.EventHandler(this.label3_click);
                this.notify_menu2.Index = 1;
                this.notify_menu2.Text = "E&xit";
                this.notify_menu2.Click += new System.EventHandler(this.notify_menu1_close);
            }
            notify.ContextMenu = this.notify_contextMenu;
            if(this.toast_isEnabled || confFile["toastMode"] != null && confFile["toastMode"].Value.ToLower().Equals("true")) {
                this.toast_isEnabled = true;
                this.FormBorderStyle = FormBorderStyle.None;
                if(!this.toast_fadeIn)
                    this.Location = new Point(width - 300, height);
                else
                    this.BackColor = Color.FromArgb(0, 255, 255, 255);
                toast_timer.Tick += new EventHandler(this.TimerTick_Toast_Push);
                toast_timer.Start();
                Console.WriteLine("Toasty");
            }
            else if(newLocation != null)
                this.Location = (Point)newLocation;
            UpdateTrack();
            //Fade(_spotify.GetStatus().Playing);
            scroll_timer.Tick += new EventHandler(this.TimerTick_Scroll);
            scroll_timer.Start();
            label3.Text = "v" + ToastTest.Program.version;
            Console.WriteLine("Spotify Toast started");
        }

        private void notify_menu1_close(object sender, EventArgs e) {
            this.close();
        }

        private void openOptionsMenu() {
            if(options == null || options.IsDisposed) {
                options = new Options();
                options._form1 = this;
                options.Show();
                options.Activate();
                if(!this.toast_isEnabled)
                    options.Location = this.Location;
            }
        }

        /// <summary>Form event</summary>
        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if(e.Control && e.KeyCode == Keys.O) {
                openOptionsMenu();
            }
        }

        /// <summary>Closes the application
        /// <para>Also disposes <see cref="SpotifyLocalAPI"/> and <see cref="Form1"/> </para>
        /// </summary>
        public bool close() {
            notify.Visible = false;
            notify.Dispose();
            _spotify.Dispose();
            this.Close();
            Console.WriteLine("Exiting...");
            return false;
        }
        Image bp = null;

        /// <summary>Returns the most used color based on a <see cref="Bitmap"/></summary>
        private static Color GetMostFrequentPixels(Bitmap b) {
            List<Color> list = new List<Color>();
            for(int x = 0; x < b.Width; x++) {
                for(int y = 0; y < b.Height; y++) {
                    Color pixel = b.GetPixel(x, y);
                    list.Add(pixel);
                }
            }
            var sort = list.GroupBy(item => item.ToString()).OrderByDescending(group => group.Count()).Select(g => g);
            return sort.First().First();
        }

        /// <summary>Update all info on the track
        /// <para><see cref="SpotifyLocalAPI.GetStatus()"/></para>
        /// </summary>
        public void UpdateTrack() {
            if(bp != null)
                bp.Dispose();
            if(this.toast_isEnabled) {
                Console.WriteLine("Toastiest");
                toast_timer.Start();
            }
            if(_spotify.GetStatus().Track != null && _spotify.GetStatus().Track.Length > 0) {
                Size s = new Size(64, 64);
                bp = new Bitmap(_spotify.GetStatus().Track.GetAlbumArt(AlbumArtSize.Size160), s);
                pictureBox1.Image = bp;
                SetColors();
            }
            else {
                text_songName.Text = (trackName = "No current song playing");
                trackNameLength = 0;
                text_artistName.Text = "No artist";
                progressBar1.Maximum = 0;
                return;
            }
            text_songName.Text = (trackName = _spotify.GetStatus().Track.TrackResource.Name + spaces);
            trackNameLength = trackName.Length;
            text_artistName.Text = _spotify.GetStatus().Track.ArtistResource.Name;
            progressBar1.Maximum = _spotify.GetStatus().Track.Length;
        }

        private Color currentColor;
        private void SetColors() {
            if(confFile["changeColorWithSong"] == null || !confFile["changeColorWithSong"].Value.ToLower().Equals("false")) {
                try {
                    Color color = GetMostFrequentPixels((Bitmap)bp);
                    int rV = 255 - color.R;
                    int gV = 255 - color.G;
                    int bV = 255 - color.B;
                    Color invert = Color.FromArgb(255, rV, gV, bV);
                    //progressBar1.BarColor = invert;
                    text_songName.ForeColor = invert;
                    text_artistName.ForeColor = invert;
                    this.BackColor = currentColor = Color.FromArgb(this.toast_fadeIn ? this.BackColor.A : color.A, color.R, color.G, color.B);
                }
                catch(Exception e) { Console.WriteLine(e.Message); }
            } else {
                if(confFile["defaultColor"] != null) {
                    int[] cS = Array.ConvertAll(confFile["defaultColor"].Value.Split(new[] { " " }, StringSplitOptions.None), int.Parse);
                    Color color = Color.FromArgb(255, cS[0], cS[1], cS[2]);
                    if(confFile["defaultTextColor"] == null || !confFile["defaultTextColor"].Value.ToLower().Equals("true")) {
                        Color invert = Color.FromArgb(255, 255 - color.R, 255 - color.G, 255 - color.B);
                        //progressBar1.BarColor = invert;
                        text_songName.ForeColor = invert;
                        text_artistName.ForeColor = invert;
                    }
                    this.BackColor = currentColor = Color.FromArgb(this.toast_fadeIn ? this.BackColor.A : color.A, color.R, color.G, color.B);
                }
                if(confFile["defaultTextColor"] != null) {
                    int[] cS = Array.ConvertAll(confFile["defaultTextColor"].Value.Split(new[] { " " }, StringSplitOptions.None), int.Parse);
                    Color color = Color.FromArgb(255, cS[0], cS[1], cS[2]);
                    text_songName.ForeColor = color;
                    text_artistName.ForeColor = color;
                }
            }
        }


        public Timer toast_timer = new Timer() { Interval = 5 };
        private int toast_ticks = 0;
        private bool toast_isOnTop = false;
        /// <summary>Make the toast push up from the screen</summary>
        private void TimerTick_Toast_Push(object sender, EventArgs e) {
            int x = this.Location.X;
            int y = this.Location.Y;
            int width = SystemInformation.VirtualScreen.Width;
            int height = SystemInformation.VirtualScreen.Height;
            if(!this.toast_fadeIn) { 
                if(x <= (width - 300) && y <= (height - 120) && !toast_isOnTop) {
                    toast_ticks += 1;
                    if(toast_ticks >= 65)
                        toast_isOnTop = true;
                    return;
                } else if(toast_isOnTop && toast_ticks >= 65) {
                    Point p = new Point(x, y + 1);
                    this.Location = p;
                    if(x >= (width - 300) && y >= height) {
                        toast_isOnTop = false;
                        toast_ticks = 0;
                        toast_timer.Stop();
                    }
                    return;
                }
                Point point = new Point(x, y - 1);
                this.Location = point;
            } else { 
                if(this.BackColor.A >= 255 && !toast_isOnTop) {
                    toast_ticks += 1;
                    if(toast_ticks >= 65)
                        toast_isOnTop = true;
                    return;
                } else if(toast_isOnTop && toast_ticks >= 65) {
                    this.BackColor = this.BackColor = Color.FromArgb(BackColor.A - 1, BackColor.R, BackColor.G, BackColor.B);
                    if(this.BackColor.A >= 255) {
                        toast_isOnTop = false;
                        toast_ticks = 0;
                        toast_timer.Stop();
                    }
                    return;
                }
                this.BackColor = Color.FromArgb(BackColor.A + 1, BackColor.R, BackColor.G, BackColor.B);
            }
        }

        Timer scroll_timer = new Timer() { Interval = 250 };
        private long time = 0;
        private int ticksBeforeStop = 0;
        /// <summary>Scroll the song name's text</summary>
        private void TimerTick_Scroll(object sender, EventArgs e) {
            int a = 34;
            DateTime now = DateTime.Now;
            if(trackNameLength < a || ((now.Ticks / 600) - time) <= 50000)
                return;
            scroll += 1;
            if((scroll + a) >= trackNameLength) {
                ticksBeforeStop += 1;
                if(ticksBeforeStop >= 10) {
                    time = now.Ticks / 600;
                    scroll = 0;
                    ticksBeforeStop = 0;
                    text_songName.Text = trackName.Substring(0, a);
                    return;
                }
            }
            string sub = trackName.Substring(scroll);
            if(sub.Length > a) {
                text_songName.Text = sub.Substring(0, a);
            }
        }

        /// <summary>Fade out/in ui based on if Spotify is playing
        /// <para><see cref="SpotifyLocalAPI.GetStatus()"/></para>
        /// </summary>
        public void Fade(bool playing) {
            Console.WriteLine("Music " + (playing ? "started" : "stopped"));
            if (!playing) {
                /*using (Graphics g = Graphics.FromImage(image)) {
                    Pen pen = new Pen(Color.FromArgb(120, 15, 15, 15), image.Width);
                    g.DrawLine(pen, -1, -1, image.Width, image.Height);
                    g.Save();
                    pen.Dispose();
                    g.Dispose();
                }
                pictureBox1.Image = image;
                bp = oldImage;*/
                Bitmap b = (Bitmap)bp;
                for(int x = 0; x < b.Width; x++) {
                    for(int y = 0; y < b.Height; y++) {
                        Color c = b.GetPixel(x, y);
                        Color newPixel = Color.FromArgb(c.A, (int) (c.R * 0.5), (int) (c.G * 0.5), (int) (c.B * 0.5));
                        b.SetPixel(x, y, newPixel);
                    }
                }
                this.BackColor = GetMostFrequentPixels(b);
                pictureBox1.Image = bp;
                text_songName.ForeColor = Color.FromArgb(255, 15, 15, 15);
                text_artistName.ForeColor = Color.FromArgb(255, 15, 15, 15);
            } else {
                if(bp != null) {
                    pictureBox1.Image = null;
                    bp.Dispose();
                }
                pictureBox1.Image = bp = new Bitmap(_spotify.GetStatus().Track.GetAlbumArt(AlbumArtSize.Size160), new Size(64, 64));
                SetColors();
            }
        }

        private void notify_click(object Sender, EventArgs e) {
            openOptionsMenu();
        }
        private void label3_mouseEnter(object sender, EventArgs e) {
            label3.BackColor = Color.FromArgb(255, 255-currentColor.R, 255-currentColor.G, 255-currentColor.B);
        }
        private void label3_mouseLeave(object sender, EventArgs e) {
            label3.BackColor = Color.FromArgb(0, 29, 29, 29);
        }
        private void label3_click(object sender, EventArgs e) {
            openOptionsMenu();
        }

        private void progressBar1_Click(object sender, EventArgs e) {
            //float absoluteMouse = (PointToClient(MousePosition).X - progressBar1.Bounds.X);
            //float c = progressBar1.Width / 100f;
            //float relativeMouse = absoluteMouse / c;
            //Convert.ToInt32(relativeMouse) * (_spotify.GetStatus().Track.Length * 0.01);
            // - I can't do anything with this right now. Need to figure out a bit more.
            
            if (_spotify.GetStatus().Playing)
                _spotify.Pause();
            else
                _spotify.Play();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            notify.Visible = false;
            notify.Icon = null;
        }
    }
}
