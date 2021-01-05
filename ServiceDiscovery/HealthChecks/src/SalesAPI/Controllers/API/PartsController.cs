using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SalesAPI.Filter;
using SalesAPI.Infrastructure;
using SalesAPI.Models;

namespace SalesAPI.Controllers.API
{
    [Route("api/[controller]")]
    public class PartsController : Controller
    {
        private readonly DataStore _dataStore;

        public PartsController(DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (_dataStore.Parts != null)
                return Ok(_dataStore.Parts);

            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var part = _dataStore.Parts.SingleOrDefault(c => c.ID == id);
            if (part != null)
            {
                return Ok(part);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateModel]
        public IActionResult Post([FromBody]Part part)
        {
            _dataStore.Parts.Add(part);
            return Created(Request.GetDisplayUrl() + "/" + part.ID, part);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Part part)
        {
            var existingPart = _dataStore.Parts.SingleOrDefault(c => c.ID == id);

            if (existingPart == null) return NotFound();

            _dataStore.Parts.Remove(existingPart);
            _dataStore.Parts.Add(part);

            return Ok();
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingPart = _dataStore.Parts.SingleOrDefault(c => c.ID == id);
            
            if (existingPart == null) return NotFound();

            _dataStore.Parts.Remove(existingPart);
            return Ok();
        }
    }
}
