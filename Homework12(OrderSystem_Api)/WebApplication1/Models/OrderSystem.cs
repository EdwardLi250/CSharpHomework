using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace WebApplication1
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
            this.Database.EnsureCreated(); 
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

    }
    public class Order
    {

        public Order() { }
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]//不自动增长
        public int OrderID { get; set; }

        public string Customer { get; set; }
        private bool IsCompleted { get; set; }
        public DateTime Time { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderItem> Orderitems { get; set; }
        public Order(int id, string customer, bool iscompleted,
            DateTime time, List<OrderItem> items)
        {
            OrderID = id;
            Customer = customer;
            IsCompleted = iscompleted;
            Time = time;
            TotalPrice = 0;
            foreach (OrderItem item in items)
                TotalPrice += item.Price * item.Quantity;
            Orderitems = items;
        }


        public override string ToString()
        {
            StringBuilder all_items = new StringBuilder();
            foreach (OrderItem oi in Orderitems)
            {
                all_items.Append(oi.ToString() + "\n");
            }
            return "ID: " + OrderID + "   " + "Created time: " + Time + "   " + "Customer's name: " + Customer
                + "\n" + all_items.ToString() +
                "Total price: " + TotalPrice + "\n" + "Completed: " + IsCompleted;
        }
        public override bool Equals(object obj)
        {
            return obj is Order order &&
                   OrderID == order.OrderID;
        }

        public override int GetHashCode()
        {
            var hashCode = -236961719;
            hashCode = hashCode * -1521134295 + OrderID.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Customer);
            hashCode = hashCode * -1521134295 + IsCompleted.GetHashCode();
            hashCode = hashCode * -1521134295 + Time.GetHashCode();
            hashCode = hashCode * -1521134295 + TotalPrice.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<OrderItem>>.Default.GetHashCode(Orderitems);
            return hashCode;
        }
    }

    public class OrderItem
    {
        public OrderItem() { }
        [Key]
        public int ItemNum { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }
        public OrderItem(string name, int number, double price)
        {
            Name = name;
            Quantity = number;
            Price = price;
        }

        public override string ToString()
        {
            return "Product name: " + Name + "   " + "Quantity: " + Quantity + "   Price: " + Price;
        }
        public override bool Equals(object obj)
        {
            return obj is OrderItem item &&
                   Name == item.Name &&
                   Quantity == item.Quantity &&
                   Price == item.Price;
        }

        public override int GetHashCode()
        {
            var hashCode = -1978195187;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Quantity.GetHashCode();
            hashCode = hashCode * -1521134295 + Price.GetHashCode();
            return hashCode;
        }
    }

}
