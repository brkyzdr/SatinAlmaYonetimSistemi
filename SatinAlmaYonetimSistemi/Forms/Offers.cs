using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SatinAlmaYonetimSistemi.Forms
{
    public partial class Offers : Form
    {
        public Offers()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            DataTable dt = CRUD.Read("SELECT * FROM PurchaseOffers");
            dataGridView1.DataSource= dt;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.Columns["ID"].HeaderText = "ID";
            dataGridView1.Columns["SupplierID"].HeaderText = "Tedarikçi";
            dataGridView1.Columns["UserID"].HeaderText = "Kullanıcı Adı";
            dataGridView1.Columns["ItemID"].HeaderText = "Ürün";
            dataGridView1.Columns["Unit"].HeaderText = "Birim";
            dataGridView1.Columns["Quantity"].HeaderText = "Miktar";
            dataGridView1.Columns["Price"].HeaderText = "Fiyat";
            dataGridView1.Columns["Currency"].HeaderText = "Para Birimi";
            dataGridView1.Columns["Status"].HeaderText = "Durum";
            dataGridView1.Columns["CreatedAt"].HeaderText = "Oluşturan Kişi";
            dataGridView1.Columns["Date"].HeaderText = "Tarih";
            dataGridView1.Columns["ApprovedAt"].HeaderText = "Onaylayan Kişi";

        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }
    }
}
