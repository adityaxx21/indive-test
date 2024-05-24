using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace indive_test.Controllers
{

    [Route("api/dashboard")]
    [ApiController]

    public class DummyController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            return Ok("Authorized");
        }
    }
}