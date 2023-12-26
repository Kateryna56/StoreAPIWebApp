namespace StoreAPIWebApp.Models
{
    public class Product
    {
        public Product()
        {
            Orders = new List<Order>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
