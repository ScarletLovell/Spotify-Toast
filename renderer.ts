// Some code taken from the Spoticord app https://github.com/nations/spoticord/
 // Code in spoticord also was taken from somewhere else
// Stuff was changed to make due for getting this program to act correctly
// Without this way, I would have to create a whole new way and possibly connect it to a oauth token
 // noone wants to do that. Spotify is very weird when it comes to getting song info
  // this is another reason why the main app for windows uses another dll
const version = "v0.1.5";

const $ = require('jquery');
const spotifyReq = require('./spotify'),
    events = require('events'),
    fs = require('fs');

var ipc = require('electron').ipcRenderer;
window.onerror = function(error, url, line) {
    ipc.send('errorInWindow', "ERR at line " + line + " / " + url + "\n\t"+error);
    window.stop();
};
window.onunload = function() {
    ipc.send('logInWindow', "Unloading...");
};

// @ts-ignore
var console = {
    log(string) {
        ipc.send('logInWindow', string);
    },
    error(string) {
        ipc.send('errorInWindow', string);
    },
    blank() {
        ipc.send('blankString');
    }
};

var spotify = new spotifyReq.SpotifyWebHelper();
var songEmitter = new events.EventEmitter(),
    currentSong = null;

var check = null;

async function spotifyReconnect() {
    spotify.getStatus(function(err, res) {
        if (!err) {
            clearInterval(check);
            intloop = setInterval(checkSpotify, 500);
            console.log("Successfully reconnected to Spotify!");
        } else {
            console.log("Can't connect to spotify... Retrying.");
        }
    });
}

var debug = true;
async function checkSpotify() {
    if(!spotify.isInitialized) {
        clearInterval(intloop);
        check = setInterval(spotifyReconnect, 5000);
        return;
    }
    spotify.getStatus(function (err, res) {
        if (err) {
            if (err.code === "ECONNREFUSED") {
                if (err.address === "127.0.0.1" && err.port === 4381) {
                    /**
                     * Temporary workaround - to truly fix this, we need to change spotify.js to check for ports above 4381 to the maximum range.
                     * This is usually caused by closing Spotify and reopening before the port stops listening. Waiting about 10 seconds should be
                     * sufficient time to reopen the application.
                     **/
                    console.error("Spotify is no longer reachable!! Retrying instead...");
                    clearInterval(intloop);
                    check = setInterval(spotifyReconnect, 5000);
                }
            } else {
                console.error("Failed to fetch Spotify data:", err);
            }
            return;
        }

        if (!res || !res.track || !res.track.track_resource || !res.track.artist_resource) {
            if(debug)
                console.error("Couldn't see the track from spotify. Does it exist?");
            return;
        }
        //console.log(res.track.track_resource.uri.replace("spotify:track:", ""));
        if(currentSong !== null) {
            if (res.track.track_resource.name === currentSong.name && res.track.track_resource.uri === currentSong.uri) {
                currentSong.playing = res.playing;
                currentSong.position = res.playing_position;
                songEmitter.emit('songUpdate', currentSong);
                return;
            }
        }

        let start = parseInt(new Date().getTime().toString().substr(0, 10)),
            end = start + (res.track.length - res.playing_position);

        var song = {
            uri: (res.track.track_resource.uri ? res.track.track_resource.uri : ""),
            name: res.track.track_resource.name,
            album: (res.track.album_resource ? res.track.album_resource.name : ""),
            artist: (res.track.artist_resource ? res.track.artist_resource.name : ""),
            playing: res.playing,
            position: res.playing_position,
            length: res.track.length,
            start,
            end
        };

        currentSong = song;
    
        songEmitter.emit('newSong', res, song);
    });
}
var intloop = setInterval(checkSpotify, 500);

function getElement(name) {
    var el = document.getElementById(name);
    if(el)
        return el;
    return null;
}
getElement("version").textContent = version;

songEmitter.on('newSong', (res, song) => {
    console.blank();
    console.log("New Song: " + song.name + " by " + res.track.artist_resource.name);
    var song = song.name;
    if(song.length > 15) {
        song = song.substr(0, 15) + "...";
    }
    getElement("song_name").textContent = song;
    var artist = res.track.artist_resource.name;
    if(!artist) {
        getElement("song_artist").textContent = "?";
        console.error("Unable to get song artist for some reason?");
    } else {
        if(artist.length > 25) {
            artist = artist.substr(0, 25) + "...";
        }
        getElement("song_artist").textContent = artist;
    }
    spotify.getAlbumArt(res.track.album_resource.uri, function(err, thing) {
        if(err) {
            console.error(err);
            return;
        } else {
            var albumart = (document.getElementById("song_albumart"));
            albumart.src = thing.thumbnail_url;
        }
    });
    console.log("Successfully updated the song. Yay!");
});

var fullRes = null;
songEmitter.on('songUpdate', song => {
    fullRes = song;
    var progress = (document.getElementById("progress"));
    progress.value = song.position;
    progress.max = song.length;
});

$("#progress").click(function() {
    if(!fullRes) {
        return;
    }
    if(fullRes.playing) {
        spotify.pause(function(err, res) {

        });
    } else {
        spotify.unpause(function(err, res) {

        });
    }
});