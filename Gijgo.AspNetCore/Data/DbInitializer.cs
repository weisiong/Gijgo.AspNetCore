using Gijgo.Asp.NetCore.Data;

namespace Gijgo.Asp.NetCore._2._2.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
