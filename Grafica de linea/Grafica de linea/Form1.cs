using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grafica_de_linea
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label57_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int width = pictureBox1.Width;
                int height = pictureBox1.Height;
                Bitmap bmp = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(bmp);
                g.Clear(Color.White);


                // Definir el origen y los escalares para el eje x y y
                int origenX = width / 2;
                int origenY = height / 2;
                int escalarX = width / 20; // Escalar para el eje X (ancho de 50 unidades)
                int escalarY = height / 20; // Escalar para el eje Y (altura de 20 unidades)

                // Dibujar los ejes x e y
                g.DrawLine(Pens.Black, 0, origenY, width, origenY); // Eje x
                g.DrawLine(Pens.Black, origenX, 0, origenX, height); // Eje y

                // Dibujar los números en el eje x
                Font font = new Font("Arial", 8);
                Brush brush = Brushes.Black;
                for (int i = -10; i <= 10; i++) // Ajustar el rango de números para un ancho de 50 unidades
                {
                    int x = origenX + i * escalarX;
                    int y = origenY + 10;
                    string text = i.ToString();
                    g.DrawString(text, font, brush, x, y);
                }

                // Dibujar los números en el eje y
                for (int i = -10; i <= 10; i++)
                {
                    int x = origenX - 10;
                    int y = origenY - i * escalarY;
                    string text = i.ToString();
                    g.DrawString(text, font, brush, x, y);
                }

                Pen pen = new Pen(Color.Black);

                int x1, y1, x2, y2;
                if (int.TryParse(textBox5.Text, out x1) && int.TryParse(textBox4.Text, out y1) &&
                    int.TryParse(textBox2.Text, out x2) && int.TryParse(textBox3.Text, out y2))
                {
                    DDA(g, pen, x1, y1, x2, y2, origenX, origenY, escalarX, escalarY);
                    pictureBox1.Image = bmp;

                    double m = CalculateSlope(x1, y1, x2, y2);
                    valor.Text = double.IsInfinity(m) ? "Pendiente infinita" : m.ToString();

                    string lineDirection = GetLineDirection(x1, y1, x2, y2);
                    FORMA.Text = $"La línea es {lineDirection}.";

                    double slope = CalculateSlope(x1, y1, x2, y2);
                    int lineCase = GetLineCase(slope, Math.Abs(x2 - x1), Math.Abs(y2 - y1));
                    MessageBox.Show($"La línea se encuentra en el caso {lineCase}.");
                    caso.Text = ($"caso {lineCase}.");

                }
                else
                {
                    MessageBox.Show("Por favor, ingrese valores enteros válidos en los cuadros de texto.");
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                MessageBox.Show("Ha ocurrido un error al dibujar la línea. Por favor, verifica los valores ingresados.");
            }
        }

        private void DDA(Graphics g, Pen pen, int x1, int y1, int x2, int y2, int origenX, int origenY, int escalarX, int escalarY)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
            float xinc = (float)dx / steps;
            float yinc = (float)dy / steps;
            float x = x1;
            float y = y1;
            int countery = 1;
            int counterx = 1;

            int prevX = origenX + Convert.ToInt32(x1 * escalarX);
            int prevY = origenY - Convert.ToInt32(y1 * escalarY);

            for (int i = 0; i <= steps; i++)
            {
                int pixelX = origenX + (int)Math.Round(x * escalarX);
                int pixelY = origenY - (int)Math.Round(y * escalarY);

                g.DrawLine(pen, prevX, prevY, pixelX, pixelY);

                prevX = pixelX;
                prevY = pixelY;

                X.Items.Add($"{counterx++}:({x})");
                Y.Items.Add($"{countery++}:({y})");

                x += xinc;
                y += yinc;
            }
        }


        private double CalculateSlope(int x1, int y1, int x2, int y2)
        {
            double dx = y1 - x1;
            double dy = y2 - x2;
            double pen= dy / dx;

            if (dx == 0)
            {
                return double.PositiveInfinity;
            }

            return pen;
        }
        private int GetLineCase(double slope, int dx, int dy)
        {
            if (slope >= 0 && slope < 1)
            {
                if (dx >= dy)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else if (slope == 1)
            {
                if (dx > dy)
                {
                    return 3;
                }
                else
                {
                    return 4;
                }
            }
            else if (slope > 1)
            {
                if (dx >= dy)
                {
                    return 5;
                }
                else
                {
                    return 6;
                }
            }
            else if (slope > -1 && slope < 0)
            {
                if (dx >= dy)
                {
                    return 7;
                }
                else
                {
                    return 8;
                }
            }
            else if (slope == -1)
            {
                if (dx > dy)
                {
                    return 9;
                }
                else
                {
                    return 10;
                }
            }
            else // slope < -1
            {
                if (dx >= dy)
                {
                    return 11;
                }
                else
                {
                    return 12;
                }
            }
        }

        private string GetLineDirection(int x1, int y1, int x2, int y2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;

            if (dx == 0)
            {
                return "Vertical";
            }

            double slope = dy / dx;

            if (slope > 1)
            {
                return "Positiva";
            }
            else if (slope < 1)
            {
                return "Negativa";
            }
            else
            {
                return "Horizontal";
            }
        }


        private void LogError(Exception ex)
        {
            Console.WriteLine(ex.Message);
            // Aquí puedes agregar el código para escribir el error en un archivo de texto.
        }

        

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Limpiar el picturebox
            LimpiarPictureBox(pictureBox1);
            // Limpiar los textbox
            textBox5.Text = "";
            textBox4.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            valor.Text = "";
            FORMA.Text = "";
            caso.Text = "";
            //baciar listbox
            X.Items.Clear();
            Y.Items.Clear();


        }

        private void LimpiarPictureBox(PictureBox pbox)
        {
            // Verificar que el picturebox tenga una imagen
            if (pbox.Image != null)
            {
                // Liberar los recursos de la imagen
                pbox.Image.Dispose();
                // Establecer la propiedad Image a null
                pbox.Image = null;
            }
        }

    }
}
