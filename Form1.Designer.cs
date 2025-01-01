namespace MyWinFormsApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Clean up any resources being used.
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
            this.Text = "Form1";

            // Tworzymy przycisk
            Button button1 = new Button();
            button1.Size = new System.Drawing.Size(200, 50);
            button1.Location = new System.Drawing.Point(300, 150);
            button1.Text = "Kliknij mnie";
            button1.Click += Button1_Click;

            // Dodajemy przycisk do formularza
            this.Controls.Add(button1);
        }
    }
}
