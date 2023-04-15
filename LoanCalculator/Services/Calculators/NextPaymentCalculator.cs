namespace LoanCalculator.Services.Calculators;

public static class NextPaymentCalculator
{
    public static DateTime GetNextPaymentDate(this DateTime currentDate, int paymentDay)
    {
        DateTime nextPaymentDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1);

        if (paymentDay > DateTime.DaysInMonth(nextPaymentDate.Year, nextPaymentDate.Month))
        {
            paymentDay = DateTime.DaysInMonth(nextPaymentDate.Year, nextPaymentDate.Month);
        }

        return nextPaymentDate.AddDays(paymentDay - 1);
    }
}