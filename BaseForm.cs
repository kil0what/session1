using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    public class BaseForm : Form
    {
        protected PictureBox logoPictureBox;
        protected Font mainFont = new Font("Comic Sans MS", 10);
        protected Color backgroundColor = Color.White;
        protected Color accentColor = Color.FromArgb(73, 140, 81);
        protected Color secondaryColor = Color.FromArgb(118, 227, 131);

        public BaseForm()
        {
            InitializeBaseForm();
            LogoHelper.ApplyIcon(this);
        }

        private void InitializeBaseForm()
        {
            this.BackColor = backgroundColor;
            this.Font = mainFont;
            this.MinimumSize = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            LoadLogo();
        }

        protected virtual void LoadLogo()
        {
            logoPictureBox = new PictureBox
            {
                Size = new Size(200, 60),
                SizeMode = PictureBoxSizeMode.Zoom,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            try
            {
                string[] possiblePaths =
                {
                    "Resources/logo.png",
                    "logo.png",
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/logo.png"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png")
                };

                foreach (var path in possiblePaths)
                {
                    if (File.Exists(path))
                    {
                        logoPictureBox.Image = Image.FromFile(path);
                        break;
                    }
                }
            }
            catch
            {
                // Создаем простой логотип если файл не найден
                logoPictureBox.BackColor = secondaryColor;
                using (Graphics g = Graphics.FromImage(new Bitmap(200, 60)))
                {
                    g.Clear(secondaryColor);
                    g.DrawString("ООО Спортивные товары",
                        new Font("Comic Sans MS", 10, FontStyle.Bold),
                        Brushes.Black, 10, 20);
                }
            }

            this.Controls.Add(logoPictureBox);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (logoPictureBox != null)
            {
                logoPictureBox.Location = new Point(
                    this.ClientSize.Width - logoPictureBox.Width - 20,
                    20);
            }
        }
    }
}