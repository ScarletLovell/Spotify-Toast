using SpotifyAPI.Local;
using SpotifyAPI.Web;
using System;
using System.Windows.Forms;

namespace ToastTest
{
    public class Spotify
    {
        public SpotifyLocalAPI _spotify = new SpotifyLocalAPI();
        public SpotifyWebAPI _spotifyWeb;
        public Form1 form1 = null;

        public bool Spotify_Load(Form1 form) {
            if(!SpotifyLocalAPI.IsSpotifyRunning() || !SpotifyLocalAPI.IsSpotifyWebHelperRunning() || !this._spotify.Connect()) {
                DialogResult result = MessageBox.Show("Unable to connect to spotify, retry?", "Spotify Toast", MessageBoxButtons.YesNo);
                if(result == DialogResult.Yes)
                    return Spotify_Load(form);
                return false;
            }
            form1 = form;
            this._spotify.ListenForEvents = true;
            this._spotify.SynchronizingObject = form1;
            _spotify.OnTrackChange += _spotify_OnTrackChange;
            _spotify.OnTrackTimeChange += _spotify_OnTrackTimeChange;
            _spotify.OnPlayStateChange += _spotify_OnPlayStateChange;
            return true;
        }

        private void _spotify_OnPlayStateChange(object sender, PlayStateEventArgs e) {
            if(_spotify == null || form1 != null) {
                if(form1.toast_isEnabled)
                    form1.toast_timer.Start();
                form1.Fade(e.Playing);
            }
        }
        /// <summary>Spotify event</summary>
        private void _spotify_OnTrackChange(object sender, TrackChangeEventArgs e) {
            if(_spotify == null || form1 != null)
                form1.UpdateTrack();
        }
        /// <summary>Spotify event</summary>
        private void _spotify_OnTrackTimeChange(object sender, TrackTimeChangeEventArgs e) {
            if(_spotify == null || !_spotify.GetStatus().Playing || form1 == null)
                return;
            form1.progressBar1.Value = (int)e.TrackTime;
            //_spotify.GetStatus().PlayingPosition
        }
    }
}
