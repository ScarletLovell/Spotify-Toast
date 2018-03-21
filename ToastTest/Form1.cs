using System;
using System.Windows.Forms;
using ToastTest;

namespace ToastTest
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SpotifyAPI.Local;
    using SpotifyAPI.Local.Enums;
    using SpotifyAPI.Web;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    public partial class Form1 : Form, IMessageFilter {

        public static List<Song> songHistory { get;set; }
        /// <summary>The way to make the app force ontop of all apps</summary>
        interface IMouseClickable {
            void HandleMouseClick(object sender, MouseEventArgs e);
        }
        int width = SystemInformation.VirtualScreen.Width;
        int height = SystemInformation.VirtualScreen.Height;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_LBUTTONDOWN = 0x0201;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        protected override CreateParams CreateParams
        { // https://stackoverflow.com/a/26607006 , I couldn't figure this out on my own.
            get {
                var cp = base.CreateParams;
                if(!JsonOptions.GetAllOptions() || JsonOptions.GetOption("alwaysOnTop") != "true") {
                    return cp;
                }
                cp.ExStyle |= 8;  // Turn on WS_EX_TOPMOST
                return cp;
            }
        }
        public static Options options;
        private int trackNameLength;
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
        public class Song {
            public string artist { get; set; }
            public string name { get; set; }
            public int plays { get; set; }
        };
        List<string> SongsByArtist = new List<string>();
        bool noBorder = false;
        public void load(bool colors) {
        // wow this part is bad.
            Console.WriteLine("load up");
            toast_timer.Stop();
            if(JsonOptions.GetAllOptions()) {
                if(colors)
                    SetColors();
                if(JsonOptions.GetOption("showSongBorder") != "") {
                    bool songBorder = JsonOptions.GetOption("showSongBorder").Equals("true");
                    pictureBox1.BorderStyle = (songBorder ? BorderStyle.Fixed3D : BorderStyle.None);
                }
                if(JsonOptions.GetOption("disableBorder") != "") {
                    noBorder = bool.Parse(JsonOptions.GetOption("disableBorder"));
                    this.FormBorderStyle = (noBorder ? FormBorderStyle.None : FormBorderStyle.FixedToolWindow);
                }
                if(JsonOptions.GetOption("barColor") != "") {
                    int[] cS = Array.ConvertAll(JsonOptions.GetOption("barColor").Split(new[] { " " }, StringSplitOptions.None), int.Parse);
                    progressBar1.BarColor = Color.FromArgb(255, cS[0], cS[1], cS[2]);
                }
                if(JsonOptions.GetOption("barHeight") != "") {
                    int barHeight = int.Parse(JsonOptions.GetOption("barHeight"));
                    progressBar1.Size = new Size(this.Size.Width, 3 + barHeight);
                    progressBar1.Location = new Point(0, 69 - barHeight);
                    label_version.Location = new Point(246, 55 - barHeight);
                }
                if(this.toast_isEnabled || JsonOptions.GetOption("toastMode").Equals("true")) {
                    this.ShowInTaskbar = false;
                    string btt = "Spotify Toast";
                    string _btt = "Spotify Toast is now hidden to the system tray";
                    notify.ShowBalloonTip(5000, btt, _btt, ToolTipIcon.Info);
                    toast_isEnabled = true;
                    this.FormBorderStyle = FormBorderStyle.None;
                    if(this.toast_fadeIn)
                        this.BackColor = Color.FromArgb(0, 255, 255, 255);
                    toast_timer.Tick += new EventHandler(this.TimerTick_Toast_Push);
                    toast_timer.Start();
                } else if(toast_isEnabled) {
                    this.ShowInTaskbar = true;
                    this.toast_isEnabled = false;
                    this.CenterToScreen();
                    toast_timer.Stop();
                }
                if(JsonOptions.GetOption("dontShowVersion") != "") {
                    label_version.Visible = !JsonOptions.GetOption("dontShowVersion").Equals("true");
                }
                if(JsonOptions.GetOption("showAmountOfPlays") != "") {
                    label1.Visible = JsonOptions.GetOption("showAmountOfPlays").Equals("true");
                    text_amountOfPlays.Visible = JsonOptions.GetOption("showAmountOfPlays").Equals("true");
                }
                if(JsonOptions.GetOption("textTicks") != "") {
                    int tick = 250;
                    if(!int.TryParse(JsonOptions.GetOption("textTicks"), out tick))
                        tick = 250;
                    if(this.scroll_timer.Interval != tick)
                        this.scroll_timer.Interval = tick;
                }
                if(JsonOptions.GetOption("toastTicks") != "") {
                    int tick = 5;
                    if(int.TryParse(JsonOptions.GetOption("toastTicks"), out tick)) {
                        toast_timer.Interval = tick;
                    }
                }
                if(JsonOptions.GetOption("toastStickTime") != "") {
                    int tick = 5;
                    if(int.TryParse(JsonOptions.GetOption("toastStickTime"), out tick)) {
                        toast_stickTime = tick;
                    }
                }
                if(JsonOptions.GetOption("toastPosition") != "") {
                    string pos = JsonOptions.GetOption("toastPosition");
                    if(pos == "Bottom Right") {
                        toast_placement = toastPlacement.bottom_right;
                    } else if(pos == "Bottom Left") {
                        toast_placement = toastPlacement.bottom_left;
                    } else if(pos == "Bottom Center") {
                        toast_placement = toastPlacement.bottom_center;
                    } else if(pos == "Top Left") {
                        toast_placement = toastPlacement.top_left;
                    } else if(pos == "Top Center") {
                        toast_placement = toastPlacement.top_center;
                    } else if(pos == "Top Right") {
                        toast_placement = toastPlacement.top_right;
                    }
                }
            }
            if(newLocation != null)
                this.Location = (Point)newLocation;
            UpdateTrack(true);
        }
        private HashSet<Control> dragControls = new HashSet<Control>();
        public bool PreFilterMessage(ref Message m) {
            if(m.Msg == WM_LBUTTONDOWN && dragControls.Contains(Control.FromHandle(m.HWnd)) && noBorder) {
                if(toast_isEnabled)
                    return false;
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                return true;
            }
            return false;
        }
        private void Form1_Load(object sender, EventArgs e) {
            if(!Spotify.Spotify_Load(this))
                return;
            Application.AddMessageFilter(this);
            dragControls.Add(this);
            dragControls.Add(text_artistName);
            dragControls.Add(text_songName);
            dragControls.Add(text_amountOfPlays);
            dragControls.Add(label1);
            dragControls.Add(pictureBox1);
            progressBar1.Size = new Size(this.Size.Width, 3);
            progressBar1.Click += progressBar1_Click;
            progressBar1.Parent = this;

            scroll_timer.Tick += new EventHandler(this.TimerTick_Scroll);
            scroll_timer.Start();
            label_version.Text = "v" + ToastTest.Program.version;
            load(false);
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
            /*notify.BalloonTipClosed += (_sender, _e) => {
                var thisIcon = (NotifyIcon) _sender;
                thisIcon.Visible = false;
                thisIcon.Dispose();
            };*/
            if(JsonOptions.GetOption("showAmountOfPlays").Equals("true")) {
                JsonSerializer serializer = new JsonSerializer();
                using(FileStream fs = File.Open("./songs.json", FileMode.Open))
                using(StreamReader sr = new StreamReader(fs))
                using(JsonReader reader = new JsonTextReader(sr)) {
                    while(reader.Read()) {
                        if(reader.TokenType == JsonToken.StartObject) {
                            Song _song = serializer.Deserialize<Song>(reader);
                            JObject song = new JObject(
                                new JProperty("artist", _song.artist),
                                new JProperty("name", _song.name),
                                new JProperty("plays", _song.plays)
                            );
                            songs.Add(song);
                        }
                    }
                    reader.Close();
                    sr.Close();
                    fs.Close();
                }
            }
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
                else {
                    Screen screen = Screen.FromControl(this);
                    Rectangle workingArea = screen.WorkingArea;
                    /*this.Location = new Point() {
                        X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - this.Width) / 2),
                        Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - this.Height) / 2)
                    };*/
                }
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

        private JArray songs = new JArray();

        /// <summary>Update all info on the track
        /// <para><see cref="SpotifyLocalAPI.GetStatus()"/></para>
        /// </summary>
        public void UpdateTrack(bool canAdd) {
            if(bp != null)
                bp.Dispose();
            if(this.toast_isEnabled) {
                toast_timer.Start();
            }
            if(_spotify.GetStatus().Track != null && _spotify.GetStatus().Track.Length > 0 && _spotify.GetStatus().Running) {
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
            text_songName.Text = (trackName = _spotify.GetStatus().Track.TrackResource.Name + "              ");
            trackNameLength = trackName.Length;
            text_artistName.Text = _spotify.GetStatus().Track.ArtistResource.Name;
            progressBar1.Maximum = _spotify.GetStatus().Track.Length;
            if(JsonOptions.GetOption("showAmountOfPlays").Equals("true") && canAdd) {
                int plays = 1;
                string songName = text_songName.Text.Trim();
                if(songName.Length > 10) {
                    songName = songName.Substring(0, 10);
                }
                string artistName = text_artistName.Text.Trim();
                if(artistName.Length > 7) {
                    artistName = artistName.Substring(0, 7);
                }

                for(int i = 0; i < songs.Count; i++) {
                    JObject _song = (JObject)songs[i];
                    if(((string) _song.GetValue("name")) == songName && ((string)_song.GetValue("artist")) == artistName) {
                        string _p = (string) _song.GetValue("plays");
                        if(Int32.TryParse(_p, out plays)) {
                            plays += 1;
                        } else {
                            plays = 1;
                        }
                        songs.RemoveAt(i);
                        break;
                    }
                }
                JObject song = new JObject(
                    new JProperty("artist", artistName),
                    new JProperty("name", songName),
                    new JProperty("plays", plays)
                );
                if(plays > 1) {
                    JObject oldSong = new JObject(
                        new JProperty("artist", artistName),
                        new JProperty("name", songName),
                        new JProperty("plays", plays - 1)
                    );
                    if(songs.Contains(oldSong)) {
                        songs.Remove(oldSong);
                    }
                }
                songs.Add(song);


                /*Song _Song = new Song();
                _Song.artist = text_artistName.Text;
                _Song.name = text_songName.Text.Trim();
                _Song.plays = plays;
                if(songHistory.Count >= 5) {
                    songHistory.RemoveAt(5);
                }
                songHistory.Add(_Song);*/
                
                string json = JsonConvert.SerializeObject(songs);
                File.WriteAllText("./songs.json", json);
                text_amountOfPlays.Text = plays.ToString();
            }
        }

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

        private Color currentColor;
        private void SetColors() {
            if(JsonOptions.GetOption("changeColorWithSong").Equals("true")) {
                try {
                    Color color = GetMostFrequentPixels((Bitmap)bp);
                    double rV = 255 - color.R; // R
                    double gV = 255 - color.G; // G
                    double bV = 255 - color.B; // B
                    double rV_comp = rV - color.R; // color - whatever color was inverted
                    double gV_comp = rV - color.G;
                    double bV_comp = bV - color.B;
                    Console.WriteLine(rV_comp + " " + gV_comp + " " + bV_comp);
                    if(rV_comp <= 50 && rV_comp >= -50)
                        rV += (rV <= 55 ? 200 : (rV > 200 ? -150 : (rV > 100 ? -100 : -50)));
                    if(gV_comp <= 50 && gV_comp >= -50)
                        gV += (gV <= 55 ? 200 : (gV > 200 ? -150 : (gV > 100 ? -100 : -50)));
                    if(bV_comp <= 50 && bV_comp >= -50)
                        bV += (bV <= 55 ? 200 : (bV > 200 ? -150 : (bV > 100 ? -100 : -50)));
                    Console.WriteLine(rV + ", " + color.R);
                    Console.WriteLine(gV + ", " + color.G);
                    Console.WriteLine(bV + ", " + color.B);
                    Color invert = Color.FromArgb(255, (int) rV, (int) gV, (int) bV);
                    //progressBar1.BarColor = invert;
                    text_songName.ForeColor = invert;
                    text_artistName.ForeColor = invert;
                    label1.ForeColor = invert;
                    label_version.ForeColor = invert;
                    text_amountOfPlays.ForeColor = invert;
                    this.BackColor = currentColor = Color.FromArgb(this.toast_fadeIn ? this.BackColor.A : color.A, color.R, color.G, color.B);
                }
                catch(Exception e) { Console.WriteLine(e.Message); }
            } else {
                if(JsonOptions.GetOption("defaultColor") != "") {
                    int[] cS = Array.ConvertAll(JsonOptions.GetOption("defaultColor").Split(new[] { " " }, StringSplitOptions.None), int.Parse);
                    Color color = Color.FromArgb(255, cS[0], cS[1], cS[2]);
                    if(!JsonOptions.GetOption("defaultTextColor").Equals("true")) {
                        Color invert = Color.FromArgb(255, 255 - color.R, 255 - color.G, 255 - color.B);
                        //progressBar1.BarColor = invert;
                        text_songName.ForeColor = invert;
                        text_artistName.ForeColor = invert;
                    }
                    this.BackColor = currentColor = Color.FromArgb(this.toast_fadeIn ? this.BackColor.A : color.A, color.R, color.G, color.B);
                }
                if(JsonOptions.GetOption("defaultTextColor") != "") {
                    int[] cS = Array.ConvertAll(JsonOptions.GetOption("defaultTextColor").Split(new[] { " " }, StringSplitOptions.None), int.Parse);
                    Color color = Color.FromArgb(255, cS[0], cS[1], cS[2]);
                    text_songName.ForeColor = color;
                    text_artistName.ForeColor = color;
                }
            }
        }


        public Timer toast_timer = new Timer() { Interval = 5 };
        private int toast_ticks = 0;
        private int toast_stickTime = 120;
        private bool toast_isOnTop = false;
        enum toastPlacement : int {
            bottom_right = 0,
            bottom_center = 1,
            bottom_left = 2,
            top_right = 3,
            top_center = 4,
            top_left = 5
        };
        private toastPlacement toast_placement = toastPlacement.bottom_right;
        bool isNewToast = true;
        /// <summary>Make the toast push up from the screen</summary>
        private void TimerTick_Toast_Push(object sender, EventArgs e) {
            int x = this.Location.X;
            int y = this.Location.Y;
            int width = SystemInformation.VirtualScreen.Width;
            int height = SystemInformation.VirtualScreen.Height;
            int down = height - 120;
            if(isNewToast && x != width) {
                isNewToast = false;
                if(toast_placement == toastPlacement.bottom_right) {
                    this.Location = new Point(width - 300, down);
                } else if(toast_placement == toastPlacement.bottom_center) {
                    this.Location = new Point(width / 2 - (this.Size.Width / 2), down);
                } else if(toast_placement == toastPlacement.bottom_left) {
                    this.Location = new Point(width - width + 10, down);
                }/* else if(toast_placement == toastPlacement.top_right) {
                    this.Location = new Point(width / 2, y);
                } else if(toast_placement == toastPlacement.top_center) {
                    this.Location = new Point(width / 2, y);
                } else if(toast_placement == toastPlacement.top_left) {
                    this.Location = new Point(width / 2, y);
                }*/
            }
            if(!this.toast_fadeIn) { 
                if(y <= (height - 120) && !toast_isOnTop) {
                    toast_ticks += 1;
                    if(toast_ticks >= toast_stickTime)
                        toast_isOnTop = true;
                    return;
                } else if(toast_isOnTop && toast_ticks >= toast_stickTime) {
                    Point p = new Point(x, y + 1);
                    this.Location = p;
                    if(y >= height) {
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
                    if(toast_ticks >= toast_stickTime)
                        toast_isOnTop = true;
                    return;
                } else if(toast_isOnTop && toast_ticks >= toast_stickTime) {
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

        private Timer scroll_timer = new Timer() { Interval = 250 };
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
            label_version.BackColor = Color.FromArgb(255, 255-currentColor.R, 255-currentColor.G, 255-currentColor.B);
        }
        private void label3_mouseLeave(object sender, EventArgs e) {
            label_version.BackColor = Color.FromArgb(0, 29, 29, 29);
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
            notify.Dispose();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e) {
            if(e.KeyChar == (char)Keys.Left) {
                Console.WriteLine("key left");
                _spotify.Previous();
            }
        }
    }
}
