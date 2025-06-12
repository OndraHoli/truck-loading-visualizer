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

        // nevim
        int dragOffsetX;
        int dragOffsetY;

        bool draggingX;
        bool draggingY;
        //nevim
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
                int centerX = panel_truckBed.Width / 2;
                int centerY = panel_truckBed.Height / 2;
                e.Graphics.DrawRectangle(pen, (centerX - bedX / 2) - 5, (centerY - bedY / 2) - 5, bedX + 5, bedY + 5);
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

            using (Pen pen = new Pen(Color.Cyan, 2))
            {
                e.Graphics.DrawLine(pen, 0, 0, panel.Width, panel.Height);

                
                e.Graphics.DrawLine(pen, 0, panel.Height, panel.Width, 0);

                e.Graphics.DrawRectangle(pen,0,0,panel.Width, panel.Height);
            }
        }
        
        private void Pallet_MouseDown(object sender, MouseEventArgs e)
        {
            
            selectedPallet = sender as Panel;
            if (selectedPallet != null)
            {
                isDragging = true;
                dragOffset = e.Location;
                
                selectedPallet.BackColor = Color.LightGray; 
                selectedPallet.BringToFront();
            }
        }
        private void Pallet_MouseMove(object sender, MouseEventArgs e)
        {

            selectedPallet = sender as Panel;
            //MessageBox.Show("Move");
            if (isDragging && selectedPallet != null)
            {
                Point newLocation = selectedPallet.Location;

         
                int deltaX = e.X - dragOffset.X;
                int deltaY = e.Y - dragOffset.Y;

                Point mouseInPallet = e.Location;
                Point mouseInTruckBed = selectedPallet.PointToScreen(mouseInPallet);
                mouseInTruckBed = panel_truckBed.PointToClient(mouseInTruckBed);


                int leftOffset = e.Location.X; 
                int rightOffset = selectedPallet.Width - e.Location.X;
                int topOffset = e.Location.Y;  
                int bottomOffset = selectedPallet.Height - e.Location.Y;
                
                int centerX = panel_truckBed.Width / 2;
                int centerY = panel_truckBed.Height / 2;

                int xCorBed = centerX - bedX / 2;
                int yCorBed = centerY - bedY / 2;

                

                /*
                bool rightStrip = (mouseInTruckBed.X + rightOffset >= xCorBed + bedX && mouseInTruckBed.X + rightOffset <= xCorBed + bedX + 50);
                bool leftStrip = (mouseInTruckBed.X - leftOffset  <= xCorBed && mouseInTruckBed.X - leftOffset >= xCorBed - 50);
                bool topStrip = (mouseInTruckBed.Y - topOffset <= yCorBed && mouseInTruckBed.Y - topOffset >= yCorBed + 50);
                bool downStrip = (mouseInTruckBed.Y + bottomOffset >= yCorBed + bedY && mouseInTruckBed.Y + bottomOffset <= yCorBed + bedY + 50);
                */

                int buffer = 100;

                bool rightStrip = (mouseInTruckBed.X + rightOffset >= xCorBed + bedX &&
                                   mouseInTruckBed.X + rightOffset <= xCorBed + bedX + buffer);

                bool leftStrip = (mouseInTruckBed.X - leftOffset <= xCorBed &&
                                  mouseInTruckBed.X - leftOffset >= xCorBed - buffer);

                bool topStrip = (mouseInTruckBed.Y - topOffset <= yCorBed &&
                                 mouseInTruckBed.Y - topOffset >= yCorBed - buffer);

                bool downStrip = (mouseInTruckBed.Y + bottomOffset >= yCorBed + bedY &&
                                  mouseInTruckBed.Y + bottomOffset <= yCorBed + bedY + buffer);

                /*
                if (!(leftStrip || rightStrip))
                {
                    newLocation.X += deltaX;
                }

                if (!(topStrip || downStrip))
                {
                    newLocation.Y += deltaY;
                }
                */

                bool allowX = true;
                bool allowY = true;

                

                if (leftStrip && deltaX < 0) allowX = false;    // Trying to move left into buffer
                if (rightStrip && deltaX > 0) allowX = false;   // Trying to move right into buffer

                if (topStrip && deltaY < 0) allowY = false;     // Trying to move up into buffer
                if (downStrip && deltaY > 0) allowY = false;    // Trying to move down into buffer

                if (allowX)
                    newLocation.X += deltaX;

                if (allowY)
                    newLocation.Y += deltaY;





                Control parent = selectedPallet.Parent;
                if (parent != null)
                {
                    newLocation.X = Math.Max(0, Math.Min(parent.Width - selectedPallet.Width, newLocation.X));
                    newLocation.Y = Math.Max(0, Math.Min(parent.Height - selectedPallet.Height, newLocation.Y));
                }



                selectedPallet.Location = newLocation;

                label_X.Text = Convert.ToString(xCorBed);
                label_Y.Text = Convert.ToString(yCorBed);






            }
        }
        

        private void Pallet_MouseUp(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("Up");
            isDragging = false;
            if (selectedPallet != null)
            {
                selectedPallet.BackColor = Color.FromArgb(102, 99, 99);
                selectedPallet = null;
            }
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
