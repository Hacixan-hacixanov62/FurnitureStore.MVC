namespace MVC_FinalProject.Models.Order
{
    public class Order
    {
        public int Id { get; set; }
        public string AppUserEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
