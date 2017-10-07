using System;
using System.Windows.Forms;

namespace ToastTest
{
    static class Program
    {
        public static float version = 1.1f;

        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form1 = new Form1();
            //spotifyCode spotify = new spotifyCode();
            //spotify.form1 = form1;
            //form1.spotifyCode = spotify;
            Application.Run(form1);
        }
    }
}
