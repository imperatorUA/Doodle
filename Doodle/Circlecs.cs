using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Doodle
{
    class Circlecs
    {
        private int angel;
        private int speed;
        private int x, y;
        private int radius;
        private Color color;
        private Size sizeMonitor;

        private Boolean move; //движение

        private int dx, dy;
        private double angelRad;

        enum Side { Top, Down, Left, Right, NaN };

        private int Angel
        {
            get
            { return angel; }

            set
            {
                if (value >= 0 && value <= 360)
                    angel = value;
                else
                    throw new Exception("Angel false");
            }
        }

        public int Speed
        {
            get
            {
                return speed;
            }

            set
            {
                speed = value;
            }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        
        public int Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Size SizeMonitor
        {
            get { return sizeMonitor; }
            set { sizeMonitor = value; }
        }
        
        public int Dx
        {
            get
            {
                return dx;
            }

            set
            {
                dx = value;
            }
        }

        public int Dy
        {
            get
            {
                return dy;
            }

            set
            {
                dy = value;
            }
        }

        public bool Move
        {
            get
            {
                return move;
            }

            set
            {
                move = value;
            }
        }

        public Circlecs(int speed, int angel, int x, int y, int radius, Color color, Size sizeMonitor)
        {
            Speed = speed;
            Angel = angel;
            X = x;
            Y = y;
            Radius = radius;
            Color = color;
            SizeMonitor = sizeMonitor;
            move = true;

            IncrementArguments();
        }

        public void IncrementArguments()//Аргументы прирощения
        {
            angelRad = (Math.PI * angel) / 180;
            Dx = (int)Math.Round(Math.Cos(angelRad) * (double)speed);
            Dy = (int)Math.Round(Math.Sin(angelRad) * (double)speed);
        }

        private void ChangeAngel(Side mode)//Измение угла от борта
        {
            switch (mode)
            {
                case Side.Down:
                    if (Dy > 0) Dy = Dy * -1;
                    break;
                case Side.Top:
                    if (Dy < 0) Dy = Dy * -1;
                    break;
                case Side.Right:
                    if (Dx > 0) Dx = Dx * -1;
                    break;
                case Side.Left:
                    if (Dx < 0) Dx = Dx * -1;
                    break;
            }
        }

        private Side Recapture()//Пересечение со стороной
        {
            if (x <= 0) return Side.Left;
            if (x >= sizeMonitor.Width - radius * 2) return Side.Right;
            if (y <= 0) return Side.Top;
            if (y >= sizeMonitor.Height - radius * 2) return Side.Down;
            return Side.NaN;
        }

        public void Draw(Graphics gr)//Отрисовка
        {
            gr.FillEllipse(new SolidBrush(color), x, y, radius * 2, radius * 2);
            //for (int i = 0; i <= 80; i = i + 20)
            //{
            //    gr.DrawEllipse(new Pen(Color.Black, 5), x + i / 2, y + i / 2, radius * 2 - i, radius * 2 - i);
            //}
            gr.DrawEllipse(new Pen(Color.Black, 5), x, y, radius * 2, radius * 2);
            if (move == true) MoveCircl();
        }

        private void MoveCircl()//Движение
        {
            x += Dx;
            y += Dy;          
            if (Recapture() != Side.NaN) ChangeAngel(Recapture());
        }

        public Boolean SmashCirles(Circlecs cirlc)//столкновение шаров
        {
            return (Math.Sqrt(Math.Pow(x + radius - cirlc.x - cirlc.Radius, 2) + Math.Pow(y + radius - cirlc.y - cirlc.Radius, 2)) <=  radius + cirlc.Radius + (Math.Sqrt(Dx * Dx + Dy * Dy) + Math.Sqrt(cirlc.Dx * cirlc.Dx + cirlc.Dy * cirlc.Dy)) * 0.1);
        }

        public double SmashDistansCirles(Circlecs cirlc)//растояние между шарами
        {
            return Math.Abs(Math.Sqrt(Math.Pow(x + radius - cirlc.x - cirlc.Radius, 2) + Math.Pow(y + radius - cirlc.y - cirlc.Radius, 2)) - (radius + cirlc.Radius));
        }

        public Boolean MouseEnter(int x0, int y0)
        {
            if (Math.Pow(x0 - (x  + radius), 2) + Math.Pow(y0 - (y + radius), 2) <= radius * radius) return true;
            return false;
        }
    }
}
