using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nakladka_vizual
{
    public partial class Form1 : Form
    {
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
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

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
        private void Panel_truckBed_MouseUp(object sender, MouseEventArgs e)
        {
            // Exactly the same cleanup you had in Pallet_MouseUp:
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
            //    we still need dragOffset from pallet, so translate panel coords to pallet-local:
            Point palletLocal = selectedPallet.PointToClient(panel_truckBed.PointToScreen(e.Location));
            int deltaX = palletLocal.X - dragOffset.X;
            int deltaY = palletLocal.Y - dragOffset.Y;

            // 2) mouseInBed is just e.Location
            Point mouseInBed = e.Location;

            // 3) compute bed bounds
            int centerX = panel_truckBed.Width / 2;
            int centerY = panel_truckBed.Height / 2;
            int xCorBed = centerX - bedX / 2;
            int yCorBed = centerY - bedY / 2;
            int buffer = 50; // or whatever

            // 4) offsets from pallet edge
            int leftOffset = palletLocal.X;
            int rightOffset = selectedPallet.Width - palletLocal.X;
            int topOffset = palletLocal.Y;
            int bottomOffset = selectedPallet.Height - palletLocal.Y;

            // 5) distances from each edge
            
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

            


            // 11) Optional debug labels
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



        private void RotatePallet(Panel pallet)
        {
            int oldWidth = pallet.Width;
            pallet.Width = pallet.Height;
            pallet.Height = oldWidth;

            // Redraw after rotation
            pallet.Invalidate();
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
    }
}
