using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using ConsoleApp11;

namespace OrderSystem_Winform
{
    
    public partial class Form1 : Form
    {
        OrderService service = new OrderService();
        public Form1()
        {
            InitializeComponent();
            List<OrderItem> order1_items = new List<OrderItem>();
            List<OrderItem> order2_items = new List<OrderItem>();
            OrderItem order1_item1 = new OrderItem("Blonde", 3, 50);
            OrderItem order1_item2 = new OrderItem("Orange", 2, 35);
            OrderItem order2_item1 = new OrderItem("IGOR", 5, 50);
            order1_items.Add(order1_item1); order1_items.Add(order1_item2);
            order2_items.Add(order2_item1);
            Order order1 = new Order(1, "FO", true, DateTime.Parse("2020/02/01 11:22:33"), order1_items);         
            Order order2 = new Order(2, "TC", true, DateTime.Parse("2020/02/01 11:22:33"), order2_items);
            
            service.addOrder(order1);
            service.addOrder(order2);
            orderBindingSource.DataSource = service.Orders;
            
        }
/*       private void orderBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            orderItemBindingSource.DataSource = service.orders[orderBindingSource.IndexOf(orderBindingSource.Current)].orderitems;
        }
*/
        private void button2_Click(object sender, EventArgs e)
        {
            List<Order> searched_list = new List<Order>();
            try
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        searched_list = service.searchOrder_byID(Int32.Parse(textBox1.Text));
                        break;
                    case 1:
                        searched_list = service.searchOrder_byCustomer(textBox1.Text);
                        break;
                    case 2:
                        searched_list = service.searchOrder_byTime(DateTime.Parse(textBox1.Text));
                        break;
                }
                orderBindingSource.DataSource = searched_list;
                if(searched_list.Count == 0)
                {
                    MessageBox.Show("订单不存在", "查找订单错误");
                }
            }
            catch(System.FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 导出订单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "XML文件(*.xml)|*.xml";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {               
                    service.Export(saveFileDialog1.FileName);  
            }
        }

        private void 导入订单ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "XML文件(*.xml)|*.xml";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                service.Import(openFileDialog1.FileName);
            }
            orderBindingSource.DataSource = service.Orders;
        }

        private void 添加订单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
            
            if (Form2.add_flag == true)
            {
                //service.addOrder(Form2.new_order);
                if(service.addOrder(Form2.new_order) == false)
                {
                    MessageBox.Show("订单编号已存在", "新增订单错误");
                }
                orderBindingSource.DataSource = service.Orders;
                orderBindingSource.ResetBindings(false);
            }
        }

        private void 修改订单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
            if(service.updateOrder(Form2.order_id, Form2.new_order) == false)
            {
                MessageBox.Show("订单编号不存在", "修改订单错误");
            }
            orderBindingSource.DataSource = service.Orders;
            orderBindingSource.ResetBindings(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Order delete_order = orderBindingSource.Current as Order;
            if(service.deleteOrder(delete_order.OrderID) == false)
            {
                MessageBox.Show("订单不存在", "删除订单错误");
            }
            orderBindingSource.DataSource = service.Orders;
            orderBindingSource.ResetBindings(false);
        }

    }
}
