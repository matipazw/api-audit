
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using middleware.api.audit;
using api.audit.web.example.Models;

namespace api.audit.web.example.Controllers
{
	[Route("api/[controller]")]
    public class EventsController : Controller
    {
        // GET api/values
        [HttpGet]

		[Audit( ExcludeFields = "test")]
		public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST api/values
        [HttpPost]
        [Audit(IdPropertyName = "Id")]
        public void Post([FromBody] Event value)
        { }
    }
}
