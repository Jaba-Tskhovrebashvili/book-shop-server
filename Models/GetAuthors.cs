namespace FirstProject.Models
{
    public class GetAuthors
    {
        public List<AuthorDto> Authors { get; set; }
        public int TotalPages { get; set;}
    }
}
