using System.Windows.Forms;
using System;

namespace SportsGoodsApp
{
    partial class ProductsForm
    {
        private System.ComponentModel.IContainer components = null;
        private FlowLayoutPanel flowPanel;
        private TextBox txtSearch;
        private ComboBox cmbManufacturer;
        private ComboBox cmbCategory;
        private ComboBox cmbSort;
        private Label lblResults;
        private Button btnBack;
        private Button btnAddToCart;
        private Panel topPanel;
        private Panel bottomPanel;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductsForm));

            // Основная форма
            this.Text = "Каталог товаров - ООО Спортивные товары";
            this.Size = new System.Drawing.Size(1200, 750);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.BackColor = System.Drawing.Color.White;
            this.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            // Верхняя панель (фильтры)
            this.topPanel = new System.Windows.Forms.Panel();
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Height = 140;
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.topPanel.Padding = new System.Windows.Forms.Padding(10);

            // Строка 1 фильтров
            Label lblSearch = new Label();
            lblSearch.Text = "🔍 Поиск:";
            lblSearch.Location = new System.Drawing.Point(20, 15);
            lblSearch.Size = new System.Drawing.Size(70, 25);
            lblSearch.Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Bold);
            lblSearch.ForeColor = System.Drawing.Color.Black;

            this.txtSearch = new TextBox();
            this.txtSearch.Location = new System.Drawing.Point(95, 15);
            this.txtSearch.Size = new System.Drawing.Size(250, 30);
            this.txtSearch.Text = "Название, артикул, описание...";
            this.txtSearch.ForeColor = System.Drawing.Color.Gray;
            this.txtSearch.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.txtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);
            this.txtSearch.Enter += new System.EventHandler(this.TxtSearch_Enter);
            this.txtSearch.Leave += new System.EventHandler(this.TxtSearch_Leave);

            Label lblManufacturer = new Label();
            lblManufacturer.Text = "🏭 Производитель:";
            lblManufacturer.Location = new System.Drawing.Point(370, 15);
            lblManufacturer.Size = new System.Drawing.Size(140, 25);
            lblManufacturer.Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Bold);
            lblManufacturer.ForeColor = System.Drawing.Color.Black;

            this.cmbManufacturer = new ComboBox();
            this.cmbManufacturer.Location = new System.Drawing.Point(515, 15);
            this.cmbManufacturer.Size = new System.Drawing.Size(200, 30);
            this.cmbManufacturer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbManufacturer.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.cmbManufacturer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbManufacturer.BackColor = System.Drawing.Color.White;
            this.cmbManufacturer.SelectedIndexChanged += new System.EventHandler(this.CmbManufacturer_SelectedIndexChanged);

            Label lblSort = new Label();
            lblSort.Text = "📊 Сортировка:";
            lblSort.Location = new System.Drawing.Point(740, 15);
            lblSort.Size = new System.Drawing.Size(120, 25);
            lblSort.Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Bold);
            lblSort.ForeColor = System.Drawing.Color.Black;

            this.cmbSort = new ComboBox();
            this.cmbSort.Location = new System.Drawing.Point(865, 15);
            this.cmbSort.Size = new System.Drawing.Size(150, 30);
            this.cmbSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSort.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.cmbSort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSort.BackColor = System.Drawing.Color.White;
            this.cmbSort.Items.AddRange(new object[] { "По умолчанию", "Цена ↑", "Цена ↓", "Наличие ↓" });
            this.cmbSort.SelectedIndex = 0;
            this.cmbSort.SelectedIndexChanged += new System.EventHandler(this.CmbSort_SelectedIndexChanged);

            // Строка 2 фильтров
            Label lblCategory = new Label();
            lblCategory.Text = "📁 Категория:";
            lblCategory.Location = new System.Drawing.Point(20, 55);
            lblCategory.Size = new System.Drawing.Size(120, 25);
            lblCategory.Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Bold);
            lblCategory.ForeColor = System.Drawing.Color.Black;

            this.cmbCategory = new ComboBox();
            this.cmbCategory.Location = new System.Drawing.Point(145, 55);
            this.cmbCategory.Size = new System.Drawing.Size(200, 30);
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.cmbCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCategory.BackColor = System.Drawing.Color.White;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.CmbCategory_SelectedIndexChanged);

            this.btnAddToCart = new Button();
            this.btnAddToCart.Text = "🛒 Добавить в корзину";
            this.btnAddToCart.Location = new System.Drawing.Point(370, 55);
            this.btnAddToCart.Size = new System.Drawing.Size(200, 35);
            this.btnAddToCart.BackColor = System.Drawing.Color.FromArgb(73, 140, 81); // Акцентный цвет
            this.btnAddToCart.ForeColor = System.Drawing.Color.White;
            this.btnAddToCart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddToCart.Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Bold);
            this.btnAddToCart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddToCart.Click += new System.EventHandler(this.BtnAddToCart_Click);

            this.lblResults = new Label();
            this.lblResults.Text = "Загрузка товаров...";
            this.lblResults.Location = new System.Drawing.Point(590, 60);
            this.lblResults.Size = new System.Drawing.Size(400, 25);
            this.lblResults.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            this.lblResults.ForeColor = System.Drawing.Color.FromArgb(73, 140, 81); // Акцентный цвет

            Button btnReset = new Button();
            btnReset.Text = "🔄 Сбросить фильтры";
            btnReset.Location = new System.Drawing.Point(850, 55);
            btnReset.Size = new System.Drawing.Size(165, 35);
            btnReset.BackColor = System.Drawing.Color.FromArgb(118, 227, 131); // Дополнительный цвет
            btnReset.ForeColor = System.Drawing.Color.Black;
            btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnReset.Font = new System.Drawing.Font("Comic Sans MS", 10F);
            btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            btnReset.Click += new System.EventHandler(this.BtnReset_Click);

            // Добавляем элементы на верхнюю панель
            this.topPanel.Controls.Add(lblSearch);
            this.topPanel.Controls.Add(this.txtSearch);
            this.topPanel.Controls.Add(lblManufacturer);
            this.topPanel.Controls.Add(this.cmbManufacturer);
            this.topPanel.Controls.Add(lblSort);
            this.topPanel.Controls.Add(this.cmbSort);
            this.topPanel.Controls.Add(lblCategory);
            this.topPanel.Controls.Add(this.cmbCategory);
            this.topPanel.Controls.Add(this.btnAddToCart);
            this.topPanel.Controls.Add(this.lblResults);
            this.topPanel.Controls.Add(btnReset);

            // Панель с товарами
            this.flowPanel = new FlowLayoutPanel();
            this.flowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanel.AutoScroll = true;
            this.flowPanel.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.flowPanel.BackColor = System.Drawing.Color.White; // Основной фон
            this.flowPanel.WrapContents = true;
            this.flowPanel.AutoScrollMargin = new System.Drawing.Size(0, 20);

            // Нижняя панель
            this.bottomPanel = new Panel();
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Height = 60;
            this.bottomPanel.BackColor = System.Drawing.Color.FromArgb(118, 227, 131); // Дополнительный цвет
            this.bottomPanel.Padding = new System.Windows.Forms.Padding(10);

            this.btnBack = new Button();
            this.btnBack.Text = "← Назад в главное меню";
            this.btnBack.Location = new System.Drawing.Point(20, 15);
            this.btnBack.Size = new System.Drawing.Size(200, 35);
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(73, 140, 81); // Акцентный цвет
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Comic Sans MS", 10F, System.Drawing.FontStyle.Bold);
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBack.Click += new System.EventHandler(this.BtnBack_Click);

            this.bottomPanel.Controls.Add(this.btnBack);

            // Добавляем панели на форму
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.flowPanel);
            this.Controls.Add(this.bottomPanel);
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}