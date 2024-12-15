namespace UniqloMVC.Models
{
    public class Comment:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
