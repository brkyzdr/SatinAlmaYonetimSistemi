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
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            DataTable dt = CRUD.Read("SELECT * FROM Users");
            dataGridView1.DataSource = dt;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AutoGenerateColumns = true;
            

            dataGridView1.Columns["ID"].HeaderText = "ID";
            dataGridView1.Columns["Role"].HeaderText = "Rol";
            dataGridView1.Columns["Username"].HeaderText = "Kullanıcı Adı";
            dataGridView1.Columns["Passwordhash"].HeaderText = "Şifre";
            dataGridView1.Columns["Name"].HeaderText = "İsim";
            dataGridView1.Columns["Surname"].HeaderText = "Soyisim";
            dataGridView1.Columns["Email"].HeaderText = "E-posta";
            dataGridView1.Columns["PhoneNum"].HeaderText = "Telefon Numarası";
            dataGridView1.Columns["CreatedAt"].HeaderText = "Oluşturan Kişi";
            dataGridView1.Columns["IsActive"].HeaderText = "Aktiflik";
        }


        private void button4_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Close();
        }

        private void Users_Load(object sender, EventArgs e)
        {

        }
    }
}
