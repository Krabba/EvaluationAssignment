using PricingService.Utils;
using Random = PricingService.Utils.Random;

namespace PricingService.Models;

public class Customer
{
    private static int _id = 0;
    public int Id { get; set; }
    public string Email { get; set; }

    public Customer()
    {
        Id = ++_id;
        Email = Random.GenerateRandomEmail();
    }
}