using Microsoft.AspNetCore.Mvc;
using SpotPriceBridge.Data;
using SpotPriceBridge.Models;
using System.Linq;

namespace SpotPriceBridge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpotPriceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SpotPriceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Root endpoint for the API (handles '/')
        [HttpGet("/")]
        public IActionResult GetRootAndSpotPrices()
        {
            var spotPrices = _context.SpotPrice.ToList();

            var response = new
            {
                Message = "Welcome to the Spot Price API!",
                SpotPrices = spotPrices
            };

            return Ok(response);
        }
    }
}
