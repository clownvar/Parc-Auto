﻿using ParcAuto.Classes_Globale;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParcAuto.Forms
{
    public partial class MajReparation : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );
        public MajReparation()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
        }
        private void RemplirComboBoxVehicules()
        {
            if (GLB.ds.Tables["Vehicules1"] != null)
                GLB.ds.Tables["Vehicules1"].Clear();
            GLB.da = new SqlDataAdapter("select * from Vehicules", GLB.Con);
            GLB.da.Fill(GLB.ds, "Vehicules1");
            foreach (DataRow item in GLB.ds.Tables["Vehicules1"].Rows)
            {
                cmbVehicule.Items.Add(item[0]);

            }
        }
        private void MajReparation_Load(object sender, EventArgs e)
        {
            RemplirComboBoxVehicules();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
