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

        public Form1()
        {
            InitializeComponent();
            palletContextMenu = new ContextMenuStrip();
            palletContextMenu.Items.Add("Rotate", null, RotatePallet);

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

            using (Pen pen = new Pen(Color.Yellow, 5))
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
            Pallet.BackColor = Color.FromArgb(102, 99, 99);

            Pallet.ContextMenuStrip = palletContextMenu;

            //back image
            //Pallet.BackgroundImage = Image.FromFile("C:\\Users\\Ondra\\source\\repos\\nakladka_vizual\\paleta.png");
            //Pallet.BackgroundImageLayout = ImageLayout.Stretch;

            Pallet.Paint += drawCross;

            Pallet.Size = new Size(widthPallet, heigthPallet);
            Pallet.Location = new Point(300, 300);
            Pallet.MouseDown += Pallet_MouseDown;
            Pallet.MouseMove += Pallet_MouseMove;
            Pallet.MouseUp += Pallet_MouseUp;

            

            panel_truckBed.Controls.Add(Pallet);
        }

        private void drawCross(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null) return;

            using (Pen pen = new Pen(Color.Cyan, 5))
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
            if(e.Button == MouseButtons.Right)
            {
                selectedPallet = sender as Panel;
                return;
            }

            if(e.Button == MouseButtons.Left)
            {
                selectedPallet = sender as Panel;
                if (selectedPallet == null) return;

                isDragging = true;
                dragOffset = e.Location;
                selectedPallet.BackColor = Color.LightGray;
                selectedPallet.BringToFront();

                // Capture and hook both Move *and* Up on the panel
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
                selectedPallet.BackColor = Color.FromArgb(102, 99, 99);
                selectedPallet = null;
            }

            // Release capture and unhook both Move and Up
            panel_truckBed.Capture = false;
            panel_truckBed.MouseMove -= Panel_truckBed_MouseMove;
            panel_truckBed.MouseUp -= Panel_truckBed_MouseUp;
        }
        private void Panel_truckBed_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging || selectedPallet == null) return;

            // 1) deltas in pallet coords
           
            Point palletLocal = selectedPallet.PointToClient(panel_truckBed.PointToScreen(e.Location));
            int deltaX = palletLocal.X - dragOffset.X;
            int deltaY = palletLocal.Y - dragOffset.Y;

            // 2) mouseInBed is just e.Location
            Point mouseInBed = e.Location;

            // 3) bed bounds
            int centerX = panel_truckBed.Width / 2;
            int centerY = panel_truckBed.Height / 2;
            int xCorBed = centerX - bedX / 2;
            int yCorBed = centerY - bedY / 2;
            int buffer = 50; // or whatever

            // 4) offsets 
            int leftOffset = palletLocal.X;
            int rightOffset = selectedPallet.Width - palletLocal.X;
            int topOffset = palletLocal.Y;
            int bottomOffset = selectedPallet.Height - palletLocal.Y;

            // 5) distances 
            
            int distLeft = (mouseInBed.X - leftOffset) - xCorBed;
            int distRight = (mouseInBed.X + rightOffset) - (xCorBed + bedX);
            int distTop = (mouseInBed.Y - topOffset) - yCorBed;
            int distBottom = (mouseInBed.Y + bottomOffset) - (yCorBed + bedY);

            // 6) zone tests
            bool inLeftBuf = distLeft < 0 && distLeft > -buffer;
            bool outLeftBuf = distLeft <= -buffer;
            bool inRightBuf = distRight > 0 && distRight < buffer;
            bool outRightBuf = distRight >= buffer;
            bool inTopBuf = distTop < 0 && distTop > -buffer;
            bool outTopBuf = distTop <= -buffer;
            bool inBottomBuf = distBottom > 0 && distBottom < buffer;
            bool outBottomBuf = distBottom >= buffer;


            // 7) allow/block
            bool allowX = !((inLeftBuf && deltaX < 0) || (inRightBuf && deltaX > 0));
            bool allowY = !((inTopBuf && deltaY < 0) || (inBottomBuf && deltaY > 0));

            
            bool cursorOutX = e.X <= xCorBed - buffer || e.X >= xCorBed + bedX + buffer;
            bool cursorOutY = e.Y <= yCorBed - buffer || e.Y >= yCorBed + bedY + buffer;

                    

            

            if(cursorOutX || cursorOutY && hasSnappedOut == false)
            {
                hasSnappedOut = true;
                hasSnappedIn = false;
                deltaX = 0; deltaY = 0; // je to potreba??
                selectedPallet.Location = e.Location;

            }

            if (!(cursorOutX || cursorOutY) && hasSnappedIn == false)
            {
                hasSnappedIn = true;
                hasSnappedOut = false;
                deltaX = 0; deltaY = 0;
                selectedPallet.Location = e.Location;
            }
            
            


            label_X4.Text = "IN";
            Point newLoc = selectedPallet.Location;
            if (allowX) newLoc.X += deltaX;
            if (allowY) newLoc.Y += deltaY;
            selectedPallet.Location = newLoc;

            


            // debug
            label_X3.Text = inLeftBuf ? "IN LEFT BUF" :
                            outLeftBuf ? "LEFT OUT" :
                            inRightBuf ? "IN RIGHT BUF" :
                            outRightBuf ? "RIGHT OUT" :
                                           "X INSIDE";
            label_Y3.Text = inTopBuf ? "IN TOP BUF" :
                            outTopBuf ? "TOP OUT" :
                            inBottomBuf ? "IN BOT BUF" :
                            outBottomBuf ? "BOT OUT" :
                                           "Y INSIDE";
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


        private void comboBox_Bedsize_SelectedIndexChanged(object sender, EventArgs e)
        {
            bedSize = comboBox_Bedsize.Text;
            
            panel_truckBed.Invalidate();
   
        }

       

        //pro debugging
        private void panel_truckBed_MouseMove(object sender, MouseEventArgs e)
        {
            label_X2.Text = Convert.ToString(e.X);
            label_Y2.Text = Convert.ToString(e.Y);
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
    }
}
