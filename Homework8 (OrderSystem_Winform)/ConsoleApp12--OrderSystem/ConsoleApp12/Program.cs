﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
/*在OrderService中添加一个Export方法，可以将所有的订单序列化为XML文件；
* 添加一个Import方法可以从XML文件中载入订单。
*/
namespace ConsoleApp11
{
    
    public class Order
    {
        public Order() { }
        public int ID { get; set; }
        public string Customer { get; set; }
        private bool IsCompleted { get; set; }
        public DateTime Time { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderItem> orderitems;
        public Order(int id, string customer, bool iscompleted,
            DateTime time, List<OrderItem> items)
        {
            ID = id;
            Customer = customer;
            IsCompleted = iscompleted;
            Time = time;
            TotalPrice = 0;
            foreach (OrderItem item in items)
                TotalPrice += item.Price * item.Quantity;
            orderitems = items;
        }


        public override string ToString()
        {
            StringBuilder all_items = new StringBuilder();
            foreach (OrderItem oi in orderitems)
            {
                all_items.Append(oi.ToString() + "\n");
            }
            return "ID: " + ID + "   " + "Created time: " + Time + "   " + "Customer's name: " + Customer 
                + "\n" + all_items.ToString() + 
                "Total price: " + TotalPrice + "\n" + "Completed: " + IsCompleted;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Customer, IsCompleted, Time, orderitems);
        }

        public override bool Equals(object obj)
        {
            return obj is Order order &&
                   ID == order.ID ;
        }
    }
    public class OrderItem
    {
        public OrderItem() { }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
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
            return HashCode.Combine(Name, Quantity, Price);
        }
    }
    public class OrderService
    {
        public List<Order> orders;
        public OrderService()
        {
            orders = new List<Order>();
        }
        public bool addOrder(Order order)
        {
            foreach (Order o in orders)
            {
                if (o.Equals(order))
                    return false;
            }
            orders.Add(order);
            return true;
        }
        public bool deleteOrder(int id)
        {
            try
            {
                orders.Remove(orders.Where(p => p.ID == id).First());
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool updateOrder(int id, Order neworder)
        {
            try
            {
                int order_number = orders.FindIndex(o => o.ID == id);
                orders[order_number] = neworder;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void sortOrder()
        {
            orders.Sort((o1, o2) => o1.ID - o2.ID);
        }
        public List<Order> searchOrder_byID(int id)
        {
            var query = orders.Where(o => o.ID == id);
            return query.ToList();
        }
        public List<Order> searchOrder_byCustomer(string order_customer)
        {
            var query = orders.Where(o => o.Customer == order_customer).OrderBy(o => o.TotalPrice);
            return query.ToList();
        }
        public List<Order> searchOrder_byTime(DateTime order_time)
        {
            var query = orders.Where(o => o.Time == order_time).OrderBy(o => o.TotalPrice);
            return query.ToList();
        }
        public List<Order> PrintOrders()
        {
            return orders;
        }
        public void Export(string filename)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Order>));
            using (FileStream file = new FileStream(filename + ".xml", FileMode.Create))
                xmlSerializer.Serialize(file, orders);
        }
        public void Import(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Order>));
                List<Order> orderlist = (List<Order>)xmlSerializer.Deserialize(file);
                foreach (Order o in orderlist){
                    addOrder(o);
                }

            }
                
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter A/D/U/S/SID/SC/ST/C/P to select the service:\n" +
                "A: add an order   D: delete an order   U:update an order   S: sort all orders\n" +
                "SID: search orders by order's ID   SC: search orders by customer's name   " +
                "ST: search orders by time\nP: Print all orders.\n" + "E: export in xml   I: import xml\n"+
                "C: close the program");
            OrderService service = new OrderService();
            bool running = true;
            while (running)
            {

                string op;
                op = Console.ReadLine();
                int order_number = -1;
                switch (op)
                {
                    case "A":
                        Console.WriteLine("Enter the order's ID: ");
                        order_number = Int32.Parse(Console.ReadLine());
                        List<OrderItem> items = new List<OrderItem>();
                        Console.WriteLine("Enter the customer's name: ");
                        string customer = Console.ReadLine();
                        string nextitem = "Y";
                        while (nextitem != "N")
                        {
                            string name, quantity, price;
                            Console.WriteLine("Item's name: ");
                            name = Console.ReadLine();
                            Console.WriteLine("Item's quantity: ");
                            quantity = Console.ReadLine();
                            Console.WriteLine("Item's price:");
                            price = Console.ReadLine();
                            items.Add(new OrderItem(name, Int32.Parse(quantity), Double.Parse(price)));
                            Console.WriteLine("Next item? (Y/N): ");
                            nextitem = Console.ReadLine();
                        }
                        DateTime time;
                        Console.WriteLine("Enter the time of this order: ");
                        time = DateTime.Parse(Console.ReadLine());
                        bool iscompleted = false;
                        Console.WriteLine("Is this order completed? (Y/N) ");
                        string ic = Console.ReadLine();
                        if (ic == "Y") iscompleted = true;

                        Order order = new Order(order_number, customer, iscompleted, time, items);
                        if (!service.addOrder(order))
                            Console.WriteLine("Already existed!");
                        else
                            Console.WriteLine("Succesfully created.");
                        break;
                    case "D":
                        Console.WriteLine("Enter the order's ID: ");
                        order_number = Int32.Parse(Console.ReadLine());
                        if (!service.deleteOrder(order_number))
                            Console.WriteLine("No matched order.");
                        else
                            Console.WriteLine("Successfully deleted.");
                        break;
                    case "U":
                        Console.WriteLine("Enter the order's ID: ");
                        order_number = Int32.Parse(Console.ReadLine());
                        List<OrderItem> newitems = new List<OrderItem>();
                        Console.WriteLine("Enter the customer's name: ");
                        string newcustomer = Console.ReadLine();
                        string newnextitem = "Y";
                        while (newnextitem != "N")
                        {
                            string name, quantity, price;
                            Console.WriteLine("Item's name: ");
                            name = Console.ReadLine();
                            Console.WriteLine("Item's quantity: ");
                            quantity = Console.ReadLine();
                            Console.WriteLine("Item's price:");
                            price = Console.ReadLine();
                            newitems.Add(new OrderItem(name, Int32.Parse(quantity), Double.Parse(price)));
                            Console.WriteLine("Next item? (Y/N): ");
                            newnextitem = Console.ReadLine();
                        }
                        DateTime newtime;
                        Console.WriteLine("Enter the time of this order: ");
                        newtime = DateTime.Parse(Console.ReadLine());
                        bool newiscompleted = false;
                        Console.WriteLine("Is this order completed? (Y/N) ");
                        newiscompleted = bool.Parse(Console.ReadLine());

                        Order neworder = new Order(order_number, newcustomer, newiscompleted, newtime, newitems);
                        if (!service.updateOrder(order_number, neworder))
                            Console.WriteLine("No matched order.");

                        else
                            Console.WriteLine("Successfully updated.");
                        break;
                    case "S":
                        service.sortOrder();
                        Console.WriteLine("Sorted.");
                        break;
                    case "SID":
                        Console.WriteLine("Enter the order's ID: ");
                        order_number = Int32.Parse(Console.ReadLine());
                        List<Order> orders_found_byid = service.searchOrder_byID(order_number);
                        if (orders_found_byid.Count == 0) Console.WriteLine("No matched order.");
                        foreach (Order o in orders_found_byid)
                            Console.WriteLine(o.ToString());
                        break;
                    case "SC":
                        Console.WriteLine("Enter the customer's name: ");
                        string order_customer = Console.ReadLine();
                        List<Order> orders_found_bycustomer = service.searchOrder_byCustomer(order_customer);
                        if (orders_found_bycustomer.Count == 0) Console.WriteLine("No matched order.");
                        foreach (Order o in orders_found_bycustomer)
                            Console.WriteLine(o.ToString());
                        break;
                    case "ST":
                        Console.WriteLine("Enter the time: ");
                        DateTime order_time = DateTime.Parse(Console.ReadLine());
                        List<Order> orders_found_bytime = service.searchOrder_byTime(order_time);
                        if (orders_found_bytime.Count == 0) Console.WriteLine("No matched order.");
                        foreach (Order o in orders_found_bytime)
                            Console.WriteLine(o.ToString());
                        break;
                    case "P":
                        foreach (Order o in service.PrintOrders())
                            Console.WriteLine(o.ToString());
                        break;
                    case "C":
                        running = false;
                        break;
                    case "E":
                        Console.WriteLine("Enter filename:");
                        string filename = Console.ReadLine();
                        service.Export(filename);
                        break;
                    case "I":
                        Console.WriteLine("Enter filename:");
                        string filename_in = Console.ReadLine();
                        service.Import(filename_in);
                        break;
                    default:
                        break;

                }


            }
        }
    }
}
