using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string numInput1 = "";
            string numInput2 = "";
            double result = double.NaN;//设置计算结果默认值为NaN，用于检测异常等
            
            numInput1 = textBox1.Text;
            double cleanNum1 = 0;
            if (!double.TryParse(numInput1, out cleanNum1))
            {
                MessageBox.Show("第一个输入不符合条件，请输入一个整数", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Text = string.Empty;
                return;
            }

            numInput2 = textBox2.Text;
            double cleanNum2 = 0;
            if (!double.TryParse(numInput2, out cleanNum2))
            {
                MessageBox.Show("第二个输入不符合条件，请输入一个整数", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox2.Text = string.Empty;
                return;
            }

            string op = textBox3.Text;
            switch (op)
            {
                case "+":
                    result = cleanNum1 + cleanNum2;
                    break;
                case "-":
                    result = cleanNum1 - cleanNum2;
                    break;
                case "*":
                    result = cleanNum1 * cleanNum2;
                    break;
                case "/":
                    if (cleanNum2 != 0)
                    {
                        result = cleanNum1 / cleanNum2;
                    }
                    break;
                default: break;
            }

            try
            { 
                if (double.IsNaN(result))
                {
                    MessageBox.Show("该计算有误，无法完成", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else textBox4.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("计算出现异常" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
