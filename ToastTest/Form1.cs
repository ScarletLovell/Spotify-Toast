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
        private spotifyCode spotifyCode = new spotifyCode();
        public static Options options;
        private int trackNameLength;
        private string spaces = "              ";
        private string trackName;
        private int scroll = 0;
        private static SpotifyLocalAPI _spotify;
        private static SpotifyWebAPI _spotifyWeb;
        public SpotifyWebAPI spotifyWeb { get { return _spotifyWeb; } set { _spotifyWeb = value; }  }

        private static NotifyIcon notify = new NotifyIcon();
        public Form1() {
            InitializeComponent();
        }

        /// <summary>Form event</summary>
        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if(e.Control && e.KeyCode == Keys.O) {
                openOptionsMenu();
            }
        }

        /// <summary>Spotify event</summary>
        private void _spotify_OnPlayStateChange(object sender, PlayStateEventArgs e) {
            if(InvokeRequired) {
                Invoke(new Action(() => _spotify_OnPlayStateChange(sender, e)));
                return;
            }
            fade(e.Playing);
        }
        /// <summary>Spotify event</summary>
        private void _spotify_OnTrackChange(object sender, TrackChangeEventArgs e) {
            if (InvokeRequired) {
                Invoke(new Action(() => _spotify_OnTrackChange(sender, e)));
                return;
            }
            updateTrack();
        }
        /// <summary>Spotify event</summary>
        private void _spotify_OnTrackTimeChange(object sender, TrackTimeChangeEventArgs e) {
            if(InvokeRequired) {
                Invoke(new Action(() => _spotify_OnTrackTimeChange(sender, e)));
                return;
            }
            if(!_spotify.GetStatus().Playing)
                return;
            progressBar1.Value = (int) e.TrackTime;
            //_spotify.GetStatus().PlayingPosition
        }
        /// <summary>Check if the application can use Spotify properly
        /// <para><see cref="SpotifyLocalAPI"/></para>
        /// </summary>
        public bool doSpotifyCheck() {
            if (_spotify == null)
                _spotify = new SpotifyLocalAPI();
            if (!SpotifyLocalAPI.IsSpotifyRunning() || !SpotifyLocalAPI.IsSpotifyWebHelperRunning() || !_spotify.Connect()) {
                DialogResult result = MessageBox.Show("Unable to connect to spotify, retry?", "Spotify Toast", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    return doSpotifyCheck();
                close();
                return false;
            }
            _spotify.ListenForEvents = true;
            _spotify.SynchronizingObject = this;
            return true;
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
        public static Color GetMostFrequentPixels(Bitmap b) {
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

        private Color currentColor;
        /// <summary>Update all info on the track
        /// <para><see cref="SpotifyLocalAPI.GetStatus()"/></para>
        /// </summary>
        private void updateTrack() {
            if (bp != null)
                bp.Dispose();
            if(this.isToast) {
                Console.WriteLine("Toastiest");
                toast_pushUpTimer.Start();
            }
            if (_spotify.GetStatus().Track != null && _spotify.GetStatus().Track.Length > 0) {
                Size s = new Size(64, 64);
                bp = new Bitmap(_spotify.GetStatus().Track.GetAlbumArt(AlbumArtSize.Size160), s);
                pictureBox1.Image = bp;
                SetColors();
            } else {
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
        private void SetColors() {
            if(confFile["changeColorWithSong"] == null || !confFile["changeColorWithSong"].Value.ToLower().Equals("false")) {
                try {
                    Color color = GetMostFrequentPixels((Bitmap)bp);
                    int r = ((Math.Abs(255 - color.R) < 50) && (Math.Abs(255 - color.R) > 5) ? 215 - color.R : 255 - color.R);
                    int g = ((Math.Abs(255 - color.G) < 50) && (Math.Abs(255 - color.G) > 5) ? 215 - color.G : 255 - color.G);
                    int b = ((Math.Abs(255 - color.B) < 50) && (Math.Abs(255 - color.B) > 5) ? 215 - color.B : 255 - color.B);
                    Color invert = Color.FromArgb(255, 255 - color.R, 255 - color.G, 255 - color.B);
                    progressBar1.BarColor = invert;
                    text_songName.ForeColor = invert;
                    text_artistName.ForeColor = invert;
                    currentColor = this.BackColor = color;
                }
                catch(Exception e) { Console.WriteLine(e.Message); }
            } else {
                if(confFile["defaultColor"] != null) {
                    int[] cS = Array.ConvertAll(confFile["defaultColor"].Value.Split(new[] { " " }, StringSplitOptions.None), int.Parse);
                    Color color = Color.FromArgb(255, cS[0], cS[1], cS[2]);
                    if(confFile["defaultTextColor"] == null || !confFile["defaultTextColor"].Value.ToLower().Equals("true")) {
                        Color invert = Color.FromArgb(255, 255 - color.R, 255 - color.G, 255 - color.B);
                        progressBar1.BarColor = invert;
                        text_songName.ForeColor = invert;
                        text_artistName.ForeColor = invert;
                    }
                    currentColor = this.BackColor = color;
                }
                if(confFile["defaultTextColor"] != null) {
                    int[] cS = Array.ConvertAll(confFile["defaultTextColor"].Value.Split(new[] { " " }, StringSplitOptions.None), int.Parse);
                    Color color = Color.FromArgb(255, cS[0], cS[1], cS[2]);
                    text_songName.ForeColor = color;
                    text_artistName.ForeColor = color;
                }
            }
        }


        Timer toast_pushUpTimer = new Timer() { Interval = 10 };
        private int toast_ticks = 0;
        private bool toast_isOnTop = false;
        /// <summary>Make the toast push up from the screen</summary>
        private void TimerTick_Toast_Push(object sender, EventArgs e) {
            int x = this.Location.X;
            int y = this.Location.Y;
            int width = SystemInformation.VirtualScreen.Width;
            int height = SystemInformation.VirtualScreen.Height;
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
                    toast_pushUpTimer.Stop();
                }
                return;
            }
            Point point = new Point(x, y - 1);
            this.Location = point;
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
        public bool isToast = false;
        public Object newLocation = null;
        ProgressBar progressBar1;
        private void Form1_Load(object sender, EventArgs e) {
            if (!doSpotifyCheck())
                return;
            progressBar1 = new ProgressBar();
            progressBar1.Location = new Point(0, 69);
            progressBar1.Size = new Size(this.Size.Width, 3);
            progressBar1.BackColor = Color.FromArgb(255, 0, 0, 0);
            progressBar1.BarColor = Color.FromArgb(255, 50, 75, 50);
            progressBar1.Click += progressBar1_Click;
            progressBar1.Parent = this;
            if(this.isToast || confFile["toastMode"] != null && confFile["toastMode"].Value.ToLower().Equals("true")) {
                this.isToast = true;
                this.FormBorderStyle = FormBorderStyle.None;
                int width = SystemInformation.VirtualScreen.Width;
                int height = SystemInformation.VirtualScreen.Height;
                this.Location = new Point(width - 300, height);
                toast_pushUpTimer.Tick += new EventHandler(this.TimerTick_Toast_Push);
                toast_pushUpTimer.Start();
                notify.Icon = new Icon(@".\Resources\spotify-icon.ico");
                notify.Text = "Spotify Toast";
                notify.BalloonTipText = "test";
                notify.BalloonTipTitle = "test2";
                notify.ShowBalloonTip(2000, "hi", "there", ToolTipIcon.Info);
                notify.Visible = true;
                notify.DoubleClick += new EventHandler(this.notify_click);
                Console.WriteLine("Toasty");
            } else if(newLocation != null)
                this.Location = (Point) newLocation;
            _spotify.OnTrackChange += _spotify_OnTrackChange;
            _spotify.OnTrackTimeChange += _spotify_OnTrackTimeChange;
            _spotify.OnPlayStateChange += _spotify_OnPlayStateChange;
            updateTrack();
            fade(_spotify.GetStatus().Playing);
            scroll_timer.Tick += new EventHandler(this.TimerTick_Scroll);
            scroll_timer.Start();
            label3.Text = "v"+ToastTest.Program.version;
            Console.WriteLine("Spotify Toast started");
        }
        private void openOptionsMenu() {
            if(options == null || options.IsDisposed) {
                options = new Options();
                options._form1 = this;
                options.Show();
                options.Activate();
                if(!this.isToast)
                    options.Location = this.Location;
            }
        }

        /// <summary>Fade out/in ui based on if Spotify is playing
        /// <para><see cref="SpotifyLocalAPI.GetStatus()"/></para>
        /// </summary>
        private void fade(bool playing) {
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
                Bitmap b = (Bitmap) bp.Clone();
                for(int x = 0; x < b.Width; x++) {
                    for(int y = 0; y < b.Height; y++) {
                        Color c = b.GetPixel(x, y);
                        Color newPixel = Color.FromArgb(c.A, (int) (c.R * 0.5), (int) (c.G * 0.5), (int) (c.B * 0.5));
                        b.SetPixel(x, y, newPixel);
                    }
                }
                this.BackColor = GetMostFrequentPixels(b);
                bp = b;
                pictureBox1.Image = bp;
                text_songName.ForeColor = Color.FromArgb(255, 15, 15, 15);
                text_artistName.ForeColor = Color.FromArgb(255, 15, 15, 15);
            } else {
                if(bp != null)
                    bp.Dispose();
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
