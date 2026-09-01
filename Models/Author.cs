using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FirstProject.Models
{
    public class Author
    {
        [Key]
        public Int64 Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Surname { get; set; }
        [Required]
        public long SexId { get; set; }

        public Author_Sex Sex { get; set; }
        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "პირადი ნომერი უნდა შედგებოდეს 11 ციფრისგან")]
        public string PersonalNumber { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public long CountryId { get; set; }
        public Country? Country { get; set; }

        [Required]
        public long CityId { get; set; }

        public City? City { get; set; }

        [MinLength(4)]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime? Created_At { get; set; }
        public DateTime? Updated_At { get; set; }

        public ICollection<Product> Products { get; set; }=new List<Product>();


    }
}
