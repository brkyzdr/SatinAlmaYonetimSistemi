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
    public partial class Stocks : Form
    {
        public Stocks()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            DataTable dt = CRUD.Read("SELECT * FROM Items");
            dataGridView1.DataSource = dt;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.Columns["ID"].HeaderText = "ID";
            dataGridView1.Columns["Name"].HeaderText = "Ürün";
            dataGridView1.Columns["Unit"].HeaderText = "Birim";
            dataGridView1.Columns["Quantity"].HeaderText = "Miktar";
            dataGridView1.Columns["IsActive"].HeaderText = "Aktiflik";
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Close();
        }
    }
}
