using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleApp11;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ConsoleApp11.Tests
{
    [TestClass()]
    public class OrderServiceTests
    {
        
        public List<OrderItem> items1 = new List<OrderItem>
        {
            new OrderItem("thing1", 3, 5),
            new OrderItem("thing2", 2, 10),
            new OrderItem("thing3", 23, 111)
        };
        public List<OrderItem> items2 = new List<OrderItem> {
            new OrderItem("thing4", 3, 5),
            new OrderItem("thing5", 21, 13),
            new OrderItem("thing2", 2, 10)
        };

        [TestMethod()]
        public void addOrderTest1()
        {
            OrderService service = new OrderService();
            Order order1 = new Order(1, "BH", true, DateTime.Parse("2020/01/01 11:11:11"), items1);
            Order order2 = new Order(2, "FO", true, DateTime.Parse("2020/02/02 11:11:11"), items2);
            service.addOrder(order1);
            service.addOrder(order2);
            Assert.AreEqual(service.orders[0], order1);
        }
        [TestMethod()]
        public void addOrderTest2()
        {
            OrderService service = new OrderService();
            Order order1 = new Order(1, "BH", true, DateTime.Parse("2020/01/01 11:11:11"), items1);
            Order order2 = new Order(1, "FO", true, DateTime.Parse("2020/02/02 11:11:11"), items2);
            service.addOrder(order1);
            Assert.IsFalse(service.addOrder(order2));
        }
        [TestMethod()]
        public void deleteOrderTest1()
        {
            OrderService service = new OrderService();
            Order order1 = new Order(1, "BH", true, DateTime.Parse("2020/01/01 11:11:11"), items1);
            service.addOrder(order1);
            service.deleteOrder(1);
            Assert.AreEqual(service.searchOrder_byID(1).Count,0);
        }
        [TestMethod()]
        public void deleteOrderTest2()
        {
            OrderService service = new OrderService();
            Order order1 = new Order(1, "BH", true, DateTime.Parse("2020/01/01 11:11:11"), items1);
            service.addOrder(order1);
            Assert.IsFalse(service.deleteOrder(2));
        }

        [TestMethod()]
        public void updateOrderTest()
        {
            OrderService service = new OrderService();
            Order order1 = new Order(1, "BH", true, DateTime.Parse("2020/01/01 11:11:11"), items1);
            service.addOrder(order1);
            Order neworder = new Order(1, "TC", true, DateTime.Parse("2020/01/01 22:22:22"), items2);
            service.updateOrder(1, neworder);
            Assert.AreEqual(service.orders[0], neworder);
        }

        [TestMethod()]
        public void sortOrderTest()
        {
            OrderService service = new OrderService();
            Order order1 = new Order(1, "BH", true, DateTime.Parse("2020/01/01 11:11:11"), items1);
            Order order2 = new Order(2, "FO", true, DateTime.Parse("2020/02/02 11:11:11"), items2);
            service.addOrder(order2);
            service.addOrder(order1);
            service.sortOrder();
            Assert.AreEqual(service.orders[0], order1);
        }

        [TestMethod()]
        public void searchOrder_byIDTest1()
        {
            OrderService service = new OrderService();
            Order order1 = new Order(1, "BH", true, DateTime.Parse("2020/01/01 11:11:11"), items1);
            Order order2 = new Order(2, "FO", true, DateTime.Parse("2020/02/02 11:11:11"), items2);
            service.addOrder(order1);
            service.addOrder(order2);          
            Assert.AreEqual(service.searchOrder_byID(2)[0], order2);
        }
        public void searchOrder_byIDTest2()
        {
            OrderService service = new OrderService();
            Order order1 = new Order(1, "BH", true, DateTime.Parse("2020/01/01 11:11:11"), items1);
            Order order2 = new Order(2, "FO", true, DateTime.Parse("2020/02/02 11:11:11"), items2);
            service.addOrder(order1);
            service.addOrder(order2);
            Assert.AreEqual(service.searchOrder_byID(3).Count, 0);
        }

        [TestMethod()]
        public void searchOrder_byCustomerTest()
        {
            OrderService service = new OrderService();
            Order order1 = new Order(1, "BH", true, DateTime.Parse("2020/01/01 11:11:11"), items1);
            Order order2 = new Order(2, "FO", true, DateTime.Parse("2020/02/02 11:11:11"), items2);
            service.addOrder(order1);
            service.addOrder(order2);
            Assert.AreEqual(service.searchOrder_byCustomer("FO")[0], order2);
        }

        [TestMethod()]
        public void searchOrder_byTimeTest()
        {
            OrderService service = new OrderService();
            Order order1 = new Order(1, "BH", true, DateTime.Parse("2020/01/01 11:11:11"), items1);
            Order order2 = new Order(2, "FO", true, DateTime.Parse("2020/02/02 11:11:11"), items2);
            service.addOrder(order1);
            service.addOrder(order2);
            Assert.AreEqual(service.searchOrder_byTime(DateTime.Parse("2020/01/01 11:11:11"))[0], order1);
        }

        [TestMethod()]
        public void ExportTest()
        {
            OrderService service = new OrderService();
            Order order1 = new Order(1, "BH", true, DateTime.Parse("2020/01/01 11:11:11"), items1);
            Order order2 = new Order(2, "FO", true, DateTime.Parse("2020/02/02 11:11:11"), items2);
            service.addOrder(order1);
            service.addOrder(order2);
            service.Export("exporttest");
            Assert.IsTrue(File.Exists("exporttest.xml"));
        }

        [TestMethod()]
        public void ImportTest()
        {
            OrderService service = new OrderService();
            Order order1 = new Order(1, "BH", true, DateTime.Parse("2020/01/01 11:11:11"), items1);
            Order order2 = new Order(2, "FO", true, DateTime.Parse("2020/02/02 11:11:11"), items2);
            service.addOrder(order1);
            service.addOrder(order2);
            service.Export("importtest");
            OrderService service_test = new OrderService();
            service_test.Import("importtest.xml");
            Assert.IsTrue(service.orders[0].Equals(service_test.orders[0]) &&
                          service.orders[1].Equals(service_test.orders[1]));
        }
    }
}