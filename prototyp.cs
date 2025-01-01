using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace GameInstallerGUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnInstallLauncher_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Instalowanie Launchera...";
            // Kod instalujący launcher
            InstallFile("https://example.com/launcher.exe", "launcher.exe");
        }

        private void btnInstallGame_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Instalowanie Gry...";
            InstallFile("https://download1085.mediafire.com/plik7z", "game.7z");
        }

        private void btnCalibrateGame_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Kalibracja gry i uruchamianie Steam...";
            Process.Start("steam://run/12345"); // Uruchamia grę o ID 12345 w Steam
        }

        private void InstallFile(string url, string fileName)
        {
            string downloadPath = Path.Combine(Path.GetTempPath(), fileName);
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, downloadPath);
            }

            if (fileName.EndsWith(".7z"))
            {
                Process.Start("7z.exe", $"x \"{downloadPath}\" -o\"C:\\Game\" -y");
            }
            else if (fileName.EndsWith(".exe"))
            {
                Process.Start(downloadPath);
            }

            lblStatus.Text = "Instalacja zakończona.";
        }
    }
}
