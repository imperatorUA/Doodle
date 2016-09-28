using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Doodle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();        
        }

        Bitmap myBitmap;
        Graphics gr;
        
        List<Circlecs> circlesList = new List<Circlecs>();

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBoxMain.Size = ClientSize;
            myBitmap = new Bitmap(pictureBoxMain.Width, pictureBoxMain.Height);
            gr = Graphics.FromImage(myBitmap);
           
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        { 
            ClearBitmap();
            int k = 1;
            foreach (Circlecs circle in circlesList)
            {
                circle.Draw(gr);//отрисовка  

                // столкновение
                for (int i = k; i < circlesList.Count; i++)
                {
                    if (circle.SmashCirles(circlesList[i]) == true && circlesList.Count >= 2)
                    {
                        ChangeTrajectory(circle, circlesList[i]);
                    }
                }
                k++;
            }     
            pictureBoxMain.Image = myBitmap;
        }

        private void ChangeTrajectory(Circlecs circl1, Circlecs circl2)
        {
            //circl2.Dx = circl1.Dx;
            //circl2.Dy = circl1.Dy;
            //double d = Math.Sqrt(Math.Pow(circl1.X + circl2.X, 2) + Math.Pow(circl1.Y + circl2.Y, 2));//растояние между центрами кругов
            //double sinL = Math.Abs(circl1.Y - circl2.Y) / d,
            //       cosL = Math.Abs(circl1.X - circl2.X) / d;

            //double Vn1 = circl1.Dx * cosL + circl1.Dy * sinL,
            //       Vt1 = circl1.Dy * cosL - circl1.Dx * sinL;

            //double Vn2 = circl2.Dx * cosL + circl2.Dy * sinL,
            //       Vt2 = circl2.Dy * cosL - circl2.Dx * sinL;

            //Swap(ref Vn1, ref Vn2);

            //circl1.Dx = (int)Math.Round(Vn1 * cosL - Vt1 * sinL);
            //circl1.Dy = (int)Math.Round(Vn1 * sinL + Vt1 * cosL);

            //circl2.Dx = (int)Math.Round(Vn2 * cosL - Vt2 * sinL);
            //circl2.Dy = (int)Math.Round(Vn2 * sinL + Vt2 * cosL);
        }

        private void Swap(ref double V1, ref double V2)
        {
            double x = V1;
            V1 = V2;
            V2 = x;
        }

        private void ClearBitmap()
        {
            gr.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 255, 136)), 0, 0, pictureBoxMain.Width, pictureBoxMain.Height);
        }

        private void pictureBoxMain_MouseClick(object sender, MouseEventArgs e)
        {
            //остановка кругов
            foreach (Circlecs circl in circlesList)
            {
                if (circl.MouseEnter(e.X, e.Y) == true)
                {
                    if(circl.Move == true)
                        circl.Move = false;
                    else
                        circl.Move = true;
                    return;
                }
            }

            if (e.X < 150 || e.X > pictureBoxMain.Size.Width - 150 || e.Y < 150 || e.Y > pictureBoxMain.Size.Height - 150) return;  //для создания кругов в центре экрана 
                    
            Random rand = new Random();
            int radius = rand.Next(50, 100);
            Circlecs c = new Circlecs(rand.Next(3, 8), rand.Next(0, 361), e.X - radius, e.Y - radius, radius, 
                Color.FromArgb(120, rand.Next(1, 255), rand.Next(5, 255), rand.Next(1, 255)), pictureBoxMain.Size);
            //foreach (Circlecs circl in circlesList)//дополнительная проверка на пересечение с другими кругами
            //    if (c.SmashCirles(circl) == true) return;
            circlesList.Add(c);
        }

        //изменение разиера\скорости кнопками
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            int size = 0, speed = 0;
        
            switch (e.KeyValue)
            {
                case 38:
                    size = 16;
                    break;
                case 40:
                    size = -16;
                    break;
                case 37:
                    speed = -1;
                    break;
                case 39:
                    speed = 1;
                    break;
                default:
                    return;
            }

            foreach (Circlecs circl in circlesList)
            {
                //изменение скорости
                if (circl.Dx >= 0)
                    circl.Dx = circl.Dx + speed;
                else
                    circl.Dx = circl.Dx - speed;

                if (circl.Dy >= 0)
                    circl.Dy = circl.Dy + speed;
                else
                    circl.Dy = circl.Dy - speed;

                //изменение размера
                if (size > 0 && (circl.X < circl.Radius + 10 || circl.X > pictureBoxMain.Width - circl.Radius * 2 + 10 || circl.Y < circl.Radius + 10 || circl.Y > pictureBoxMain.Height - circl.Radius * 2 + 10 || (circl.Radius * 2 + 5 < Math.Abs(size) && size < 0)))
                    continue;
                if (circl.Radius < Math.Abs(size) + 5 && size < 0)
                    continue;
                circl.Radius = circl.Radius + size;
                circl.X = circl.X - size / 2;
                circl.Y = circl.Y - size / 2;               
            }
        }
    }
}
