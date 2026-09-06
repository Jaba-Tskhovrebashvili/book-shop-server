namespace FirstProject.Models
{
    public class AuthorDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public long? SexId { get; set; }
        public long? CountryId { get; set; }

        public long? CityId { get; set; }
    }
}
