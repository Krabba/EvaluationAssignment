namespace PricingService.Services;

using PricingService.Models;
using PricingService.Utils;

public class SubscriptionService
{
    private static List<Subscription> _subscriptions = new List<Subscription>();

    public Subscription CreateSubscription(int customerId, int serviceId, decimal serviceDiscount,
        List<DiscountPeriod> discountPeriods,
        DateTime startDate, DateTime endDate)
    {
        var service = ServiceService.GetServiceById(serviceId);
        service.PricePerDay = Math.Max(service.PricePerDay - serviceDiscount, 0);

        var subscription = new Subscription(customerId, service, discountPeriods, startDate, endDate);
        _subscriptions.Add(subscription);
        return subscription;
    }

    public Subscription? GetSubscriptionById(int subscriptionId)
    {
        var subscription = _subscriptions.FirstOrDefault(s => s.Id == subscriptionId);
        return subscription;
    }

    public List<Subscription> GetSubscriptions(int? customerId = null, DateTime? startDate = null,
        DateTime? endDate = null)
    {
        if (!customerId.HasValue && !startDate.HasValue && !endDate.HasValue)
        {
            return _subscriptions;
        }

        if (customerId.HasValue && !startDate.HasValue && !endDate.HasValue)
        {
            return _subscriptions.Where(s => s.CustomerId == customerId).ToList();
        }

        var filteredSubscriptions = _subscriptions.Where(s =>
            (s.CustomerId == customerId) &&
            (s.StartDate <= endDate) &&
            (s.EndDate >= startDate)
        ).ToList();

        return filteredSubscriptions;
    }

    public SubscriptionPriceDetails GetPriceForSubscriptionPeriod(Customer customer, DateTime startDate,
        DateTime endDate)
    {
        var subscriptions = GetSubscriptions(customer.Id, startDate, endDate);

        var price = 0m;
        var discountAmount = 0m;

        foreach (var subscription in subscriptions)
        {
            var pricePerDay = subscription.Service.PricePerDay;
            var weekendsOff = subscription.Service.WeekendsOff;
            var overlapStartDate = subscription.StartDate > startDate ? subscription.StartDate : startDate;
            var overlapEndDate = subscription.EndDate < endDate ? subscription.EndDate : endDate;

            if (overlapStartDate < overlapEndDate)
            {
                int overlapDays = (overlapEndDate - overlapStartDate).Days + 1;

                if (weekendsOff)
                {
                    var weekendDays = Dates.GetWeekendDayCount(overlapStartDate, overlapEndDate);
                    overlapDays -= weekendDays;
                }

                decimal priceExcludingDiscount = overlapDays * pricePerDay;
                price += priceExcludingDiscount;

                foreach (var discountPeriod in subscription.DiscountPeriods)
                {
                    var discountOverlapStartDate =
                        discountPeriod.StartDate > startDate ? discountPeriod.StartDate : startDate;
                    var discountOverlapEndDate = discountPeriod.EndDate < endDate ? discountPeriod.EndDate : endDate;

                    if (discountOverlapStartDate < discountOverlapEndDate)
                    {
                        int discountOverlapDays = (discountOverlapEndDate - discountOverlapStartDate).Days + 1;
                        var discountPercentage = (decimal)discountPeriod.Discount;

                        if (weekendsOff)
                        {
                            var discountWeekendDays =
                                Dates.GetWeekendDayCount(discountOverlapStartDate, discountOverlapEndDate);
                            discountOverlapDays -= discountWeekendDays;
                        }

                        decimal discount = discountPercentage > 0 && discountPercentage < 1
                            ? discountOverlapDays * pricePerDay * discountPercentage
                            : discountOverlapDays * pricePerDay;

                        discountAmount += discount;
                    }
                }
            }
        }

        var discountedPrice = price - discountAmount;

        return new SubscriptionPriceDetails
        {
            CustomerId = customer.Id,
            Price = price,
            DiscountedPrice = discountedPrice,
            DiscountAmount = discountAmount,
            Subscriptions = subscriptions
        };
    }
}