using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using System.Data.Entity;


namespace OrderSystem_Winform
{
    public class OrderContext : DbContext
    {
        public OrderContext() : base("OrderSystemDataBase")
        {
            Database.SetInitializer(
              new DropCreateDatabaseIfModelChanges<OrderContext>());
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
    public class OrderService
    {
        //public List<Order> Orders;
        public OrderService()
        {
            //Orders = new List<Order>();
        }
        public List<Order> Orders
        {
            get
            {
                using (var context = new OrderContext())
                {
                    return context.Orders.ToList();
                }
            }

        }
        public bool addOrder(Order order)
        {
            foreach (Order o in Orders)
            {
                if (o.Equals(order))
                    return false;
            }
            //orders.Add(order);
            using (var context = new OrderContext())
            {
                context.Orders.Add(order);
                context.SaveChanges();

            }
            return true;
        }
        public bool deleteOrder(int id)
        {
            try
            {
                //Orders.Remove(Orders.Where(p => p.OrderID == id).First());

                using (var context = new OrderContext())
                {
                    var order = context.Orders.Include("Orderitems").Where(p => p.OrderID == id).First();
                    context.Orders.Remove(order);
                    //context.Orders.Remove(Orders.Where(p => p.OrderID == id).First());
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool updateOrder(int id, Order neworder)
        {
            try
            {
                using (var context = new OrderContext())
                {
                    //int order_number = Orders.FindIndex(o => o.OrderID == id);
                    //Orders[order_number] = neworder;
                    var order = context.Orders.Include("Orderitems").Where(p => p.OrderID == id).First();
                    context.Orders.Remove(order);
                    context.Orders.Add(neworder);
                    context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void sortOrder()
        {
            using (var context = new OrderContext())
            {
                context.Orders.ToList().Sort((o1, o2) => o1.OrderID - o2.OrderID);
                context.SaveChanges();

            }
        }
        public List<Order> searchOrder_byID(int id)
        {
            using (var context = new OrderContext())
            {
                var query = context.Orders.Include("Orderitems").Where(p => p.OrderID == id);
                //var query = Orders.Where(o => o.OrderID == id);
                return query.ToList();
            }
        }
        public List<Order> searchOrder_byCustomer(string order_customer)
        {
            using (var context = new OrderContext())
            {
                var query = context.Orders.Include("Orderitems").Where(p => p.Customer == order_customer).OrderBy(o => o.TotalPrice);
                //var query = Orders.Where(o => o.Customer == order_customer).OrderBy(o => o.TotalPrice);
                return query.ToList();
            }
        }
        public List<Order> searchOrder_byTime(DateTime order_time)
        {
            using (var context = new OrderContext())
            {
                var query = context.Orders.Include("Orderitems").Where(p => p.Time == order_time).OrderBy(o => o.TotalPrice);
                //var query = Orders.Where(o => o.Time == order_time).OrderBy(o => o.TotalPrice);
                return query.ToList();
            }
        }
        public List<Order> PrintOrders()
        {
            using (var context = new OrderContext())
            {
                return context.Orders.ToList();
            }
            //return Orders;
        }
        public void Export(string filename)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Order>));
            using (FileStream file = new FileStream(filename + ".xml", FileMode.Create))
            {
                using (var context = new OrderContext())
                {
                    xmlSerializer.Serialize(file, context.Orders.ToList());
                }
            }
        }
        public void Import(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Order>));
                List<Order> orderlist = (List<Order>)xmlSerializer.Deserialize(file);
                using (var context = new OrderContext())
                {
                    foreach (Order o in orderlist)
                    {
                        context.Orders.Add(o);
                        context.SaveChanges();
                    }
                }

            }

        }
    }

}

