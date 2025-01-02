using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace MyWinFormsApp
{
    public partial class Form1 : Form
    {
        private string downloadUrl = "https://download1085.mediafire.com/rp3zy61f9hbg18pRPwZSJHLfCcVUJBn70F3pTeaRMt1KPr7B8m9LTUlJuzpXkI_pYQP4Nm9KDIJgcbQbnp73F9dKgGskIBdLBjAgygKE9qMqvdpqFdl36iSBYLOf3f-2b6BJxx-ePd0Xg8DNxoCYGGD1DIhni0typ-L3COAZTr8SMFA/u41ujicfbrbxu8w/GTA+San+Andreas.7z"; // Upewnij się, że URL jest poprawny
        private string downloadFilePath;
        private string extractFolderPath;
        private string sevenZipExe;
        private string sevenZipDownloadUrl = "https://www.7-zip.org/a/7z2301-x64.exe"; // Oficjalny instalator 7-Zip

        private ProgressBar progressBar;
        private Label lblStatus;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public Form1()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            InitializeComponent();
            SetBackgroundImage();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8601 // Possible null reference assignment.
            progressBar = (ProgressBar)Controls["progressBar"];
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8601 // Possible null reference assignment.
            lblStatus = (Label)Controls["lblStatus"];
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }

        private void SetBackgroundImage()
        {
            var assembly = Assembly.GetExecutingAssembly();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            using (Stream stream = assembly.GetManifestResourceStream("MyWinFormsApp.background.jpg"))
            {
                if (stream != null)
                {
                    BackgroundImage = Image.FromStream(stream);
                    BackgroundImageLayout = ImageLayout.Stretch;
                }
                else
                {
                    MessageBox.Show("Obrazek tła nie został znaleziony.");
                }
            }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }

        private async void BtnDownload_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Status: Wybór folderu instalacji...";

            // Otwarte okno dialogowe do wyboru folderu instalacji 7-Zip
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Wybierz folder, w którym chcesz zainstalować 7-Zip";
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string installFolder = folderDialog.SelectedPath;
                    sevenZipExe = Path.Combine(installFolder, "7z.exe");

                    // Sprawdzanie, czy 7-Zip jest już zainstalowany
                    if (!File.Exists(sevenZipExe))
                    {
                        lblStatus.Text = "Status: Pobieranie i instalacja 7-Zip...";
                        await DownloadAndInstall7Zip(installFolder);
                    }
                    else
                    {
                        lblStatus.Text = "Status: 7-Zip już zainstalowany.";
                    }

                    // Wybór folderu do pobrania i rozpakowywania plików
                    folderDialog.Description = "Wybierz folder do pobrania i rozpakowania plików";
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFolder = folderDialog.SelectedPath;
                        downloadFilePath = Path.Combine(selectedFolder, "GTA_San_Andreas.7z"); // Sprawdzenie w folderze wybranym przez użytkownika
                        extractFolderPath = Path.Combine(selectedFolder, "GTA_San_Andreas");

                        lblStatus.Text = "Status: Sprawdzanie pliku do pobrania...";

                        // Sprawdzanie, czy plik już istnieje w folderze
                        if (File.Exists(downloadFilePath))
                        {
                            lblStatus.Text = "Status: Plik już istnieje. Rozpakowywanie...";
                            await ExtractFileAsync(downloadFilePath);  // Rozpakowywanie w tym samym folderze
                        }
                        else
                        {
                            lblStatus.Text = "Status: Pobieranie...";

                            try
                            {
                                // Pobieranie pliku
                                await DownloadFileAsync(downloadUrl, downloadFilePath);
                                lblStatus.Text = "Status: Pobieranie zakończone. Rozpakowywanie...";
                                await ExtractFileAsync(downloadFilePath);  // Rozpakowywanie w tym samym folderze
                                lblStatus.Text = "Status: Zakończono!";
                                MessageBox.Show("Plik został pobrany i rozpakowany pomyślnie!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                lblStatus.Text = "Status: Błąd!";
                                MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                progressBar.Value = 0;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nie wybrano folderu do pobrania pliku. Program zostanie zamknięty.");
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Nie wybrano folderu instalacji. Program zostanie zamknięty.");
                    this.Close();
                }
            }
        }

        private async Task DownloadAndInstall7Zip(string installFolder)
        {
            string installerPath = Path.Combine(Application.StartupPath, "7zip_installer.exe");
            try
            {
#pragma warning disable SYSLIB0014 // Type or member is obsolete
                using (WebClient client = new())
                {
                    client.DownloadProgressChanged += (s, e) =>
                    {
                        progressBar.Value = e.ProgressPercentage;
                    };

                    await client.DownloadFileTaskAsync(new Uri(sevenZipDownloadUrl), installerPath);
                }
#pragma warning restore SYSLIB0014 // Type or member is obsolete

                // Uruchamianie instalatora z uprawnieniami administratora
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = installerPath,
                    Arguments = $"/S /D={installFolder}",
                    UseShellExecute = true, // Wymagane dla podniesionych uprawnień
                    Verb = "runas", // Wymusza uruchomienie jako Administrator
                    CreateNoWindow = true
                };

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Process process = Process.Start(processInfo);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                process.WaitForExit();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                // Sprawdzamy, czy 7-Zip został zainstalowany
                if (!File.Exists(sevenZipExe))
                {
                    throw new Exception("Nie udało się zainstalować 7-Zip. Sprawdź uprawnienia administracyjne.");
                }

                // Dodanie 7-Zip do PATH tymczasowo
                Environment.SetEnvironmentVariable("PATH", $"{installFolder};{Environment.GetEnvironmentVariable("PATH")}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas instalacji 7-Zip: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (File.Exists(installerPath))
                    File.Delete(installerPath);
            }
        }

        private async Task DownloadFileAsync(string url, string destination)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
#pragma warning disable SYSLIB0014 // Type or member is obsolete
                        using (WebClient webClient = new())
                        {
                            webClient.DownloadProgressChanged += (s, e) =>
                            {
                                progressBar.Value = e.ProgressPercentage;
                            };

                            // Pobieranie pliku
                            await webClient.DownloadFileTaskAsync(new Uri(url), destination);
                        }
#pragma warning restore SYSLIB0014 // Type or member is obsolete
                    }
                    else
                    {
                        MessageBox.Show($"Błąd: Nie udało się uzyskać pliku z serwera. Status: {response.StatusCode}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ExtractFileAsync(string downloadFilePath)
        {
            await Task.Run(() =>
            {
                if (!Directory.Exists(extractFolderPath))
                {
                    Directory.CreateDirectory(extractFolderPath);
                }

                Process process = new Process();
                process.StartInfo.FileName = sevenZipExe;
                process.StartInfo.Arguments = $"x \"{downloadFilePath}\" -o\"{extractFolderPath}\" -y";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.OutputDataReceived += (s, e) =>
                {
                    if (e.Data != null && e.Data.Contains("%"))
                    {
                        // Wyszukiwanie postępu w danych wyjściowych
                        string progress = e.Data.Split('%')[0].Trim();
                        if (int.TryParse(progress, out int percentage))
                        {
                            this.Invoke((Action)(() =>
                            {
                                progressBar.Value = percentage; // Uaktualnianie paska postępu
                            }));
                        }
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
            });

            this.Invoke((Action)(() =>
            {
                lblStatus.Text = "Status: Rozpakowywanie zakończone.";
            }));
        }
    }
}
