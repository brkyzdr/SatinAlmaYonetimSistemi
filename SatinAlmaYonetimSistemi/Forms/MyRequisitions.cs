using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Data;

namespace SatinAlmaYonetimSistemi.Forms
{
    public partial class MyRequisitions : Form
    {
        public MyRequisitions()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            DataTable dt = CRUD.Read("SELECT * FROM Requisitions");
            dataGridView1.DataSource = dt;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.Columns["ID"].HeaderText = "ID";
            dataGridView1.Columns["UserID"].HeaderText = "Talep Eden Kişi";
            dataGridView1.Columns["Item"].HeaderText = "Ürün";
            dataGridView1.Columns["Unit"].HeaderText = "Birim";
            dataGridView1.Columns["Quantity"].HeaderText = "Miktar";
            dataGridView1.Columns["Description"].HeaderText = "Açıklama";
            dataGridView1.Columns["Status"].HeaderText = "Durum";
            dataGridView1.Columns["ApprovedAt"].HeaderText = "Satın Alma Sorumlusu";
            dataGridView1.Columns["DateTime"].HeaderText = "Talep Tarihi";

        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Close();
        }
    }
}
