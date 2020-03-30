using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


/*将课本中例5-31的Cayley树绘图代码进行修改。添加一组控件用以调节树的绘制参数。
 * 参数包括递归深度（n）、主干长度（leng）、右分支长度比（per1）、左分支长度比（per2）、
 * 右分支角度（th1）、左分支角度（th2）、画笔颜色（pen）*/
namespace WindowsFormsApp2_Cayley_Tree
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.panel1.BackColor = Color.White;
        }

        private Graphics graphics;
        double th1 = 30 * Math.PI / 180;
        double th2 = 20 * Math.PI / 180;
        double per1 = 0.6;
        double per2 = 0.7;
        static Color color;

        private void button1_Click(object sender, EventArgs e)
        {   
            if (graphics == null) graphics = this.panel1.CreateGraphics();         
            graphics.Clear(Color.White);
            int n = Int32.Parse(numericUpDown1.Value.ToString());
            double height = Double.Parse(textBox2.Text);
            per1 = (double)trackBar1.Value / 10;
            per2 = (double)trackBar2.Value / 10;
            th1 = Double.Parse(textBox5.Text);
            th2 = Double.Parse(textBox6.Text);
            
            drawCayleyTree(n, 200, 380, height, -Math.PI / 2);
        }

      
        void drawCayleyTree(int n, double x0, double y0, double leng, double th)
        {
            if (n == 0) return;
            double x1 = x0 + leng * Math.Cos(th);
            double y1 = y0 + leng * Math.Sin(th);

            drawLine(x0, y0, x1, y1);

            drawCayleyTree(n - 1, x1, y1, per1 * leng, th + th1);
            drawCayleyTree(n - 1, x1, y1, per2 * leng, th - th2);
        }
        public Form1(ColorDialog colorDialog1)
        {
            this.colorDialog1 = colorDialog1;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = colorDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                color = colorDialog1.Color;
                button2.BackColor = colorDialog1.Color;
                
            }
        }
        void drawLine(double x0,double y0,double x1, double y1)
        {
            Pen pen = new Pen(color);
            graphics.DrawLine(pen, (int)x0, (int)y0, (int)x1, (int)y1);
        }



    }
}
