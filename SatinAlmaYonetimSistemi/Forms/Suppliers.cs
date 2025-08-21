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
    public partial class Suppliers : Form
    {
        public Suppliers()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            DataTable dt = CRUD.Read("SELECT * FROM Suppliers");
            dataGridView1.DataSource = dt;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.Columns["ID"].HeaderText = "ID";
            dataGridView1.Columns["Name"].HeaderText = "Tedarikçi";
            dataGridView1.Columns["Phone"].HeaderText = "Telefon Numarası";
            dataGridView1.Columns["Email"].HeaderText = "E-posta";
            dataGridView1.Columns["Address"].HeaderText = "Adres";
            dataGridView1.Columns["IsActive"].HeaderText = "Aktiflik";
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }

        private void Suppliers_Load(object sender, EventArgs e)
        {

        }
    }
}
