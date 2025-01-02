using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MyWinFormsApp
{
    public partial class Form1 : Form
    {
        private string downloadUrl = "https://download1085.mediafire.com/rp3zy61f9hbg18pRPwZSJHLfCcVUJBn70F3pTeaRMt1KPr7B8m9LTUlJuzpXkI_pYQP4Nm9KDIJgcbQbnp73F9dKgGskIBdLBjAgygKE9qMqvdpqFdl36iSBYLOf3f-2b6BJxx-ePd0Xg8DNxoCYGGD1DIhni0typ-L3COAZTr8SMFA/u41ujicfbrbxu8w/GTA+San+Andreas.7z";

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
            using (SaveFileDialog saveFileDialog = new())
            {
                saveFileDialog.Filter = "Archiwa 7z (*.7z)|*.7z";
                saveFileDialog.Title = "Wybierz lokalizację zapisu pliku";
                saveFileDialog.FileName = "GTA_San_Andreas.7z";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    lblStatus.Text = "Status: Pobieranie...";
                    try
                    {
                        await DownloadFileAsync(downloadUrl, saveFileDialog.FileName);
                        lblStatus.Text = "Status: Pobieranie zakończone!";
                        MessageBox.Show("Plik został pobrany pomyślnie!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "Status: Błąd pobierania!";
                        MessageBox.Show($"Wystąpił błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        progressBar.Value = 0;
                    }
                }
            }
        }

        private async Task DownloadFileAsync(string url, string destination)
        {
#pragma warning disable SYSLIB0014 // Type or member is obsolete
            using (WebClient client = new())
            {
                client.DownloadProgressChanged += (s, e) =>
                {
                    progressBar.Value = e.ProgressPercentage;
                };

                await client.DownloadFileTaskAsync(new Uri(url), destination);
            }
#pragma warning restore SYSLIB0014 // Type or member is obsolete
        }
    }
}
