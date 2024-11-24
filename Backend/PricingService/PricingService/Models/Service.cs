namespace PricingService.Models;

public class Service
{
    public int Id { get; set; }
    public ServiceClass ServiceClass { get; set; }
    public decimal PricePerDay { get; set; }
    public bool WeekendsOff { get; set; }

    public Service(int id, ServiceClass serviceClass, decimal pricePerDay, bool weekendsOff)
    {
        Id = id;
        ServiceClass = serviceClass;
        PricePerDay = pricePerDay;
        WeekendsOff = weekendsOff;
    }
}

public enum ServiceClass
{
    A,
    B,
    C
}