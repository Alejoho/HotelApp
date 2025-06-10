namespace HotelApp.Web;

public static class Helpers
{
    public static string CalculateTotalCost(decimal price, DateTime startDate, DateTime endDate)
    {
        int days = (endDate - startDate).Days;
        return $"{(days * price):C}";
    }
}