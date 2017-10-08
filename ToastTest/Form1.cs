using System;
using System.Windows.Forms;

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
        protected override CreateParams CreateParams
        { // https://stackoverflow.com/a/26607006 , I couldn't figure this out on my own.
            get {
                var cp = base.CreateParams;
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confFile = configManager.AppSettings.Settings;
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
        private Timer scroll_timer;
        private static SpotifyLocalAPI _spotify;
        private static SpotifyWebAPI _spotifyWeb;
        public SpotifyWebAPI spotifyWeb { get { return _spotifyWeb; } set { _spotifyWeb = value; }  }
        public Form1() {
            InitializeComponent();
        }

        /// <summary>Form event</summary>
        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if(e.Control && e.KeyCode == Keys.O) {
                if (options == null || options.IsDisposed) {
                    options = new Options();
                    options._form1 = this;
                    options.Show();
                    options.Activate();
                    options.Location = this.Location;
                }
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
            if (_spotify.GetStatus().Playing || _spotify.GetStatus().Track != null) {
                Size s = new Size(64, 64);
                bp = new Bitmap(_spotify.GetStatus().Track.GetAlbumArt(AlbumArtSize.Size160), s);
                pictureBox1.Image = bp;
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confFile = configManager.AppSettings.Settings;
                if(confFile["changeColorWithSong"] == null || !confFile["changeColorWithSong"].Value.ToLower().Equals("false")) {
                    try {
                        Color color = GetMostFrequentPixels((Bitmap)bp);
                        Color invert = Color.FromArgb(255, 255 - color.R, 255 - color.G, 255 - color.B);
                        text_songName.ForeColor = invert;
                        text_artistName.ForeColor = invert;
                        currentColor = this.BackColor = color;
                    }
                    catch(Exception e) { Console.WriteLine(e.Message); }
                }
            }
            trackName = _spotify.GetStatus().Track.TrackResource.Name + spaces;
            text_songName.Text = trackName;
            trackNameLength = trackName.Length;
            text_artistName.Text = _spotify.GetStatus().Track.ArtistResource.Name;
            progressBar1.Maximum = _spotify.GetStatus().Track.Length;
            Scroll();
        }
        private void TimerTick(object sender, EventArgs e) {
            Scroll();
        }
        private long time = 0;
        private int ticksBeforeStop = 0;

        /// <summary>Scroll the song name's text</summary>
        private new void Scroll() {
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
        public Object newLocation = null;
        private void Form1_Load(object sender, EventArgs e) {
            if (!doSpotifyCheck())
                return;
            if(newLocation != null) {
                this.Location = (Point) newLocation;
            }
            _spotify.OnTrackChange += _spotify_OnTrackChange;
            _spotify.OnTrackTimeChange += _spotify_OnTrackTimeChange;
            _spotify.OnPlayStateChange += _spotify_OnPlayStateChange;
            updateTrack();
            scroll_timer = new Timer();
            scroll_timer.Tick += new EventHandler(this.TimerTick);
            scroll_timer.Interval = 250;
            scroll_timer.Start();
            label3.Text = "v"+ToastTest.Program.version;
            this.KeyDown += Form1_KeyDown;
            Console.WriteLine("Spotify Toast started");
        }

        /// <summary>Fade out/in ui based on if Spotify is playing
        /// <para><see cref="SpotifyLocalAPI.GetStatus()"/></para>
        /// </summary>
        private void fade(bool playing) {
            Console.WriteLine("Music " + (playing ? "started" : "stopped"));
            if (!playing) {
                Image oldImage = bp;
                Image image = pictureBox1.Image;
                using (Graphics g = Graphics.FromImage(image)) {
                    Pen pen = new Pen(Color.FromArgb(120, 15, 15, 15), image.Width);
                    g.DrawLine(pen, -1, -1, image.Width, image.Height);
                    g.Save();
                    pen.Dispose();
                    g.Dispose();
                }
                pictureBox1.Image = image;
                bp = oldImage;
                text_songName.ForeColor = Color.FromArgb(255, 15, 15, 15);
                text_artistName.ForeColor = Color.FromArgb(255, 15, 15, 15);
            } else {
                text_songName.ForeColor = Color.FromArgb(255, 224, 224, 224);
                text_artistName.ForeColor = Color.FromArgb(255, 224, 224, 224);
                if (bp != null)
                    bp.Dispose();
                pictureBox1.Image = bp = new Bitmap(_spotify.GetStatus().Track.GetAlbumArt(AlbumArtSize.Size160), new Size(64, 64));
            }
        }

        private void label3_mouseEnter(object sender, EventArgs e) {
            label3.BackColor = Color.FromArgb(255, 255-currentColor.R, 255-currentColor.G, 255-currentColor.B);
        }
        private void label3_mouseLeave(object sender, EventArgs e) {
            label3.BackColor = Color.FromArgb(0, 29, 29, 29);
        }
        private void label3_click(object sender, EventArgs e) {
            if(options == null || options.IsDisposed) {
                options = new Options();
                options._form1 = this;
                options.Show();
                options.Activate();
                options.Location = this.Location;
            }
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
    }
}
