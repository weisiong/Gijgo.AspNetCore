
namespace Gijgo.AspNetCore.Models.DTO
{
    public class LocationPlayers
    {
        public int id { get; set; }

        public string text { get; set; }

        public bool @checked { get; set; }

        public bool hasChildren { get; set; }

        public dynamic children { get; set; }

        public string type { get; set; }
    }
}
