using System.ComponentModel.DataAnnotations;

namespace FirstProject.Models
{
    public class Product
    {
        [Key]
        public Int64 Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MinLength(100)]
        [MaxLength(500)]
        public string Annotation { get; set; }

        [Required]
        public long typeId { get; set; }
        public Product_Type Product_Type { get; set; }

        [Required]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "ISBN უნდა შედგებოდეს 13 ციფრისგან")]
        public string ISBN { get; set; }
        [Required]
        public DateTime release_date { get; set; }

        [Required]
        public long publishId {  get; set; }
        public Publishing_house publishing_house { get; set; }

        [Required]
        public int page_quantity { get; set; }

        [Required]
        public string address { get; set; }

        public DateTime? Created_At { get; set; }
        public DateTime? Updated_At { get; set; }

        public ICollection<Author> Authors { get; set; }=new List<Author>();

    }
}
