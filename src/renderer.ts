// Some code taken from the Spoticord app https://github.com/nations/spoticord/
 // Code in spoticord also was taken from somewhere else
// Stuff was changed to make due for getting this program to act correctly
// Without this way, I would have to create a whole new way and possibly connect it to a oauth token
 // noone wants to do that. Spotify is very weird when it comes to getting song info
  // this is another reason why the main app for windows uses another dll

export { };

const version = "v0.1.5";

const $ = require('jquery');
const spotifyReq = require('./spotify');
import fs = require('fs');
import events = require('events');
import Vibrant = require('node-vibrant');

const remote = require('electron').remote,
    ipc = require('electron').ipcRenderer;
window.onerror = (error, url, line) => {
    ipc.send('error', "ERR at line " + line + " / " + url + "\n\t"+error);
    //window.stop();
};
window.onunload = () => {
    ipc.send('log', "Unloading...");
};

const console = {
    log(string) {
        ipc.send('log', string);
    },
    error(string) {
        ipc.send('error', string);
    },
    blank() {
        ipc.send('blank');
    }
};

var spotify = new spotifyReq.SpotifyWebHelper();
var songEmitter = new events.EventEmitter(),
    currentSong = null;

var check = null;

function spotifyReconnect() {
    spotify.getStatus(function(err, res) {
        if (!err) {
            clearInterval(check);
            intloop = setInterval(checkSpotify, 1200);
            console.log("Successfully reconnected to Spotify!");
        } else {
            console.log("Can't connect to spotify... Retrying.");
        }
    });
}

var debug = true;
function checkSpotify() {
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
                console.error("Failed to fetch Spotify data: " +  err);
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

var fullRes = null;
songEmitter.on('newSong', (res, song) => {
    fullRes = res;
    console.blank();
    console.log("New Song: " + song.name + " by " + res.track.artist_resource.name);
    var song = song.name;
    if(song.length > 15) {
        song = song.substr(0, 15) + "...";
    }
    (document.getElementById("song_name")  as HTMLParagraphElement).textContent = song;
    var artist = res.track.artist_resource.name;
    if(!artist) {
        (document.getElementById("song_artist") as HTMLParagraphElement).textContent = "?";
        console.error("Unable to get song artist for some reason?");
    } else {
        if(artist.length > 25) {
            artist = artist.substr(0, 25) + "...";
        }
        (document.getElementById("song_artist") as HTMLParagraphElement).textContent = artist;
    }
    spotify.getAlbumArt(res.track.album_resource.uri, function(err, thing) {
        if(err) {
            console.error(err);
            return;
        } else {
            // @ts-ignore
            var albumart = (document.getElementById("song_albumart") as HTMLImageElement);
            albumart.src = thing.thumbnail_url;
            Vibrant.from(thing.thumbnail_url).build().getPalette().then((palette) => {
                console.log("no?");
                var r, g, b, full;
                if(palette.Vibrant) { 
                // I would rather have just vibrant but i guess not
                    r = palette.Vibrant.r;
                    g = palette.Vibrant.g;
                    b = palette.Vibrant.b;
                    full = palette.Vibrant;
                } else if(palette.LightVibrant) {
                    r = palette.LightVibrant.r;
                    g = palette.LightVibrant.g;
                    b = palette.LightVibrant.b;
                    full = palette.LightVibrant;
                } else if(palette.DarkVibrant) {
                    r = palette.DarkVibrant.r;
                    g = palette.DarkVibrant.g;
                    b = palette.DarkVibrant.b;
                    full = palette.DarkVibrant;
                } else if(palette.LightMuted) {
                    r = palette.LightMuted.r;
                    g = palette.LightMuted.g;
                    b = palette.LightMuted.b;
                    full = palette.LightMuted;
                } else if(palette.DarkMuted) {
                    r = palette.DarkMuted.r;
                    g = palette.DarkMuted.g;
                    b = palette.DarkMuted.b;
                    full = palette.DarkMuted;
                }
                var rV = 255 - r,
                    gV = 255 - g,
                    bV = 255 - b;
                var rV_comp = rV - r,
                    gV_comp = gV - g,
                    bV_comp = bV - b;
                if(rV_comp <= 50 && rV_comp >= -50)
                    rV += (rV <= 55 ? 200 : (rV > 200 ? -150 : (rV > 100 ? -100 : -50)));
                if(gV_comp <= 50 && gV_comp >= -50)
                    gV += (gV <= 55 ? 200 : (gV > 200 ? -150 : (gV > 100 ? -100 : -50)));
                if(bV_comp <= 50 && bV_comp >= -50)
                    bV += (bV <= 55 ? 200 : (bV > 200 ? -150 : (bV > 100 ? -100 : -50)));
                console.log("no?");
                if(rV < 0)
                    rV = 0;
                else if(rV > 255)
                    rV = 255;
                if(gV < 0)
                    gV = 0;
                else if(gV > 255)
                    gV = 255;
                if(bV < 0)
                    bV = 0;
                else if(bV > 255)
                    bV = 255;
                
                var vec1 = full.getHex(),
                    vec2 = Vibrant.Util.rgbToHex(rV, gV, bV);
                var diff = Vibrant.Util.hexDiff(vec1, vec2);
                console.log(diff);
                if(diff < 20) {
                    if(rV < 235)
                        rV += 20;
                    else
                        rV -= 20;
                    if(gV < 235)
                        gV += 20;
                    else
                        gV -= 20;
                    if(bV < 235)
                        bV += 20;
                    else
                        bV -= 20;
                }
                
                var invert = Vibrant.Util.rgbToHex(rV, gV, bV);
                $("body").css('background-color', full.getHex());
                $("body").css('color', invert);
            });
        }
    });
    console.log("Successfully updated the song. Yay!");
});
songEmitter.on('songUpdate', song => {
    var progress = (document.getElementById("progress") as HTMLProgressElement);
    var image = (document.getElementById("song_albumart") as HTMLImageElement);
    if(!song.playing) {
        progress.style.opacity = "0.3";
        image.style.opacity = "0.3";
    } else {
        progress.style.opacity = "1";
        image.style.opacity = "1";
    }
    progress.value = song.position;
    progress.max = song.length;
});
var intloop = setInterval(checkSpotify, 1200);
(function() {
    $("#progress").click(function() {
        console.log("click 1");
        if(!fullRes) {
            return;
        }
        console.log("click 2");
        if(fullRes.playing) {
            spotify.pause(function(err, res) {
                if(err)
                    console.error(err);
                if(res)
                    console.log(res);
            });
        } else {
            spotify.unpause(function(err, res) {
                if(err)
                    console.error(err);
                if(res)
                    console.log(res);
            });
        }
    });
    document.getElementById("version").textContent = version;
    console.log("Window ready");
})();