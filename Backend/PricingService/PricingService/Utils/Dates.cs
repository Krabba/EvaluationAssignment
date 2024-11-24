namespace PricingService.Utils;

public class Dates
{
    public static int GetWeekendDayCount(DateTime start, DateTime end)
    {
        return Enumerable
            .Range(0, (end - start).Days + 1)
            .Count(date =>
                start.AddDays(date).DayOfWeek == DayOfWeek.Saturday ||
                start.AddDays(date).DayOfWeek == DayOfWeek.Sunday);
    }
}