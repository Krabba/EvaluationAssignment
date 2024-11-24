namespace PricingService.Services;

using PricingService.Models;

public class CustomerService
{
    private static List<Customer> _customers = new List<Customer>();

    public Customer CreateCustomer()
    {
        var customer = new Customer();
        _customers.Add(customer);
        return customer;
    }

    public Customer? GetCustomerById(int customerId)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == customerId);
        return customer;
    }

    public List<Customer> GetCustomers()
    {
        return _customers;
    }
}