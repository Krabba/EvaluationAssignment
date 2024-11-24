namespace PricingService.Models;

public class Subscription
{
    private static int _id = 0;
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Service Service { get; set; }
    public List<DiscountPeriod> DiscountPeriods { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public Subscription(int customerId, Service service, List<DiscountPeriod> discountPeriods, DateTime startDate,
        DateTime endDate)
    {
        Id = ++_id;
        CustomerId = customerId;
        Service = service;
        DiscountPeriods = discountPeriods;
        StartDate = startDate;
        EndDate = endDate;
    }
}

public class DiscountPeriod
{
    public float Discount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class SubscriptionPriceDetails
{
    public int CustomerId { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountedPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}