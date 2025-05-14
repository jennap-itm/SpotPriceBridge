using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using SpotPriceBridge.Models;
using System.Data;

[ApiController]
[Route("api/[controller]")]
public class SpotPriceController : ControllerBase
{
    private readonly IConfiguration _config;

    public SpotPriceController(IConfiguration config)
    {
        _config = config;
    }

    // Root endpoint for the API (handles '/')
    [HttpGet("/")]
    public IActionResult GetRootAndSpotPrices()
    {
        var spotPrices = new List<SpotPriceModel>();

        // Fetching spot prices from the database
        using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT Code, AskPrice FROM NewSpotPrice", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                spotPrices.Add(new SpotPriceModel
                {
                    Code = reader.GetString(reader.GetOrdinal("Code")),
                    AskPrice = reader.GetDecimal(reader.GetOrdinal("AskPrice")),
                });
            }
        }

        // Create a response object that contains both the welcome message and the spot prices
        var response = new
        {
            Message = "Welcome to the Spot Price API!",
            SpotPrices = spotPrices
        };

        return Ok(response);
    }
}
