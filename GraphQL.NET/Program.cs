using System;
using System.Threading.Tasks;

namespace GraphQL.NET
{
    class Program
    {
        static async Task Main()
        {
            //await BasicUsage.Run();
            //await BasicSchemaFirstApproach.Run();
            await BasicGraphTypeFirstApproach.Run();
        }
    }
}
