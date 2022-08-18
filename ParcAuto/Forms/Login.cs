﻿using ParcAuto.Classes_Globale;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParcAuto;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace ParcAuto.Forms
{
    public partial class Login : Form
    {
        
        public Login()
        {
            InitializeComponent();
        }
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeft, int nTop, int nRight, int nBottom, int nWidthEllipse, int nHeightEllipse);

        private void Login_Load(object sender, EventArgs e)
        {
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));
            
        }

        private void label2_Click(object sender, EventArgs e)
        {
            txtpass.Clear();
            txtuser.Clear();
        }

        private void quitter_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Voulez vous vraiment Quitter ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtpass.UseSystemPasswordChar = true;
            }
            else
            {
                txtpass.UseSystemPasswordChar = false;
            }
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            
            GLB.Con = new SqlConnection($"Data Source=DAL1251\\SQLEXPRESS,1433;Initial Catalog=Parc_Automobile;Persist Security Info=True;User ID={txtuser.Text.Trim()};Password={txtpass.Text.Trim()}");
            GLB.Cmd = GLB.Con.CreateCommand();
            try
            {
                GLB.Con.Open();
                GLB.Con.Close();
                this.Hide();
                (new Annee()).ShowDialog();
            }
            catch (SqlException)
            {
                MessageBox.Show("Login Error");
            }
        }
    }
}
