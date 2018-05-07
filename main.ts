// Spotify-Toast - http://www.github.com/Ashleyz4/Spotify-Toast


const path = require('path'),
    glob = require('glob'),
    {app, BrowserWindow} = require('electron');

// @ts-ignore
var ipc = require('electron').ipcMain;
ipc.on('errorInWindow', (event, data) => {
    if(data && {}.toString.call(data) === '[object Object]') {
        console.log("R_E: " + JSON.stringify(data));
    } else {
        console.log("R_E: "+data);
    }
});
ipc.on('logInWindow', (event, data) => {
    if(data && {}.toString.call(data) === '[object Object]') {
        console.log("R_L: " + JSON.stringify(data));
    } else {
        console.log("R_L: "+data);
    }
});
ipc.on('blankString', (event, data) => {
    console.log("");
});

// @ts-ignore
if(process.mas)
    app.setName('Spotify-Toast');

let mainWindow = null;

function init() {
    function createWindow () {
        const windowOptions = {
            width: 292,
            height: 72,
            title: "Spotify-Toast"
        }
        mainWindow = new BrowserWindow(windowOptions);
        mainWindow.setResizable(false);
        mainWindow.setAlwaysOnTop(true);
        mainWindow.setMenu(null);
        mainWindow.loadURL(path.join('file://', __dirname, '/index.html'));
    }
    app.on('ready', () => {
        createWindow();
    });
    app.on('window-all-closed', () => {
        if (process.platform !== 'darwin') {
            app.quit();
        }
    });
    app.on('activate', () => {
        if (mainWindow === null) {
            createWindow();
        }
    })
}

init();