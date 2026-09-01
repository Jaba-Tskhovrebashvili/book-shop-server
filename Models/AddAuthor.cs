using System.ComponentModel.DataAnnotations;

namespace FirstProject.Models
{
    public class AddAuthor
    {
        public string Name { get; set; }

        public string Surname { get; set; }
        public long SexId { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime BirthDate { get; set; }

        public long CountryId { get; set; }

        public long CityId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
