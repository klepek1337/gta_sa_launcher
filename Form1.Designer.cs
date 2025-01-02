namespace MyWinFormsApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Instalator - Pobieranie Pliku";
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            // Przycisk Pobierz
            Button btnDownload = new Button();
            btnDownload.Size = new System.Drawing.Size(200, 50);
            btnDownload.Location = new System.Drawing.Point(300, 150);
            btnDownload.Text = "Pobierz plik";
            btnDownload.Click += BtnDownload_Click;

            // Pasek postępu
            ProgressBar progressBar = new ProgressBar();
            progressBar.Location = new System.Drawing.Point(200, 250);
            progressBar.Size = new System.Drawing.Size(400, 30);
            progressBar.Name = "progressBar";

            // Etykieta statusu
            Label lblStatus = new Label();
            lblStatus.AutoSize = true;
            lblStatus.Location = new System.Drawing.Point(200, 300);
            lblStatus.Name = "lblStatus";
            lblStatus.Text = "Status: Gotowy";

            // Dodanie komponentów do formularza
            this.Controls.Add(btnDownload);
            this.Controls.Add(progressBar);
            this.Controls.Add(lblStatus);
        }
    }
}
