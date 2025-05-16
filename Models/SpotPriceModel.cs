using System.ComponentModel.DataAnnotations;

namespace SpotPriceBridge.Models
{
    public class SpotPriceModel
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal AskPrice { get; set; }
        public DateTime UpdatedOnUtc { get; set; }
    }
}
