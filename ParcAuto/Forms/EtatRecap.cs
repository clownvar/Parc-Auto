﻿using ParcAuto.Classes_Globale;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParcAuto.Forms
{
    public partial class EtatRecap : Form
    {
        public EtatRecap()
        {
            InitializeComponent();
        }
        private void ConsommationcarburantSNTL()
        {
            GLB.Cmd.CommandText = $"select * from EtatRecapCarburantSNTL where Annee = {GLB.SelectedDate}";
            GLB.Con.Open();
            GLB.dr = GLB.Cmd.ExecuteReader();
            if (GLB.dr.Read())
            {
                ReportCarbSNTL.Text = GLB.dr[1].ToString();
                AchatCarbSNTL.Text = GLB.dr[2].ToString();
                SumStockCarbSNTL.Text = GLB.dr[3].ToString();
                trim1CarbSNTL.Text = GLB.dr[4].ToString();
                trim2CarbSNTL.Text = GLB.dr[5].ToString();
                trim3CarbSNTL.Text = GLB.dr[6].ToString();
                trim4CarbSNTL.Text = GLB.dr[7].ToString();
                sumtrimestresCarbSNTL.Text = GLB.dr[8].ToString();
                DisponibleCarbSNTL.Text = GLB.dr[9].ToString();
            }
            GLB.dr.Close();
            GLB.Con.Close();
        }
        private void ConsommationCarteFree()
        {
            GLB.Cmd.CommandText = $"select * from EtatRecapCartefree where Annee = {GLB.SelectedDate}";
            GLB.Con.Open();
            GLB.dr = GLB.Cmd.ExecuteReader();
            if (GLB.dr.Read())
            {
                ReportCarteFree.Text = GLB.dr[1].ToString();
                AchatCarteFree.Text = GLB.dr[2].ToString();
                SumStockCarteFree.Text = GLB.dr[3].ToString();
                trim1CarteFree.Text = GLB.dr[4].ToString();
                trim2CarteFree.Text = GLB.dr[5].ToString();
                trim3CarteFree.Text = GLB.dr[6].ToString();
                trim4CarteFree.Text = GLB.dr[7].ToString();
                sumtrimestres.Text = GLB.dr[8].ToString();
                Disponible.Text = GLB.dr[9].ToString();
            }
            GLB.dr.Close();
            GLB.Con.Close();
        }
        private void ConsommationReparation()
        {
            GLB.Cmd.CommandText = $"select * from EtatRecapReparation where Annee = {GLB.SelectedDate}";
            GLB.Con.Open();
            GLB.dr = GLB.Cmd.ExecuteReader();
            if (GLB.dr.Read())
            {
                ReportReparation.Text = GLB.dr[1].ToString();
                AchatReparation.Text = GLB.dr[2].ToString();
                sumStockReparation.Text = GLB.dr[3].ToString();
                trim1Reparation.Text = GLB.dr[4].ToString();
                trim2Reparation.Text = GLB.dr[5].ToString();
                trim3Reparation.Text = GLB.dr[6].ToString();
                trim4Reparation.Text = GLB.dr[7].ToString();
                sumtrimestresReparation.Text = GLB.dr[8].ToString();
                DisponibleReparation.Text = GLB.dr[9].ToString();
            }
            GLB.dr.Close();
            GLB.Con.Close();
        }
        private void ConsommationTransport()
        {
            GLB.Cmd.CommandText = $"select * from EtatRecapTransport where Annee = {GLB.SelectedDate}";
            GLB.Con.Open();
            GLB.dr = GLB.Cmd.ExecuteReader();
            if (GLB.dr.Read())
            {
                ReportTransport.Text = GLB.dr[1].ToString();
                AchatTransport.Text = GLB.dr[2].ToString();
                SumStockTransport.Text = GLB.dr[3].ToString();
                trim1transport.Text = GLB.dr[4].ToString();
                trim2transport.Text = GLB.dr[5].ToString();
                trim3transport.Text = GLB.dr[6].ToString();
                trim4transport.Text = GLB.dr[7].ToString();
                sumTrimestresTransport.Text = GLB.dr[8].ToString();
                DisponibleTransport.Text = GLB.dr[9].ToString();
            }
            GLB.dr.Close();
            GLB.Con.Close();
        }

        private void DirectionsCentrales()
        {
            GLB.Cmd.Parameters.Clear();
            //GLB.Cmd.CommandText = "select * from Directions where Annee = @Annee and Direction not like 'DR%'";
            GLB.Cmd.CommandText = "select * from Directions where Annee = @Annee";
            GLB.Cmd.Parameters.Add("@Annee", SqlDbType.Int).Value = int.Parse(GLB.SelectedDate);

            GLB.Con.Open();
            GLB.dr = GLB.Cmd.ExecuteReader();
            while (GLB.dr.Read())
            {
                dgvDirectionsCentrales.Rows.Add(GLB.dr["Direction"], GLB.dr["DFixeCarteFree"], GLB.dr["AutreCarteFree"], GLB.dr["DFixeCarb"], GLB.dr["DMissionsCarb"], GLB.dr["DHebdoCarb"], GLB.dr["DExpCarb"], GLB.dr["Reparation"], GLB.dr["Jawaz_Train"], GLB.dr["Annee"]);
            }
            GLB.dr.Close();
            GLB.Con.Close();
        }

        private void EtatRecap_Load(object sender, EventArgs e)
        {
            ConsommationCarteFree();
            ConsommationcarburantSNTL();
            ConsommationReparation();
            ConsommationTransport();
            DirectionsCentrales();
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            Commandes.Command = Choix.modifier;
            (new MajEtatRecap()).ShowDialog();
            EtatRecap_Load(sender,e);

        }

        private void btnImprimer_Click(object sender, EventArgs e)
        {
            if (printDialog1.ShowDialog(this) == DialogResult.OK)
            {
                printPreviewDialog1.Document.PrinterSettings = printDialog1.PrinterSettings;
                printPreviewDialog1.ShowDialog();
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Impression.Print_Header(e, imageList1.Images[0]);
            Impression.Print_footer(e);

            if (e.PageSettings.Landscape)
                Print_EtatRecapTable_Paysage(e);
            else
                Print_EtatRecapTable_Portrait(e);
            
        }
        private void Print_EtatRecapTable_Portrait(PrintPageEventArgs e)
        {
            int[] Cell_surfaces = { 52, 46 };

            Print_Rectangle(e, 10, 200, 200, 45, fontStyle: FontStyle.Bold, Text: Designation.Text);

            Print_Rectangle(e, 210, 200, 156, 22, Text: $"Stock en {GLB.SelectedDate}", fontStyle: FontStyle.Bold, fontSize: 9);
            Print_Rectangle(e, 210, 222, Cell_surfaces[0], Cell_surfaces[1]/2, Text: "Report", fontStyle: FontStyle.Bold, fontSize: 7);
            Print_Rectangle(e, 262, 222, Cell_surfaces[0], Cell_surfaces[1]/2, Text: "Achat", fontStyle: FontStyle.Bold, fontSize: 7);
            Print_Rectangle(e, 314, 222, Cell_surfaces[0], Cell_surfaces[1]/2, Text: "Total", fontStyle: FontStyle.Bold, fontSize: 7);

            Print_Rectangle(e, 366, 200, Cell_surfaces[0] * 4, 45, Text: label1.Text, fontStyle: FontStyle.Bold, fontSize: 9);
            Print_Rectangle(e, 366, 222, Cell_surfaces[0], Cell_surfaces[1] / 2, Text: "1er Trim", fontStyle: FontStyle.Bold, fontSize: 6);
            Print_Rectangle(e, 418, 222, Cell_surfaces[0], Cell_surfaces[1] / 2, Text: "2eme Trim", fontStyle: FontStyle.Bold, fontSize: 6);
            Print_Rectangle(e, 470, 222, Cell_surfaces[0], Cell_surfaces[1] / 2, Text: "3eme Trim", fontStyle: FontStyle.Bold, fontSize: 6);
            Print_Rectangle(e, 522, 222, Cell_surfaces[0], Cell_surfaces[1] / 2, Text: "4eme Trim", fontStyle: FontStyle.Bold, fontSize: 6);
        }
        private void Print_EtatRecapTable_Paysage(PrintPageEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Brushes.Black), new Rectangle(10, 200, 200, 45));
        }

        private void Print_Rectangle(PrintPageEventArgs e, int x, int y, int width, int heigth, float fontSize=9, FontStyle fontStyle = FontStyle.Regular, string Text = "")
        {
            //new Font("Arial", 9, FontStyle.Bold), Brushes.Black, 110, 222, new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center }
            e.Graphics.DrawRectangle(new Pen(Brushes.Black), new Rectangle(x, y, width, heigth));
            e.Graphics.DrawString(Text, new Font("Arial", fontSize, fontStyle), Brushes.Black, x+(width/2), y+(heigth/2), new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });

        }
    }
}
