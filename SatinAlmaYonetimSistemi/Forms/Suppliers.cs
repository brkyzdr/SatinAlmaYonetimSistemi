﻿using Data;
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
            ReadData();
        }
        
        private void ReadData()
        {
            DataTable dt = CRUD.Read("SELECT *,  " +
                "CASE WHEN s.IsActive = 1 THEN 'Aktif' ELSE 'Pasif' END AS Status " +
                "FROM Suppliers s");
            dataGridView1.DataSource = dt;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView1.AutoGenerateColumns = true;

            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["Name"].HeaderText = "Tedarikçi";
            dataGridView1.Columns["Phone"].HeaderText = "Telefon Numarası";
            dataGridView1.Columns["Email"].HeaderText = "E-posta";
            dataGridView1.Columns["Address"].HeaderText = "Adres";
            dataGridView1.Columns["IsActive"].Visible = false;
            dataGridView1.Columns["Status"].HeaderText = "Aktiflik";
            
        }

        private void CreateData()
        {
            if (!string.IsNullOrEmpty(textBoxSupplier.Text)
                && !string.IsNullOrEmpty(textBoxPhoneNumber.Text)
                && !string.IsNullOrEmpty(textBoxEmail.Text)
                && !string.IsNullOrEmpty(textBoxAddress.Text)
                && !string.IsNullOrEmpty(comboBoxIsActive.Text)
                )
            {
                DialogResult result = MessageBox.Show(
                    "Kaydı oluşturmak istediğinize emin misiniz?",   // Mesaj
                    "Onay",                                          // Başlık
                    MessageBoxButtons.YesNo,                         // Evet / Hayır butonları
                    MessageBoxIcon.Question                          // Soru ikonu
                );

                if (result == DialogResult.Yes)
                {
                    int isActive;
                    if (comboBoxIsActive.Text=="Aktif")
                    {
                        isActive = 1;
                    }
                    else
                    {
                        isActive=0;
                    }

                    var data = new Dictionary<string, object>
                    {
                        {"Name" ,textBoxSupplier.Text },
                        {"Phone" ,textBoxPhoneNumber.Text },
                        {"Email" ,textBoxEmail.Text },
                        {"Address" ,textBoxAddress.Text },
                        {"IsActive", isActive},
                    };
                    CRUD.Create("Suppliers", data);

                    MessageBox.Show("Kayıt başarıyla oluşturuldu.", "Bilgi");
                    ReadData();
                }
                else
                {
                    MessageBox.Show("Kayıt işlemi iptal edildi.", "Bilgi");
                }
            }
            else
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
            }
        }

        private void UpdateData()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                "Kaydı güncellemek istediğinize emin misiniz?",   // Mesaj
                "Onay",                                          // Başlık
                MessageBoxButtons.YesNo,                         // Evet / Hayır butonları
                MessageBoxIcon.Question                          // Soru ikonu
                );

                // Kullanıcı "Yes" derse kayıt işlemi yapılır
                if (result == DialogResult.Yes)
                {
                    int isActive;
                    if (comboBoxIsActive.Text == "Aktif")
                    {
                        isActive = 1;
                    }
                    else
                    {
                        isActive = 0;
                    }

                    var data = new Dictionary<string, object>
                    {
                        {"Name" ,textBoxSupplier.Text },
                        {"Phone" ,textBoxPhoneNumber.Text },
                        {"Email" ,textBoxEmail.Text },
                        {"Address" ,textBoxAddress.Text },
                        {"IsActive", isActive},
                    };
                    string condition = $"ID = '{dataGridView1.SelectedRows[0].Cells["ID"].Value}'";
                    CRUD.Update("Suppliers", data, condition);
                    MessageBox.Show("Kayıt başarıyla güncellendi.", "Bilgi");
                    ReadData();
                }
                else
                {
                    MessageBox.Show("Kayıt güncelleme işlemi iptal edildi.", "Bilgi");
                }
            }
            else
            {
                MessageBox.Show("Lütfen güncellenecek bir satır seçin.");
            }
        }

        private void DeleteData()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                "Veriyi silmek istediğinize emin misiniz?",   // Mesaj
                "Onay",                                          // Başlık
                MessageBoxButtons.YesNo,                         // Evet / Hayır butonları
                MessageBoxIcon.Question                          // Soru ikonu
                );

                if (result == DialogResult.Yes)
                {
                    string condition = $"ID = '{dataGridView1.SelectedRows[0].Cells["ID"].Value}'";
                    CRUD.Delete("Suppliers", condition);
                    MessageBox.Show("Veri başarıyla silindi.");
                    ReadData();
                }
                else
                {
                    MessageBox.Show("Veri silme işlemi iptal edildi.", "Bilgi");
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin.");
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Close();
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            CreateData();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBoxSupplier.Text = row.Cells["Name"].Value.ToString();
                textBoxPhoneNumber.Text = row.Cells["Phone"].Value.ToString();
                textBoxEmail.Text = row.Cells["Email"].Value.ToString();
                textBoxAddress.Text = row.Cells["Address"].Value.ToString();
                comboBoxIsActive.Text = row.Cells["Status"].Value.ToString();
                //if (row.Cells["IsActive"].Value.ToString() == "1")
                //    comboBoxIsActive.Text = "Aktif";
                //else
                //    comboBoxIsActive.Text = "Pasif";
            }
        }

        private void textBoxPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Sayı, kontrol (backspace vb.) veya nokta/virgül dışındaki her şeyi engelle
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Hem nokta hem virgül girişini tek bir "decimal ayırıcı" kabul et (noktayı tercih edelim)
            if (e.KeyChar == ',')
            {
                e.KeyChar = '.'; // virgül yazılırsa nokta yap
            }

            // Birden fazla nokta olmasını engelle
            if (e.KeyChar == '.' && (sender as System.Windows.Forms.TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void textBoxPhoneNumber_Validating(object sender, CancelEventArgs e)
        {
            if (!decimal.TryParse(textBoxPhoneNumber.Text, out decimal value) || value <= 0)
            {
                MessageBox.Show("Lütfen sıfırdan büyük bir sayı giriniz.", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true; // focus TextBox'ta kalır
            }
        }
    }
}
