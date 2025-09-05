using Data;
using Data.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SatinAlmaYonetimSistemi.Forms
{
    public partial class OffersAnswer : Form
    {
        public OffersAnswer()
        {
            InitializeComponent();
            ReadData();
        }

        private void ReadData()
        {
            DataTable dt = CRUD.Read(
                "SELECT " +
                "o.ID, s.Name AS SupplierName, o.Item, o.Quantity, o.Unit, o.Price, o.Currency, o.Date," +
                "COALESCE(u.Name, '') + ' ' + COALESCE(u.Surname, '') + '(#' + CAST(u.ID AS varchar(20)) + ')' AS PurchaseName " +
                "FROM Offers o " +
                "INNER JOIN Suppliers s ON o.SupplierID = s.ID " +
                "INNER JOIN Users u ON o.UserID = u.ID " +
                "WHERE o.Status='Onay Bekliyor' ");

            dataGridView.DataSource = dt;
            dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView.AutoGenerateColumns = true;

            dataGridView.Columns["ID"].Visible = false;
            dataGridView.Columns["SupplierName"].HeaderText = "Ürün";
            dataGridView.Columns["Item"].HeaderText = "Ürün";
            dataGridView.Columns["Unit"].HeaderText = "Birim";
            dataGridView.Columns["Quantity"].HeaderText = "Miktar";
            dataGridView.Columns["Price"].HeaderText = "Fiyat";
            dataGridView.Columns["Currency"].HeaderText = "Para Birimi";
            dataGridView.Columns["PurchaseName"].HeaderText = "Satınalma Sorumlusu";
            dataGridView.Columns["Date"].HeaderText = "Teklif Tarihi";
        }

        private void SaveData()
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                "Kayıt etmek istediğinize emin misiniz?",   // Mesaj
                "Onay",                                          // Başlık
                MessageBoxButtons.YesNo,                         // Evet / Hayır butonları
                MessageBoxIcon.Question                          // Soru ikonu
                );


                if (result == DialogResult.Yes)
                {
                    var data = new Dictionary<string, object>
                    {
                        {"ApprovedByID",Session.UserID },
                        {"Status" , "Onaylandı"},
                    };
                    string condition = $"ID = '{dataGridView.SelectedRows[0].Cells["ID"].Value}'";
                    CRUD.Update("Offers", data, condition);
                    MessageBox.Show("Kayıt başarıyla yapıldı.", "Bilgi");
                    ReadData();
                }
                else
                {
                    MessageBox.Show("İşlem iptal edildi.", "Bilgi");
                }
            }
            else
            {
                MessageBox.Show("Lütfen satır seçin.");
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            buttonSave.Enabled = true;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];
                textBoxSupplier.Text = row.Cells["SupplierName"].Value.ToString();
                textBoxItem.Text = row.Cells["Item"].Value.ToString();
                textBoxQuantity.Text = row.Cells["Quantity"].Value.ToString();
                textBoxUnit.Text = row.Cells["Unit"].Value.ToString();
                textBoxPrice.Text = row.Cells["Price"].Value.ToString();
                textBoxCurrency.Text = row.Cells["Currency"].Value.ToString();
            }

            textBoxSupplier.Enabled = true;
            textBoxCurrency.Enabled = true;
            textBoxItem.Enabled = true;
            textBoxPrice.Enabled = true;
            textBoxQuantity.Enabled = true;
            textBoxUnit.Enabled = true;
            label1.Enabled = true;

            textBoxSupplier.Visible = true;
            textBoxCurrency.Visible = true;
            textBoxItem.Visible = true;
            textBoxPrice.Visible = true;
            textBoxQuantity.Visible = true;
            textBoxUnit.Visible = true;
            label1.Visible = true;
        }
    }
}
