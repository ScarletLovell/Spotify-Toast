using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using System;
using System.Windows.Forms;
using System.Configuration;
using System.IO;

namespace ToastTest
{
    static class Program
    {
        public static bool doContinue { get; set; }
        public static string version = "1.4";

        [STAThread]
        static void Main() {
            if(!File.Exists("./songs.json")) {
                File.Create("./songs.json").Close();
            }
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form1 = new Form1();
            /*
            string webApi = "";
            Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection confFile = configManager.AppSettings.Settings;
            if(confFile["webApi"] != null)
                webApi = confFile["webApi"].Value;
            webApi = "don't do it";
            DialogResult result = MessageBox.Show("Do you want to activate Spotify Web API? This will only show up once\n" +
                "- This will open your browser each time the program is launched -", "Spotify Toast", MessageBoxButtons.YesNo);
            bool res = (result == DialogResult.Yes ? true : false);
            if(res)
                doSpotify(form1);
            Console.WriteLine("res  -  " + res);
            configManager.AppSettings.Settings.Add("webApi", (res ? "true" : "false"));
            else if(webApi.ToLower().Equals("true"))
                doSpotify(form1);
            
            configManager.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
            */
            doContinue = true;
            while(doContinue) {
                doContinue = false;
                Application.Run(form1);
            }
        }
        static async void doSpotify(Form1 form1) {
            WebAPIFactory webApiFactory = new WebAPIFactory(
                "http://localhost",
                8000,
                "46d63b1e4b774a7580c898d3909d4cd8",
                Scope.UserReadPrivate,
                TimeSpan.FromSeconds(20)
            );
            try {
                form1.spotifyWeb = await webApiFactory.GetWebApi();
            } catch(Exception e) {
                MessageBox.Show(e.Message, "Spotify Toast", MessageBoxButtons.OK);
            }

            if(form1.spotifyWeb == null)
                return;
        }
    }
}
