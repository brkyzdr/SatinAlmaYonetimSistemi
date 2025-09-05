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

namespace SatinAlmaYonetimSistemi.Forms
{
    public partial class RequisitionsAnswer : Form
    {
        public RequisitionsAnswer()
        {
            InitializeComponent();
            ReadData();
        }

        private void ReadData()
        {
            DataTable dt = CRUD.Read(
                "SELECT " +
                "r.ID, r.Item, r.Quantity, r.Unit, r.Description, r.Date, r.Status," +
                "CASE WHEN r.ApprovedByID = 0 THEN 'Beklemede' END AS ApprovedBy " +
                "FROM Requisitions r " +
                "WHERE r.Status ='Onay Bekliyor'");

            dataGridView.DataSource = dt;
            dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView.AutoGenerateColumns = true;

            dataGridView.Columns["ID"].Visible = false;
            dataGridView.Columns["Item"].HeaderText = "Ürün";
            dataGridView.Columns["Unit"].HeaderText = "Birim";
            dataGridView.Columns["Quantity"].HeaderText = "Miktar";
            dataGridView.Columns["Description"].HeaderText = "Açıklama";
            dataGridView.Columns["Status"].HeaderText = "Durum";
            dataGridView.Columns["ApprovedBy"].Visible = false;
            dataGridView.Columns["Date"].HeaderText = "Talep Tarihi";
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
                        {"Status" ,comboBoxStatus.Text },
                    };
                    string condition = $"ID = '{dataGridView.SelectedRows[0].Cells["ID"].Value}'";
                    CRUD.Update("Requisitions", data, condition);
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

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            comboBoxStatus.Enabled = true;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView.Rows[e.RowIndex];
                comboBoxStatus.Text = row.Cells["Status"].Value.ToString();
            }
        }
    }
}
