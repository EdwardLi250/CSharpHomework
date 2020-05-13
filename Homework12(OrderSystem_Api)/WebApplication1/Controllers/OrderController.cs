using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly OrderContext orderContext;

        public OrderController(OrderContext orderContext)
        {
            this.orderContext = orderContext;
        }

        private IQueryable<Order> AllOrders()
        {
            return orderContext.Orders.Include("Orderitems");
        }

        //GET: api/order
        [HttpGet]
        public ActionResult<List<Order>> GetAllOrders()
        {
            var orders = AllOrders();
            return orders.ToList();
        }

        //GET: api/order/ini
        [HttpGet("ini")]
        public ActionResult<List<Order>> Initialize()
        {
            List<OrderItem> order1_items = new List<OrderItem>();
            List<OrderItem> order2_items = new List<OrderItem>();
            OrderItem order1_item1 = new OrderItem("Blonde", 3, 50);
            OrderItem order1_item2 = new OrderItem("Orange", 2, 35);
            OrderItem order2_item1 = new OrderItem("IGOR", 5, 50);
            order1_items.Add(order1_item1); order1_items.Add(order1_item2);
            order2_items.Add(order2_item1);
            Order order1 = new Order(1, "FO", true, DateTime.Parse("2020/02/01 11:22:33"), order1_items);
            Order order2 = new Order(2, "TC", true, DateTime.Parse("2020/02/01 11:22:33"), order2_items);

            orderContext.Orders.Add(order1);
            orderContext.Orders.Add(order2);
            orderContext.SaveChanges();
            //return Ok(AllOrders().ToList());
            return GetAllOrders();
        }

        //POST: api/order
        [HttpPost]
        public ActionResult<Order> AddOrder(Order order)
        {
            try
            {
                orderContext.Orders.Add(order);
                orderContext.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return order;
        }

        //GET: api/order/{id}
        [HttpGet("{id}")]
        public ActionResult<List<Order>> GetOrderByID(int id)
        {
            var query = orderContext.Orders.Include("Orderitems").Where(p => p.OrderID == id);
            //var query = Orders.Where(o => o.OrderID == id);
            return query.ToList();
        }

        //GET: api/order/time?=
        [HttpGet("time")]
        public ActionResult<List<Order>> GetOrderByTime(DateTime order_time)
        {
            
            var query = orderContext.Orders.Include("Orderitems").Where(p => p.Time == order_time).OrderBy(o => o.TotalPrice);
            //var query = Orders.Where(o => o.Time == order_time).OrderBy(o => o.TotalPrice);
            return query.ToList();
        }

        //GET: api/order/customer?name=
        [HttpGet("customer")]
        public ActionResult<List<Order>> GetOrderByCustomer(string order_customer)
        {
            var query = orderContext.Orders.Include("Orderitems").Where(p => p.Customer == order_customer).OrderBy(o => o.TotalPrice);
            //var query = Orders.Where(o => o.Customer == order_customer).OrderBy(o => o.TotalPrice);
            return query.ToList();
        }

        //DELETE: api.order/{id}
        [HttpDelete("{id}")]
        public ActionResult<Order> DeleteOrder(int id)
        {
            try
            {
                //Orders.Remove(Orders.Where(p => p.OrderID == id).First());
                var order = orderContext.Orders.Include("Orderitems").Where(p => p.OrderID == id).First();
                orderContext.Orders.Remove(order);
                //context.Orders.Remove(Orders.Where(p => p.OrderID == id).First());
                orderContext.SaveChanges();
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }
    }
}





    
