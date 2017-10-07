using System;
using System.Windows.Forms;
using static ToastTest.Program;

namespace ToastTest
{
    using SpotifyAPI.Local;
    using SpotifyAPI.Local.Enums;
    using SpotifyAPI.Local.Models;
    using System.Drawing;

    public partial class Form1 : Form
    {
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


        public bool doSpotifyCheck() {
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

        public bool close() {
            _spotify.Dispose();
            this.Close();
            Console.WriteLine("Exiting...");
            return false;
        }
        private void updateTrack() {
            Console.WriteLine("updateTrack");
            if(!_spotify.GetStatus().Playing) {
                
            }
            Size s = new Size(64, 64);
            Image bp = new Bitmap(_spotify.GetStatus().Track.GetAlbumArt(AlbumArtSize.Size160), s);
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
            DateTime now = DateTime.Now;
            if (trackNameLength < 30 || ((now.Ticks / 600) - time) <= 50000)
                return;
            scroll += 1;
            if ((scroll + 30) >= trackNameLength) {
                ticksBeforeStop += 1;
                if (ticksBeforeStop >= 10) {
                    time = now.Ticks / 600;
                    scroll = 0;
                    ticksBeforeStop = 0;
                    label1.Text = trackName.Substring(0, 30);
                    return;
                }
            }
            string sub = trackName.Substring(scroll);
            if (sub.Length > 30) {
                label1.Text = sub.Substring(0, 30);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            if (!doSpotifyCheck())
                return;
            _spotify.OnTrackChange += _spotify_OnTrackChange;
            _spotify.OnTrackTimeChange += _spotify_OnTrackTimeChange;
            updateTrack();
            scroll_timer = new Timer();
            scroll_timer.Tick += new EventHandler(this.TimerTick);
            scroll_timer.Interval = 250;
            scroll_timer.Start();
            label3.Text = "v"+ToastTest.Program.version;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
