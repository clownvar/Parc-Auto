﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing;

namespace ParcAuto.Classes_Globale
{
    public class GLB
    {
        public static SQLiteConnection Con = new SQLiteConnection("Data Source=Parcautodb.sqlite3;Version=3;new=False;Compress=True;FailIfMissing=True;;datetimeformat=CurrentCulture"); 
        public static SQLiteCommand Cmd = Con.CreateCommand();
        public static SQLiteDataReader dr;
        public static DataSet ds = new DataSet();
        public static SQLiteDataAdapter da;
        public static int Matricule;
        public static string Matricule_Voiture;
        public static int id_Carburant;
        public static int id_Reparation;
        public static int id_Transport;
        public static int id_CarteFree;
        public static  int number_of_lines;
        public static Dictionary<string, string> Entites = new Dictionary<string, string> 
        { 
            { "DG", "Direction Générale" },
            { "CDG", "Cabinet/Direction Générale" },
            { "DC", "Direction de la communication" },
            { "DA", "Direction de l'Audit" },
            { "DAL", "Direction de l'approvisionnement et de la logistique" },
            { "DRH", "Direction des ressources humaines" },
            { "DFC", "Direction financière et comptable" },
            { "DAI", "Direction Afrique et International" },
            { "TP", "Trésorier Payeur" },
            { "DP", "Direction du patrimoine" },
            { "DF", "Direction de la formation" },
            { "DOSI", "Direction organisation et systèmes d'information" },
            { "DFCE", "Direction de la formation en cours d'emploi" },
            { "DRIF", "Direction de la recherche et de l'ingénierie de la formation" },
            { "DDMP", "Direction Développement et Management de Projets" },
            { "DRCS", "DR Casablanca-Settat" },
            { "DRRSK", "DR Rabat-Salé-Kénitra" },
            { "DRTTH", "DR Tanger-Tétouan-Al Hoceima" },
            { "DRPS", "DR Province de Sud" },
            { "DRFM", "DR Fes-Meknes" },
            { "DRBK", "DR Béni Mellal-Khénifra" },
            { "DRMS", "DR Marrakech-Safi" },
            { "DRO", "DR Oriental" },
            { "DRSM", "DR Souss Massa" },
            { "DRDT", "DR Draa Tafilalet" }
        };
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(115, 139, 215);
            dgv.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dgv.BackgroundColor = Color.White;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(115, 139, 215);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }
        
        /// <summary>
        ///     Draws on "print document" with a formal document layout.
        /// </summary>
        /// <param name="e">Print event</param>
        /// <param name="DGV">Datagridview to add columns and rows into document.</param>
        /// <param name="Logo">OFPPT logo to add into header with its text.</param>
        /// <param name="FontHeader">Font for the columns.</param>
        /// <param name="FontRows">Font for the rows.</param>
        /// <param name="Skipindex">Column index to skip/ not show (-1 to not skip).</param>
        /// <param name="StartingColumnPosition">The X position for where the first column should show.</param>
        /// <param name="StartingRowPosition">The Y position for where the First row should show.</param>
        static public void Drawonprintdoc(PrintPageEventArgs e,  DataGridView DGV, Image Logo, Font FontHeader, Font FontRows, int Skipindex = -1, int StartingColumnPosition = 5, int StartingRowPosition = 200, string Total = "",float bias = 0.0f) // Bias is temporary fix
        {
            float column_gap = e.PageSettings.Bounds.Width - StartingColumnPosition - 10 + bias;// - bias;
            foreach (DataGridViewColumn item in DGV.Columns)
                column_gap -= e.Graphics.MeasureString(longestcellinrow(DGV,item.Index),FontHeader).Width;
            column_gap /= DGV.Columns.Count-1;
            if (column_gap < 0) column_gap = 0;
            //Header
            e.Graphics.DrawImage(Logo, 50, 17);
            e.Graphics.DrawLine(new Pen(Color.Black, 2), 150, 40, 150, 85);            
            e.Graphics.DrawString("مكتب التكوين المهني و إنعاش الشغل", new Font("PFDinTextArabic-Light", 9, FontStyle.Bold), Brushes.Black, 158, 40);
            e.Graphics.DrawString("Office de la Formation Professionnelle\net de la Promotion du Travail", new Font("Arial",9), Brushes.Black, 158, 60);

            e.Graphics.DrawString($"Casablanca, le {DateTime.Now.ToString("dd/MM/yyyy")}", new Font("Arial", 9), Brushes.Black, e.PageSettings.Bounds.Width - 180, 105 );

            //Footer
            e.Graphics.DrawString("Intersection Route BO 50 et R.N. n°11 (Route Nouaceur) BP 40207 Sidi Maârouf Casablanca 20 270\n 20 270 و الطريق الوطنية رفم 11 (طريق النواصر) ص. ب 40207 سيدي معروف الدار البيضاء B.O 50 ملتمى طريق\nTél.: 05 22 78 72 60/61 - Fax : 05 22 32 15 09", new Font("Arial", 9), Brushes.Black, e.PageSettings.Bounds.Width/2, e.PageSettings.Bounds.Height - 35, new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });

            List<float> columns_pos = new List<float>();
            columns_pos.Add(StartingColumnPosition);
            //45 lines per page (26 per page paysage)
            if (Skipindex != -1) // When skipping
            {
                foreach (DataGridViewColumn col in DGV.Columns)
                {
                    if (col.HeaderText == DGV.Columns[Skipindex].HeaderText) continue;
                    e.Graphics.DrawString(col.HeaderText, FontHeader, Brushes.Black, columns_pos[columns_pos.Count - 1], StartingRowPosition - 17);
                    columns_pos.Add(columns_pos[columns_pos.Count - 1] + column_gap + (e.Graphics.MeasureString(longestcellinrow(DGV, col.Index),FontHeader).Width));
                }
                e.Graphics.DrawLine(new Pen(Color.Black), columns_pos[0], StartingRowPosition - 5, columns_pos[columns_pos.Count - 1] - column_gap, StartingRowPosition - 5);

                //if (!e.PageSettings.Landscape)
                //{
                    for (int item = DGV.Rows.Count - number_of_lines; item < DGV.Rows.Count - number_of_lines + (number_of_lines < (e.PageSettings.Landscape? 26:45) ? number_of_lines : (e.PageSettings.Landscape ? 26 : 45)); item++)
                    {
                        for (int i = 0; i < DGV.Rows[item].Cells.Count - 1; i++)
                        {
                            string MaxCellInRowLen;
                            if (i < Skipindex)
                            {
                                MaxCellInRowLen = longestcellinrow(DGV, i);
                                e.Graphics.DrawString(DGV.Rows[item].Cells[i].Value.ToString(), FontRows, Brushes.Black, columns_pos[i] + (float.TryParse(DGV.Rows[item].Cells[i].Value.ToString(), out _) ? ((e.Graphics.MeasureString(MaxCellInRowLen,FontRows).Width - e.Graphics.MeasureString(DGV.Rows[item].Cells[i].Value.ToString(),FontRows).Width)) : 0), StartingRowPosition);
                            } 
                            else
                            {
                                MaxCellInRowLen = longestcellinrow(DGV, i+1);
                                e.Graphics.DrawString(DGV.Rows[item].Cells[i + 1].Value.ToString(), FontRows, Brushes.Black, columns_pos[i] + (float.TryParse(DGV.Rows[item].Cells[i + 1].Value.ToString(), out _) ? (e.Graphics.MeasureString(MaxCellInRowLen,FontRows).Width - e.Graphics.MeasureString(DGV.Rows[item].Cells[i + 1].Value.ToString(),FontRows).Width) : 0), StartingRowPosition);
                            }
                        }
                        StartingRowPosition += 20;
                    }
                //}
                //else
                //{
                //    for (int item = DGV.Rows.Count - number_of_lines; item < DGV.Rows.Count - number_of_lines + (number_of_lines < 26 ? number_of_lines : 26); item++)
                //    {
                //        for (int i = 0; i < DGV.Rows[item].Cells.Count - 1; i++)
                //        {
                //            string MaxCellInRowLen;
                //            if (i < Skipindex)
                //            {
                //                MaxCellInRowLen = longestcellinrow(DGV, i);
                //                e.Graphics.DrawString(DGV.Rows[item].Cells[i].Value.ToString(), FontRows, Brushes.Black, columns_pos[i] + (float.TryParse(DGV.Rows[item].Cells[i].Value.ToString(), out _) ? ((e.Graphics.MeasureString(MaxCellInRowLen, FontRows).Width - e.Graphics.MeasureString(DGV.Rows[item].Cells[i].Value.ToString(), FontRows).Width)) : 0), StartingRowPosition);
                //            }
                //            else
                //            {
                //                MaxCellInRowLen = longestcellinrow(DGV, i + 1);
                //                e.Graphics.DrawString(DGV.Rows[item].Cells[i + 1].Value.ToString(), FontRows, Brushes.Black, columns_pos[i] + (float.TryParse(DGV.Rows[item].Cells[i + 1].Value.ToString(), out _) ? (e.Graphics.MeasureString(MaxCellInRowLen, FontRows).Width - e.Graphics.MeasureString(DGV.Rows[item].Cells[i + 1].Value.ToString(), FontRows).Width) : 0), StartingRowPosition);
                //            }
                //        }
                //        StartingRowPosition += 20;
                //    }
                //}
            }
            else  //If Nothing to skip
            {                                                                                                                                                              
                foreach (DataGridViewColumn item in DGV.Columns)
                {
                    e.Graphics.DrawString(item.HeaderText, FontHeader, Brushes.Black, columns_pos[columns_pos.Count - 1], StartingRowPosition - 17);                                            
                    columns_pos.Add(columns_pos[columns_pos.Count - 1] + column_gap + (e.Graphics.MeasureString(longestcellinrow(DGV, item.Index), FontHeader).Width));                                         
                }
                e.Graphics.DrawLine(new Pen(Color.Black), columns_pos[0], StartingRowPosition - 5, columns_pos[columns_pos.Count - 1] - column_gap, StartingRowPosition - 5);

                //if (!e.PageSettings.Landscape)
                //{
                    for (int item = DGV.Rows.Count - number_of_lines; item < DGV.Rows.Count - number_of_lines + (number_of_lines < (e.PageSettings.Landscape ? 26 : 45) ? number_of_lines : (e.PageSettings.Landscape ? 26 : 45)); item++)
                    {
                        for (int i = 0; i < DGV.Rows[item].Cells.Count; i++)
                        {
                            string MaxCellInRowLen;
                            MaxCellInRowLen = longestcellinrow(DGV, i);
                            e.Graphics.DrawString(DGV.Rows[item].Cells[i].Value.ToString(), FontRows, Brushes.Black, columns_pos[i] + (float.TryParse(DGV.Rows[item].Cells[i].Value.ToString(), out _) ? (e.Graphics.MeasureString(MaxCellInRowLen, FontRows).Width - e.Graphics.MeasureString(DGV.Rows[item].Cells[i].Value.ToString(), FontRows).Width) : 0), StartingRowPosition);
                        }
                        StartingRowPosition += 20;
                    }
                //}
                //else
                //{
                //    for (int item = DGV.Rows.Count - number_of_lines; item < DGV.Rows.Count - number_of_lines + (number_of_lines<26?number_of_lines:26); item++)
                //    {
                //        for (int i = 0; i < DGV.Rows[item].Cells.Count; i++)
                //        {
                //            string MaxCellInRowLen;
                //            MaxCellInRowLen = longestcellinrow(DGV, i);
                //            e.Graphics.DrawString(DGV.Rows[item].Cells[i].Value.ToString(), FontRows, Brushes.Black, columns_pos[i] + (float.TryParse(DGV.Rows[item].Cells[i].Value.ToString(), out _) ? (e.Graphics.MeasureString(MaxCellInRowLen, FontRows).Width - e.Graphics.MeasureString(DGV.Rows[item].Cells[i].Value.ToString(), FontRows).Width) : 0), StartingRowPosition);
                //        }
                //        StartingRowPosition += 20;
                //    }
                //}
            }


            //if (!e.PageSettings.Landscape)
            //{
                e.HasMorePages = number_of_lines > (e.PageSettings.Landscape ? 26 : 45) ? true: false;
                number_of_lines -= (e.PageSettings.Landscape ? 26 : 45);
            //}
            //else
            //{
            //    e.HasMorePages = number_of_lines > 26? true: false;
            //    number_of_lines -= 26;
            //}

            if (!e.HasMorePages)
                e.Graphics.DrawString(Total, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, e.PageSettings.Bounds.Width - (Total.Length*12)-10, StartingRowPosition + 30);
        }

        private static string longestcellinrow(DataGridView DGV, int Column_index)
        {
            string output= DGV.Columns[Column_index].HeaderText;
            foreach (DataGridViewRow item in DGV.Rows)
                if (item.Cells[Column_index].Value.ToString().Length > output.Length)
                    output = item.Cells[Column_index].Value.ToString();
                
            return output;
        }
    }
}