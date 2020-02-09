using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.ASP.Models.Queries;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GraphQL.ASP.Controllers
{
    [Route("[controller]")]
    public class GraphQLController : Controller
    {
        [HttpPost]
        public async Task Query([FromBody] IDictionary<string, string> query)
        {
            var schema = new Schema { Query = new StarWarsQuery() };
            var json = await schema.ExecuteAsync(options =>
            {
                string operationName;
                options.OperationName = query.TryGetValue("operationName", out operationName) ? operationName : "";
                options.Query = query["query"];
            });
            var jsonBytes = Encoding.UTF8.GetBytes(json);
            HttpContext.Response.StatusCode = 200;
            HttpContext.Response.ContentType = "application/json";
            HttpContext.Response.ContentLength = jsonBytes.Length;
            var result = await HttpContext.Response.BodyWriter.WriteAsync(jsonBytes);
            await HttpContext.Response.CompleteAsync();
        }
    }
}
