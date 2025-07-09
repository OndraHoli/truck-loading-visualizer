using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



using PdfSharp.Pdf;
using PdfSharp.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Reflection;



namespace nakladka_vizual
{
    public partial class Form1 : Form
    {
        private ContextMenuStrip palletContextMenu;
        private bool isDragging = false;
        private Point dragOffset;
        private Panel selectedPallet = null;

        int bedX;
        int bedY;
        String bedSize = "200x50";


        bool hasSnappedOut = false;
        bool hasSnappedIn = true;

        private bool _preventOverlap = false;

        public Form1()
        {
            InitializeComponent();
            typeof(Panel)
                .GetProperty("DoubleBuffered",
                 BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(panel_truckBed, true, null);
            palletContextMenu = new ContextMenuStrip();
            palletContextMenu.Items.Add("Rotate", null, RotatePallet);
            palletContextMenu.Items.Add("Delete", null, DeletePallet);
            palletContextMenu.Items.Add("Rename", null, RenamePallet);
            panel_truckBed.BackColor = ColorTranslator.FromHtml("#F0F0F0");
            panel_truckBed.BorderStyle = BorderStyle.FixedSingle;

            var bedSizeLabel = new Label
            {
                Text = $"{bedX}×{bedY}",        // or use bedSize if you prefer the raw string
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#2C3E50"),
                BackColor = Color.Transparent,
                Location = new Point(8, 8)          // small inset from top-left
            };

            // 2) Add it into the truck-bed panel
            panel_truckBed.Controls.Add(bedSizeLabel);
            bedSizeLabel.BringToFront();

            // 
            comboBox_Bedsize.SelectedIndexChanged += (s, e) =>
            {
                bedSize = comboBox_Bedsize.Text;
                var parts = bedSize.Split('x');
                bedX = int.Parse(parts[0]);
                bedY = int.Parse(parts[1]);
                bedSizeLabel.Text = $"{bedX}×{bedY}";
                panel_truckBed.Invalidate();
            };

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //extra
            //panel_truckBed.Paint += panel_truckBed_Paint;
        }

        //extra
        /*
        private void panel_truckBed_Paint(object sender, PaintEventArgs e)
        {
            string text = "MS OBALY, A.S.";
            using (Font font = new Font("Arial", 30, FontStyle.Bold))
            using (Brush brush = new SolidBrush(Color.DarkBlue)) // or any color
            {
                // Optional: center it
                SizeF textSize = e.Graphics.MeasureString(text, font);
                float x = (panel_truckBed.Width - textSize.Width) / 2;
                float y = 10; // you can move it down as needed

                e.Graphics.DrawString(text, font, brush, x, y);
            }
        }
        */
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            string[] parts = bedSize.Split('x');
            bedX = Convert.ToInt16(parts[0]);
            bedY = Convert.ToInt16(parts[1]);

            using (Pen pen = new Pen(ColorTranslator.FromHtml("#2C3E50"), 5))
            {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;

                int frameX = (panel_truckBed.Width - bedX) / 2;
                int frameY = (panel_truckBed.Height - bedY) / 2;

                //offsety??? -5 nebo -3? co?
                e.Graphics.DrawRectangle(pen, frameX - 5 -1,frameY - 5 -1, bedX + 5*2 + 2,bedY+ 5*2 + 2);
            }

        }

        
        private void button_addPallet_Click(object sender, EventArgs e)
        {
            int widthPallet;
            int heigthPallet;

            if (checkBox_CustomPallet.Checked == true)
            {
                widthPallet = (int)numericUpDown_PalletX.Value;
                heigthPallet = (int)numericUpDown_PalletY.Value;
            }
            else
            {
                String palletSize = comboBox_Pallet.Text;
                string[] parts = palletSize.Split('x');
                widthPallet = Convert.ToInt16(parts[0]);
                heigthPallet = Convert.ToInt16(parts[1]);
            }

            Panel Pallet = new Panel();
            Pallet.BackColor = ColorTranslator.FromHtml("#16A085");
            Pallet.Padding = new Padding(10, 10, 10, 0);


            Pallet.ContextMenuStrip = palletContextMenu;

            typeof(Panel)
                .GetProperty("DoubleBuffered",
                 BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(Pallet, true, null);

            //back image
            //Pallet.BackgroundImage = Image.FromFile("C:\\Users\\Ondra\\source\\repos\\nakladka_vizual\\paleta.png");
            //Pallet.BackgroundImageLayout = ImageLayout.Stretch;

            Pallet.Paint += drawCross;

            Pallet.Size = new Size(widthPallet, heigthPallet);
            Pallet.Location = new Point(100, 100);
            Pallet.MouseDown += Pallet_MouseDown;
            Pallet.MouseMove += Pallet_MouseMove;
            Pallet.MouseUp += Pallet_MouseUp;

            // only a header strip at the top
            var lbl = new Label
            {
                Text = $"{widthPallet}×{heigthPallet}",
                Dock = DockStyle.Top,            // dock to top instead of filling
                Height = 20,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Font = new Font("Arial", 9f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(102, 99, 99) // match panel so text pops
            };
            
            lbl.MouseDown += Pallet_MouseDown;
            Pallet.Controls.Add(lbl);
            lbl.BringToFront();


            panel_truckBed.Controls.Add(Pallet);
        }

        private void drawCross(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null) return;

            using (Pen pen = new Pen(ColorTranslator.FromHtml("#ECF0F1"), 5))
            {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                e.Graphics.DrawLine(pen, 0, 0, panel.Width, panel.Height);          
                e.Graphics.DrawLine(pen, 0, panel.Height, panel.Width, 0);
                e.Graphics.DrawRectangle(pen,0,0,panel.Width, panel.Height);
                
            }
            using (Pen pen = new Pen(Color.Black, 2))
            {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                e.Graphics.DrawRectangle(pen, 0, 0, panel.Width, panel.Height);
            }
        }

        private void Pallet_MouseDown(object sender, MouseEventArgs e)
        {
            // Figure out which Panel was clicked (panel or its label)
            Panel panel = null;
            if (sender is Panel p)
                panel = p;
            else if (sender is Label lbl)
                panel = lbl.Parent as Panel;

            if (panel == null)
                return;

            // Right-click: just select for context menu
            if (e.Button == MouseButtons.Right)
            {
                selectedPallet = panel;
                return;
            }

            // Left-click: start dragging
            if (e.Button == MouseButtons.Left)
            {
                selectedPallet = panel;
                isDragging = true;

                // Compute dragOffset in panel-coordinates
                // Convert the mouse point (which is relative to the sender)
                // into screen coords, then back into the panel’s client coords:
                var screenPt = ((Control)sender).PointToScreen(e.Location);
                dragOffset = panel.PointToClient(screenPt);

                selectedPallet.BackColor = Color.LightGray;
                selectedPallet.BringToFront();

                // Hook the bed’s move/up to continue drag
                panel_truckBed.Capture = true;
                panel_truckBed.MouseMove += Panel_truckBed_MouseMove;
                panel_truckBed.MouseUp += Panel_truckBed_MouseUp;
            }
        }
        private void Panel_truckBed_MouseUp(object sender, MouseEventArgs e)
        {
            
            isDragging = false;

            if (selectedPallet != null)
            {
                selectedPallet.BackColor = ColorTranslator.FromHtml("#16A085");
                selectedPallet = null;
            }

            // Release capture and unhook both Move and Up
            panel_truckBed.Capture = false;
            panel_truckBed.MouseMove -= Panel_truckBed_MouseMove;
            panel_truckBed.MouseUp -= Panel_truckBed_MouseUp;
        }
        private void Panel_truckBed_MouseMove(object sender, MouseEventArgs e)
        {
            panel_truckBed.Refresh();


            if (!isDragging || selectedPallet == null) return;

            // 1) raw delta
            Point palletLocal = selectedPallet.PointToClient(
                panel_truckBed.PointToScreen(e.Location));
            int deltaX = palletLocal.X - dragOffset.X;
            int deltaY = palletLocal.Y - dragOffset.Y;

            // 2) bed rect + buffer
            int buffer = 75;
            int bedLeft = (panel_truckBed.Width - bedX) / 2;
            int bedTop = (panel_truckBed.Height - bedY) / 2;
            int bedRight = bedLeft + bedX;
            int bedBottom = bedTop + bedY;

            // 3) old location
            Point oldLoc = selectedPallet.Location;

            // 4) compute rawLoc and the “extended” zone (bed ± buffer, size-adjusted)
            Point rawLoc = new Point(oldLoc.X + deltaX,
            oldLoc.Y + deltaY);
            var extended = new Rectangle(
            bedLeft - buffer,
            bedTop - buffer,
            bedX + buffer * 2 - selectedPallet.Width,
            bedY + buffer * 2 - selectedPallet.Height);
            bool outside = !extended.Contains(rawLoc);

            if (outside)
            {
                // -- snapped out --
                if (!hasSnappedOut)
                {
                    hasSnappedOut = true;
                    hasSnappedIn = false;
                }

                // free move: just use rawLoc (no clamp, no overlap check)
                selectedPallet.Location = rawLoc;
            }
            else
            {
                // -- snapped in --
                if (!hasSnappedIn)
                {
                    hasSnappedIn = true;
                    hasSnappedOut = false;
                }

                // clamp rawLoc to the true bed edges
                int clampedX = Math.Max(bedLeft,
                Math.Min(bedRight - selectedPallet.Width, rawLoc.X));
                int clampedY = Math.Max(bedTop,
                Math.Min(bedBottom - selectedPallet.Height, rawLoc.Y));
                Point newLoc = new Point(clampedX, clampedY);

                if (!_preventOverlap)
                {
                    // overlap‐off: move freely
                    selectedPallet.BackColor = Color.FromArgb(102, 99, 99);
                    selectedPallet.Location = newLoc;
                }
                else
                {
                    // try X only
                    var testX = new Point(newLoc.X, oldLoc.Y);
                    bool canX = !IsOverlapping(selectedPallet, testX);

                    // try Y only
                    var testY = new Point(oldLoc.X, newLoc.Y);
                    bool canY = !IsOverlapping(selectedPallet, testY);

                    // build allowed location
                    var allowed = oldLoc;
                    if (canX) allowed.X = newLoc.X;
                    if (canY) allowed.Y = newLoc.Y;

                    if (allowed != oldLoc)
                    {
                        // moved at least on one axis
                        selectedPallet.BackColor = Color.FromArgb(102, 99, 99);
                        selectedPallet.Location = allowed;
                    }
                    else
                    {
                        // blocked completely
                        selectedPallet.BackColor = Color.LightCoral;
                    }
                }

            }
        }
        private void Pallet_MouseMove(object sender, MouseEventArgs e)
        {
            // logika presunuta na vyssi uroven (panelu)
        }
        

        private void Pallet_MouseUp(object sender, MouseEventArgs e)
        {
            // logika presunuta na vyssi uroven (panelu)


        }



        private void RotatePallet(object Sender, EventArgs e)
        {

            if (selectedPallet != null)
            {
                int oldWidth = selectedPallet.Width;
                selectedPallet.Width = selectedPallet.Height;
                selectedPallet.Height = oldWidth;
                selectedPallet.Invalidate(); 
            }
        }
        private void DeletePallet(object sender, EventArgs e)
        {
            if (selectedPallet != null)
            {
                // Remove from the UI and free resources
                panel_truckBed.Controls.Remove(selectedPallet);
                selectedPallet.Dispose();
                selectedPallet = null;
            }
        }


        private void comboBox_Bedsize_SelectedIndexChanged(object sender, EventArgs e)
        {
            bedSize = comboBox_Bedsize.Text;
            
            panel_truckBed.Invalidate();
   
        }

       

        //pro debugging
        private void panel_truckBed_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        //extra
        private void button_export_Click(object sender, EventArgs e)
        {
            // dialog
            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "PDF Files|*.pdf";
                dlg.DefaultExt = "pdf";
                dlg.FileName = "NalozenyKamion.pdf";

                if (dlg.ShowDialog() != DialogResult.OK)
                    return;   

                
                ExportTruckBedToPdf(dlg.FileName);

                MessageBox.Show("Exported to " + dlg.FileName, "Done",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        //extra
        private void ExportTruckBedToPdf(string filename)
        {
            // 1) Bitmap
            var bmp = new Bitmap(panel_truckBed.Width, panel_truckBed.Height);
            panel_truckBed.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));

            // 2) Create PDF 
            var doc = new PdfSharp.Pdf.PdfDocument();
            var page = doc.AddPage();
            page.Orientation = PdfSharp.PageOrientation.Landscape;
            using (var gfx = PdfSharp.Drawing.XGraphics.FromPdfPage(page))
            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                var img = PdfSharp.Drawing.XImage.FromStream(ms);
                gfx.DrawImage(img, 0, 0, page.Width, page.Height);
            }
            doc.Save(filename);
        }

        private void checkBox_overlap_CheckedChanged(object sender, EventArgs e)
        {
            _preventOverlap = checkBox_overlap.Checked;
        }
        private bool IsOverlapping(Panel p, Point newLoc)
        {
            var candidate = new Rectangle(newLoc, p.Size);
            return panel_truckBed.Controls
                .OfType<Panel>()           // all pallets
                .Where(q => q != p)        // except the one we’re moving
                .Any(q => q.Bounds.IntersectsWith(candidate));
        }
        private void RenamePallet(object sender, EventArgs e)
        {
            if (selectedPallet == null) return;

            // Find the header label in the panel
            var lbl = selectedPallet.Controls
                       .OfType<Label>()
                       .FirstOrDefault();
            if (lbl == null) return;

            // Kick off the same inline‐edit logic as a double‐click
            Label_DoubleClick(lbl, EventArgs.Empty);
        }
        private void Label_DoubleClick(object sender, EventArgs e)
        {
            var lbl = (Label)sender;
            var panel = (Panel)lbl.Parent;

            var tb = new TextBox
            {
                Text = lbl.Text,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Center,
                Font = lbl.Font
            };

            void Commit()
            {
                panel.Controls.Clear();
                lbl.Text = string.IsNullOrWhiteSpace(tb.Text)
                           ? $"{panel.Width}×{panel.Height}"
                           : tb.Text;
                panel.Controls.Add(lbl);
            }

            tb.Leave += (s2, e2) => Commit();
            tb.KeyDown += (s2, ke) => {
                if (ke.KeyCode == Keys.Enter) { ke.Handled = true; ke.SuppressKeyPress = true; Commit(); }
            };

            panel.Controls.Clear();
            panel.Controls.Add(tb);
            tb.Focus();
        }

    }


}
