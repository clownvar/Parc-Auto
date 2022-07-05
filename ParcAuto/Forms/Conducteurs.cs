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

        private void btnQuitter_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Commandes.Command.ToString());
        }

        private void Conducteurs_Load(object sender, EventArgs e)
        {

        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            MAJConducteur maj = new MAJConducteur();
            Commandes.Command = Choix.modifier;
            maj.Show();
        }
    }
}
