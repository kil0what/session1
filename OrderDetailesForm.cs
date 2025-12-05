using System;
using System.Drawing;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    public class OrderDetailsForm : Form
    {
        private TextBox txtDetails;
        private Button btnClose;
        private Button btnPrint;
        private Button btnReturn;
        private Label lblTitle;
        private PictureBox logoPictureBox;

        public OrderDetailsForm(string details, int orderId)
        {
            InitializeForm(details, orderId);
            LogoHelper.ApplyIcon(this)
            ;
        }

        private void InitializeForm(string details, int orderId)
        {
            this.Text = $"ООО Спортивные товары - Детали заказа №{orderId}";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = LogoHelper.BackgroundColor;
            this.Font = LogoHelper.MainFont;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // ЛОГОТИП (левый верхний угол)
            logoPictureBox = LogoHelper.CreateLogo();

            // Заголовок
            lblTitle = new Label
            {
                Text = $"📄 Детали заказа №{orderId}",
                Location = new Point(180, 15),
                Size = new Size(300, 40),
                Font = new Font("Comic Sans MS", 14, FontStyle.Bold),
                ForeColor = LogoHelper.AccentColor
            };

            // Текст с деталями
            txtDetails = new TextBox
            {
                Location = new Point(20, 80),
                Size = new Size(650, 380),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Comic Sans MS", 10),
                Text = details,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Кнопка печати
            btnPrint = LogoHelper.CreateStyledButton(
                "🖨️ Печать заказа",
                LogoHelper.SecondaryColor,
                Color.Black,
                150, 40);
            btnPrint.Location = new Point(20, 480);
            btnPrint.Click += (s, e) =>
            {
                MessageBox.Show("Функция печати будет реализована позже", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            // Кнопка возврата
            btnReturn = LogoHelper.CreateStyledButton(
                "📦 Оформить возврат",
                LogoHelper.AccentColor,
                Color.White,
                160, 40);
            btnReturn.Location = new Point(180, 480);
            btnReturn.Click += (s, e) =>
            {
                MessageBox.Show("Функция возврата будет реализована позже", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            // Кнопка закрытия
            btnClose = LogoHelper.CreateStyledButton(
                "✕ Закрыть",
                Color.LightGray,
                Color.Black,
                120, 40);
            btnClose.Location = new Point(550, 480);
            btnClose.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[]
            {
                logoPictureBox, lblTitle, txtDetails,
                btnPrint, btnReturn, btnClose
            });
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (logoPictureBox != null)
            {
                logoPictureBox.Location = new Point(20, 10);
            }

            if (lblTitle != null)
            {
                lblTitle.Location = new Point(180, 15);
            }

            if (txtDetails != null)
            {
                txtDetails.Width = this.ClientSize.Width - 40;
                txtDetails.Height = this.ClientSize.Height - 200;
            }

            if (btnClose != null)
            {
                btnClose.Left = this.ClientSize.Width - btnClose.Width - 20;
                btnClose.Top = this.ClientSize.Height - btnClose.Height - 20;
            }

            if (btnPrint != null)
            {
                btnPrint.Top = this.ClientSize.Height - btnPrint.Height - 20;
            }

            if (btnReturn != null)
            {
                btnReturn.Top = this.ClientSize.Height - btnReturn.Height - 20;
                btnReturn.Left = btnPrint.Right + 10;
            }
        }
    }
}