// Spotify-Toast - http://www.github.com/Ashleyz4/Spotify-Toast

export { }
const path = require('path'),
    glob = require('glob');

const {app, BrowserWindow} = require('electron');

const ipcMain = require('electron').ipcMain;
ipcMain.on('error', function(event, c) {
    console.log(c);
});
ipcMain.on('log', function(event, c) {
    console.log(c);
});
ipcMain.on('blank', function(event, c) {
    console.log(" ");
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
        if (process.platform === 'linux') {
            // @ts-ignore
            windowOptions.icon = path.join(__dirname, '/spotify-icon.ico');
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