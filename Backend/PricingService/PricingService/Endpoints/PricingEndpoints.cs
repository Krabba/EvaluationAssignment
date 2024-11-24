namespace PricingService.Endpoints;

using PricingService.Services;

public static class PricingEndpoints
{
    public static void MapPricingEndpoints(this WebApplication app)
    {
        app.MapGet("/pricing", GetPriceForSubscriptionPeriod);
    }

    private static IResult GetPriceForSubscriptionPeriod(int? customerId, string? startDate, string? endDate,
        CustomerService customerService,
        SubscriptionService subscriptionService)
    {
        if (!customerId.HasValue || startDate == null || endDate == null)
        {
            return Results.BadRequest(new { message = "CustomerId or StartDate/EndDate cannot be null." });
        }

        var customer = customerService.GetCustomerById(customerId.Value);

        if (customer == null)
        {
            return Results.NotFound(new { message = "Customer not found." });
        }

        var _startDate = DateTime.Parse(startDate);
        var _endDate = DateTime.Parse(endDate);

        var details =
            subscriptionService.GetPriceForSubscriptionPeriod(customer, _startDate, _endDate);

        return Results.Ok(details);
    }
}