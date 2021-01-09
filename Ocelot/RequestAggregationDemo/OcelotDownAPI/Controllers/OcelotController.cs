using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OcelotDownAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcelotController : ControllerBase
    {
        // GET api/ocelot/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await Task.Run(() =>
            {
                return $"This is from {HttpContext.Request.Host.Value}, path: {HttpContext.Request.Path}";
            });
            return Ok(result);
        }

        // GET api/ocelot/aggrJay
        [HttpGet("aggrJay")]
        public async Task<IActionResult> AggrJay(int id)
        {
            var result = await Task.Run(() =>
            {
                ResponseResult response = new ResponseResult()
                { Comment = $"From Jay, path: {HttpContext.Request.Path}" };
                return response;
            });
            return Ok(result);
        }

        // GET api/ocelot/aggrKim
        [HttpGet("aggrKim")]
        public async Task<IActionResult> AggrKim(int id)
        {
            var result = await Task.Run(() =>
            {
                ResponseResult response = new ResponseResult()
                { Comment = $"From Kim, path: {HttpContext.Request.Path}" };
                return response;
            });
            return Ok(result);
        }

        // GET api/ocelot/consultJay
        [HttpGet("consultJay")]
        public async Task<IActionResult> ConsultJay(int id)
        {
            var result = await Task.Run(() =>
            {
                ResponseResult response = new ResponseResult()
                { Comment = $"Consult Jay, host: {HttpContext.Request.Host.Value}, path: {HttpContext.Request.Path}, id: {id}" };
                return response;
            });
            return Ok(result);
        }

        // GET api/ocelot/identityJay
        [HttpGet("identityJay")]
        //[Authorize]
        public async Task<IActionResult> IdentityJay(int id)
        {
            var result = await Task.Run(() =>
            {
                ResponseResult response = new ResponseResult()
                { Comment = $"Identify Jay, host: {HttpContext.Request.Host.Value}, path: {HttpContext.Request.Path}, id: {id}" };
                return response;
            });
            return Ok(result);
        }
    }

    public class ResponseResult
    {
        public string Comment { get; set; }
    }
}
