namespace PricingService.Services;

using PricingService.Models;

public class ServiceService
{
    public static Service GetServiceById(int serviceId)
    {
        Service service = serviceId switch
        {
            1 => new Service(1, ServiceClass.A, 0.2m, true),
            2 => new Service(2, ServiceClass.B, 0.24m, true),
            3 => new Service(3, ServiceClass.C, 0.4m, false),
            _ => throw new Exception($"Unknown service ID: {serviceId}")
        };

        return service;
    }
}