using System.ComponentModel.DataAnnotations;

namespace SpotPriceBridge.Models
{
    public class SpotPriceModel
    {
        [Key]
        public int Id { get; set; }
        public required string Code { get; set; }
        public decimal AskPrice { get; set; }
    }
}
