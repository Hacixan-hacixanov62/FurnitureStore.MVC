namespace MVC_FinalProject.Models.Basket
{
    public class BasketUpdate
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int OldQuantity { get; set; }
        public int NewQuantity { get; set; }
    }
}
