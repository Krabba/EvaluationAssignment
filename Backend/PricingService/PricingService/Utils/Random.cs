using System;
using System.Text;
using PricingService.Models;

namespace PricingService.Utils;

public class Random
{
    private static System.Random _random = new System.Random();

    public static string GenerateRandomEmail()
    {
        string username = GenerateRandomString(8);
        string domain = GenerateRandomString(5) + ".com";

        return username + "@" + domain;
    }

    public static ServiceClass GenerateRandomServiceClass()
    {
        Array values = Enum.GetValues(typeof(ServiceClass));
        return (ServiceClass)values.GetValue(_random.Next(values.Length))!;
    }

    private static string GenerateRandomString(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            result.Append(chars[_random.Next(chars.Length)]);
        }

        return result.ToString();
    }
}