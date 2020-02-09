using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.NET
{
    public static class BasicSchemaFirstApproach
    {


        public class Droid
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class Query
        {
            [GraphQLMetadata("hero")]
            public Droid GetHero()
            {
                return new Droid { Id = "1", Name = "R2-D2" };
            }
        }

        public static async Task Run()
        {
            var schema = Schema.For(@"
              type Droid {
                id: String!
                name: String!
              }

              type Query {
                hero: Droid
              }
            ", _ =>
            {
                _.Types.Include<Query>();
            });

            var json = await schema.ExecuteAsync(_ =>
            {
                // The following line specifies what's going to be asked to the API
                _.Query = "{ hero { id name } }";
            });

            Console.WriteLine(json);
        }
    }
}
