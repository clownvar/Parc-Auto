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
            maj.ShowDialog();
            RemplirLaGrille();
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
        private void RemplirLaGrille()
        {
            dgvconducteur.Rows.Clear();
            try
            {
                GLB.Cmd.CommandText = $"select * from Conducteurs";
                GLB.Con.Open();
                GLB.dr = GLB.Cmd.ExecuteReader();
                while (GLB.dr.Read())
                {
                    dgvconducteur.Rows.Add(GLB.dr[0], GLB.dr[1], GLB.dr[2], GLB.dr[3], GLB.dr[4], GLB.dr[5], GLB.dr[6], GLB.dr[7], GLB.dr[8], GLB.dr[9]);
                }
                GLB.dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                GLB.Con.Close();
            }
        }

        private void btnQuitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Conducteurs_Load(object sender, EventArgs e)
        {
            StyleDataGridView();
            RemplirLaGrille();


        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            
            try
            {
                GLB.Matricule = (int)dgvconducteur.CurrentRow.Cells[0].Value;
                string Nom = dgvconducteur.CurrentRow.Cells[1].Value.ToString() ;
                string Prenom= dgvconducteur.CurrentRow.Cells[2].Value.ToString();
                DateTime DateNaiss =(DateTime)dgvconducteur.CurrentRow.Cells[3].Value;
                DateTime DateEmbauche = (DateTime)dgvconducteur.CurrentRow.Cells[4].Value;
                string NumPermis = dgvconducteur.CurrentRow.Cells[5].Value.ToString();
                string Adresse  = dgvconducteur.CurrentRow.Cells[6].Value.ToString();
                string Ville = dgvconducteur.CurrentRow.Cells[7].Value.ToString();
                string Tel = dgvconducteur.CurrentRow.Cells[8].Value.ToString();
                string Email = dgvconducteur.CurrentRow.Cells[9].Value.ToString();    
                MAJConducteur maj = new MAJConducteur(Nom,Prenom,DateNaiss,DateEmbauche,NumPermis,Adresse,Ville,Tel,Email);
                Commandes.Command = Choix.modifier;
                maj.ShowDialog();
                RemplirLaGrille();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Il faut selectionner sur la table pour modifier la ligne.", "Erreur",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //TODO: catch NullReferenceException (idriss)


        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            try
            {
                GLB.Matricule = (int)dgvconducteur.SelectedRows[0].Cells[0].Value;
                GLB.Cmd.CommandText = $"delete from Conducteurs where Matricule = {GLB.Matricule}";

            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Il faut selectionner sur la table pour modifier la ligne.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //TODO: catch NullReferenceException (idriss)

            DialogResult res = MessageBox.Show("Voulez Vous Vraiment Suprimmer Ce Conducteur ?","Confirmation",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button1);
            if (res == DialogResult.Yes)
            {
                GLB.Con.Open();
                GLB.Cmd.ExecuteNonQuery();
                GLB.Con.Close();
                RemplirLaGrille();
            }
            
        }
    }
}
