using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SalesAPI.Filter;
using SalesAPI.Infrastructure;
using SalesAPI.Models;

namespace SalesAPI.Controllers.API
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly DataStore _dataStore;

        public OrdersController(DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (_dataStore.Orders != null)
                return Ok(_dataStore.Orders);

            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var order = _dataStore.Orders.SingleOrDefault(c => c.ID == id);
            if (order != null)
            {
                return Ok(order);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateModel]
        public IActionResult Post([FromBody]Order student)
        {
            _dataStore.Orders.Add(student);
            return Created(Request.GetDisplayUrl() + "/" + student.ID, student);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Order order)
        {
            var existingOrder = _dataStore.Orders.SingleOrDefault(c => c.ID == id);

            if (existingOrder == null) return NotFound();

            _dataStore.Orders.Remove(existingOrder);
            _dataStore.Orders.Add(order);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingOrder = _dataStore.Orders.SingleOrDefault(c => c.ID == id);

            if (existingOrder == null) return NotFound();

            _dataStore.Orders.Remove(existingOrder);
            return Ok();
        }
    }
}
