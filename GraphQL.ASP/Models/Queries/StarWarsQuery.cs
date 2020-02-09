using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.ASP.Models.Queries
{
    public class Droid
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DroidType : ObjectGraphType<Droid>
    {
        public DroidType()
        {
            Field(x => x.Id).Description($"The {nameof(Droid.Id)} of the {nameof(Droid)}");
            Field(x => x.Name).Description($"The {nameof(Droid.Name)} of the {nameof(Droid)}");
        }
    }

    public class StarWarsQuery : ObjectGraphType
    {
        public StarWarsQuery()
        {
            Field<DroidType>(
                "hero",
                resolve: context =>
                {
                    // The filtering and stuff can be processed here
                    return new Droid { Id = 1, Name = "R2-D2" };
                }
            );
        }
    }
}
