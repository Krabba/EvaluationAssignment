namespace PricingService.Endpoints;

using PricingService.Services;
using PricingService.Models;

public static class SubscriptionEndpoints
{
    public static void MapSubscriptionEndpoints(this WebApplication app)
    {
        app.MapPost("/subscriptions", CreateSubscription);
        app.MapGet("/subscriptions", GetSubscriptions);
        app.MapGet("/subscriptions/{id}", GetSubscription);
    }

    private static IResult CreateSubscription(CreateSubscriptionRequestBody request,
        CustomerService customerService,
        SubscriptionService subscriptionService)

    {
        if (!request.CustomerId.HasValue)
        {
            return Results.BadRequest(new { message = "Please provide a customer ID." });
        }

        if (request.Subscriptions.Count == 0)
        {
            return Results.BadRequest(new { message = "Please provide at least one subscription." });
        }

        var customer = customerService.GetCustomerById(request.CustomerId.Value);

        if (customer == null)
        {
            return Results.NotFound(new { message = "Customer not found." });
        }

        var subscriptions = new List<Subscription>();

        foreach (var s in request.Subscriptions)
        {
            var startDate = DateTime.Parse(s.StartDate);
            var endDate = DateTime.Parse(s.EndDate);
            var subscription = subscriptionService.CreateSubscription(customer.Id, s.ServiceId, s.ServiceDiscount,
                s.DiscountPeriods, startDate, endDate);

            subscriptions.Add(subscription);
        }

        if (subscriptions.Count == 0)
        {
            return Results.BadRequest(new { message = "No subscriptions created." });
        }

        return Results.Created("/subscriptions", subscriptions);
    }

    private static IResult GetSubscriptions(SubscriptionService subscriptionService)
    {
        return Results.Ok(subscriptionService.GetSubscriptions());
    }

    private static IResult GetSubscription(int id, SubscriptionService subscriptionService)
    {
        var subscription = subscriptionService.GetSubscriptionById(id);
        return subscription != null
            ? Results.Ok(subscription)
            : Results.NotFound(new { message = "Subscription not found." });
    }
}

public class CreateSubscriptionRequestBody
{
    public int? CustomerId { get; set; }
    public List<SubscriptionDetails> Subscriptions { get; set; } = new List<SubscriptionDetails>();
}

public class SubscriptionDetails
{
    public int ServiceId { get; set; }
    public decimal ServiceDiscount { get; set; } = 0;
    public List<DiscountPeriod> DiscountPeriods { get; set; } = new List<DiscountPeriod>();
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
}