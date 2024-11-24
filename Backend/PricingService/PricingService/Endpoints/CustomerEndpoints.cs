namespace PricingService.Endpoints;

using PricingService.Services;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        app.MapPost("/customers", CreateCustomer);
        app.MapGet("/customers", GetCustomers);
        app.MapGet("/customers/{id}", GetCustomer);
    }

    private static IResult CreateCustomer(CustomerService customerService)
    {
        var customer = customerService.CreateCustomer();
        return Results.Created("/customers", customer);
    }

    private static IResult GetCustomers(CustomerService customerService)
    {
        var customers = customerService.GetCustomers();
        return Results.Ok(customers);
    }

    private static IResult GetCustomer(int id, CustomerService customerService)
    {
        var customer = customerService.GetCustomerById(id);
        return customer != null ? Results.Ok(customer) : Results.NotFound(new { message = "Customer not found." });
    }
}