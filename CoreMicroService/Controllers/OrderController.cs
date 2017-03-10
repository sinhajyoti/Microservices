using CoreMicroService.Models;
using CoreMicroService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;
using CoreMicroService.Cache;

namespace CoreMicroService.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        //public IApiRepository ItemsRepo { get; set; }
        private readonly OrderRepository orderRepo;
        private readonly TestSetting _testsetting;
        //private readonly IDistributedCache _cache;
        private readonly ICacheClient _cache;

        //public OrderController(IConfiguration config , IOptions<TestSetting> testsetting, IDistributedCache distributedCache)
        public OrderController(IConfiguration config, IOptions<TestSetting> testsetting, ICacheClient distributedCache)
        {
            //ItemsRepo = itemsRepo;
            orderRepo = new OrderRepository(config);
            _testsetting = testsetting.Value;
            _cache = distributedCache;
        }

        //[Route("/Error")]
        //public IActionResult Index()
        //{
        //    return Content("Error occurred with status code: "+HttpContext.Response.StatusCode.ToString());            // Handle error here
        //}


        [Produces("text/json")]
        [HttpGet]
        public IEnumerable<Order> GetAll()
        {
            //return null;
            //var a = 2;
            //var b = 1;

            //var x = a / (b-1);
            var CACHEKEY = "ALLORDER";
            var orders = _cache.Get<IEnumerable<Order>>(CACHEKEY);//get the data from cache in byte[]
            if (orders == null)//no cached data
            {
                // Get the data to be cached from DB
                orders = orderRepo.FindAll();
                // Store it in cache
                _cache.Set(CACHEKEY, orders, TimeSpan.FromHours(2));
            }
            return orders;
        }

        [HttpGet("{name}", Name = "GetOrderById")]
        public IActionResult GetOne(string name)
        {
            var item = orderRepo.FindByID(name);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);//http 200 - OK
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }
            //item.Value += _testsetting.Message;
            orderRepo.Add(order);
            //return CreatedAtRoute("GetItemById", new { name = item.Name }, item);
            return CreatedAtAction("GetOne", new { name = order.OrderName }, order); // http 201 -- Created

        }

        [HttpPut("{name}",Name ="UpdateOrder")]
        public IActionResult Update(string orderId,[FromBody] Order order)
        {
            if (order == null|| order.OrderId.ToString() != orderId)
            {
                return BadRequest();
            }
            
            var  foundItem= orderRepo.FindByID(order.OrderId.ToString());
            if (foundItem == null)
            {
                return NotFound();
            }
            orderRepo.Update(order);
            return new NoContentResult(); // http 204 -- No content

        }

        [HttpDelete("{name}", Name = "DeleteOrder")]
        public IActionResult Delete(string orderID)
        {
            var foundItem = orderRepo.FindByID(orderID);
            if (foundItem == null)
            {
                return NotFound();
            }
            orderRepo.Remove(orderID);
            return new NoContentResult(); // http 204 -- No content

        }

    }
}
