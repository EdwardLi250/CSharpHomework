using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleCrawler
{
    public partial class Form1 : Form
    {
        SimpleCrawler crawler = new SimpleCrawler();
        public Form1()
        {
            InitializeComponent();
            crawler.PageDownloaded += Crawler_PageDownloaded;
            textBox1.Clear();
        }

        private void Crawler_PageDownloaded(string obj)
        {
            if (this.listBox1.InvokeRequired)
            {
                Action<String> action = this.AddUrl;
                this.Invoke(action, new object[] { obj });
            }
            else
            {
                listBox1.Items.Add(obj);
            }
        }

        private void AddUrl(string url)
        {
            listBox1.Items.Add(url);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            crawler.AddUrl(textBox1.Text);
            listBox1.Items.Clear();
            new Thread(crawler.Crawl).Start();
        }
    }

}

