using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*写一个订单管理的控制台程序，能够实现添加订单、删除订单、修改订单、
* 查询订单（按照订单号、商品名称、客户等字段进行查询）功能。
提示：主要的类有Order（订单）、OrderItem（订单明细项），OrderService（订单服务），
订单数据可以保存在OrderService中一个List中。在Program里面可以调用OrderService的方法
完成各种订单操作。
要求：
（1）使用LINQ语言实现各种查询功能，查询结果按照订单总金额排序返回。
（2）在订单删除、修改失败时，能够产生异常并显示给客户错误信息。
（3）作业的订单和订单明细类需要重写Equals方法，确保添加的订单不重复，每个订单的订单明细不重复。
（4）订单、订单明细、客户、货物等类添加ToString方法，用来显示订单信息。
（5）OrderService提供排序方法对保存的订单进行排序。默认按照订单号排序，也可以
使用Lambda表达式进行自定义排序。
*/
namespace ConsoleApp11
{
    class Order
    {
        public int ID { get; set; }
        public string Customer { get; set; }
        private bool IsCompleted { get; set; }
        public DateTime Time { get; set; }
        public double TotalPrice;
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
                TotalPrice += item.Price;
            orderitems = items;
        }
 

        public override string ToString()
        {
            StringBuilder all_items = new StringBuilder();
            foreach (OrderItem oi in orderitems)
            {
                all_items.Append(oi.ToString()+"\n");
            }
            return ID + ": " + Time + "  " + Customer + "\n" + all_items.ToString() + IsCompleted;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Customer, IsCompleted, Time, orderitems);
        }

        public override bool Equals(object obj)
        {
            return obj is Order order &&
                   ID == order.ID &&
                   Customer == order.Customer &&
                   IsCompleted == order.IsCompleted &&
                   Time == order.Time &&
                   EqualityComparer<List<OrderItem>>.Default.Equals(orderitems, order.orderitems);
        }
    }
    class OrderItem
    {
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
            return Name + ": " + Quantity + " * " + Price;
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
    class OrderService
    {
        public List<Order> orders;
        public OrderService()
        {
            orders = new List<Order>();
        }
        public bool addOrder(Order order) {
            foreach (Order o in orders)
            {
                if (o.Equals(order))
                    return false;
            }
            orders.Add(order);
            return true;
        }
        public bool deleteOrder(int id) {
            try
            {
                int order_number = orders.FindIndex(o => o.ID == id);
                orders.RemoveAt(order_number);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool updateOrder(int id, Order neworder) {
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
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter A/D/U/S/SID/SC/ST/C/P to select the service:\n" +
                "A: add an order   D: delete an order   U:update an order   S: sort all orders\n"+
                "SID: search orders by order's ID   SC: search orders by customer's name   " +
                "ST: search orders by time\nP: Print all orders.\n"+
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
                            if(ic == "Y") iscompleted = true;

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
                        default:
                            break;

                    }
                
                
            }
        }
    }
}
