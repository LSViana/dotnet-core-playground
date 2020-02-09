using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.NET
{
    public static class BasicUsage
    {
        public static async Task Run()
        {
            var schema = Schema.For(@"
                type Query {
                    message: String
                }
            ");
            var json = await schema.ExecuteAsync(_ =>
            {
                _.Query = "{ message }";
                _.Root = new { Message = "GraphQL.NET Query" };
            });
            Console.WriteLine(json);
        }
    }
}
