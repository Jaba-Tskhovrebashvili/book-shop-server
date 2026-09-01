using System.ComponentModel.DataAnnotations;

namespace FirstProject.Models
{
    public class AddProduct
    {

        public string Name { get; set; }
        public string Annotation { get; set; }

        public long typeId { get; set; }

        public string ISBN { get; set; }
        public DateTime release_date { get; set; }

        public long publishId { get; set; }

        public int page_quantity { get; set; }

        public string address { get; set; }
        public List<AuthorProd> Authors { get; set; }
    }
}
