namespace Gijgo.AspNetCore.Models.DTO
{
    public class GetPlayersDto
    {
        public int? page;
        public int? limit;
        public string sortBy;
        public string direction;
        public string name;
        public string nationality;
        public string placeOfBirth;
    }
}
