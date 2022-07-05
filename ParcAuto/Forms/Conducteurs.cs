﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParcAuto.Classes_Globale;

namespace ParcAuto.Forms
{
    public partial class Conducteurs : Form
    {
        public Conducteurs()
        {
            InitializeComponent();
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            MAJConducteur maj = new MAJConducteur();
            Commandes.Command = Choix.ajouter;
            maj.Show();
        }
        private void StyleDataGridView()
        {
            dgvconducteur.BorderStyle = BorderStyle.None;
            dgvconducteur.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dgvconducteur.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvconducteur.DefaultCellStyle.SelectionBackColor = Color.FromArgb(115, 139, 215);
            dgvconducteur.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dgvconducteur.BackgroundColor = Color.White;
            dgvconducteur.EnableHeadersVisualStyles = false;
            dgvconducteur.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvconducteur.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(115, 139, 215);
            dgvconducteur.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }

        private void btnQuitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Conducteurs_Load(object sender, EventArgs e)
        {
            StyleDataGridView();
            //Jeux d'essaie 
            //TODO : Remplir la Grille
            dgvconducteur.Rows.Add(null, null, null, null, null, null, null, null, null, null);
            dgvconducteur.Rows.Add(null, null, null, null, null, null, null, null, null, null);
            dgvconducteur.Rows.Add(null, null, null, null, null, null, null, null, null, null);
            dgvconducteur.Rows.Add(null, null, null, null, null, null, null, null, null, null);


        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            MAJConducteur maj = new MAJConducteur();
            Commandes.Command = Choix.modifier;
            try
            {
                GLB.Matricule = (int)dgvconducteur.SelectedRows[0].Cells[0].Value;
                maj.Show();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Il faut selectionner sur la table pour modifier la ligne.", "Erreur",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //TODO: catch NullReferenceException 


        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                GLB.Matricule = (int)dgvconducteur.SelectedRows[0].Cells[0].Value;
                GLB.Cmd.CommandText = $"delete from conducteur where matricule={GLB.Matricule}";
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Il faut selectionner sur la table pour modifier la ligne.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //TODO: catch NullReferenceException 

            GLB.Con.Open();
            GLB.Cmd.ExecuteNonQuery();
            GLB.Con.Close();
        }
    }
}
