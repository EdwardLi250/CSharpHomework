using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsoleApp11;

namespace OrderSystem_Winform
{
    public partial class Form2 : Form
    {
        List<OrderItem> order_items = new List<OrderItem>();
        public static Order new_order { get; set; }
        public static bool add_flag;
        public static int order_id;
        public Form2()
        {
            InitializeComponent();
            add_flag = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OrderItem item = new OrderItem(textBox4.Text, Int32.Parse(textBox5.Text), Double.Parse(textBox6.Text));
                order_items.Add(item);
                textBox4.Clear(); textBox5.Clear(); textBox6.Clear();
                orderItemBindingSource.DataSource = order_items;
                orderItemBindingSource.ResetBindings(false);
            }
            catch(System.FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                order_id = Int32.Parse(textBox1.Text);
                new_order = new Order(Int32.Parse(textBox1.Text), textBox2.Text, checkBox1.Checked, DateTime.Parse(textBox3.Text), order_items);
                add_flag = true;
            }
            catch (System.FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
