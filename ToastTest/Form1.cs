using System;
using System.Windows.Forms;

namespace ToastTest
{
    using SpotifyAPI.Local;
    using SpotifyAPI.Local.Enums;
    using System.Drawing;

    public partial class Form1 : Form
    {
        public spotifyCode spotifyCode;
        protected override CreateParams CreateParams
        { // https://stackoverflow.com/a/26607006 , I couldn't figure this out on my own.
            get {
                var cp = base.CreateParams;
                cp.ExStyle |= 8;  // Turn on WS_EX_TOPMOST
                return cp;
            }
        }

        private static spotifyCode spotify;
        public static Options options;
        private int trackNameLength;
        private string spaces = "              ";
        private string trackName;
        private int scroll = 0;
        private Timer scroll_timer;
        private static SpotifyLocalAPI _spotify;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if(e.Control && e.KeyCode == Keys.O) {
                if (options == null || options.IsDisposed) {
                    options = new Options();
                    options.Show();
                    options.Activate();
                    options.Location = this.Location;
                }
            }
        }
        private void _spotify_OnPlayStateChange(object sender, PlayStateEventArgs e) {
            pictureFade(e.Playing);
        }
        private void _spotify_OnTrackChange(object sender, TrackChangeEventArgs e) {
            if (InvokeRequired) {
                Invoke(new Action(() => _spotify_OnTrackChange(sender, e)));
                return;
            }
            updateTrack();
        }
        private void _spotify_OnTrackTimeChange(object sender, TrackTimeChangeEventArgs e) {
            progressBar1.Value = (int) e.TrackTime;
        }

        public bool doSpotifyCheck()
        {
            if (_spotify == null)
                _spotify = new SpotifyLocalAPI();
            if (!SpotifyLocalAPI.IsSpotifyRunning() || !SpotifyLocalAPI.IsSpotifyWebHelperRunning() || !_spotify.Connect())
            {
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

        public bool close() {
            _spotify.Dispose();
            this.Close();
            Console.WriteLine("Exiting...");
            return false;
        }
        Image bp = null;
        private void updateTrack() {
            if (bp != null)
                bp.Dispose();
            if(_spotify.GetStatus().Track == null) {
                //updateTrack();
                return;
            }
            Size s = new Size(64, 64);
            bp = new Bitmap(_spotify.GetStatus().Track.GetAlbumArt(AlbumArtSize.Size160), s);
            pictureBox1.Image = bp;
            trackName = _spotify.GetStatus().Track.TrackResource.Name + spaces;
            label1.Text = trackName;
            trackNameLength = trackName.Length;
            label2.Text = _spotify.GetStatus().Track.ArtistResource.Name;
            progressBar1.Maximum = _spotify.GetStatus().Track.Length;
            Scroll();
        }
        private void TimerTick(object sender, EventArgs e) {
            Scroll();
        }
        private long time = 0;
        private int ticksBeforeStop = 0;
        private void Scroll() {
            int a = 35;
            DateTime now = DateTime.Now;
            if (trackNameLength < a || ((now.Ticks / 600) - time) <= 50000)
                return;
            scroll += 1;
            if ((scroll + a) >= trackNameLength) {
                ticksBeforeStop += 1;
                if (ticksBeforeStop >= 10) {
                    time = now.Ticks / 600;
                    scroll = 0;
                    ticksBeforeStop = 0;
                    label1.Text = trackName.Substring(0, a);
                    return;
                }
            }
            string sub = trackName.Substring(scroll);
            if (sub.Length > a) {
                label1.Text = sub.Substring(0, a);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!doSpotifyCheck())
                return;
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
        }

        private void pictureFade(bool playing) {
            if (!playing) {
                Image oldImage = bp;
                Image image = pictureBox1.Image;
                using (Graphics g = Graphics.FromImage(image))
                {
                    Pen pen = new Pen(Color.FromArgb(75, 15, 15, 15), image.Width);
                    g.DrawLine(pen, -1, -1, image.Width, image.Height);
                    g.Save();
                }
                pictureBox1.Image = image;
                bp = oldImage;
            } else {
                if (bp != null)
                    bp.Dispose();
                pictureBox1.Image = bp = new Bitmap(_spotify.GetStatus().Track.GetAlbumArt(AlbumArtSize.Size160), new Size(64, 64));
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            if (_spotify.GetStatus().Playing)
                _spotify.Pause();
            else
                _spotify.Play();
        }
    }
}
