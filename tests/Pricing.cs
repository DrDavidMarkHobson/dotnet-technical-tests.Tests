namespace dotnettechnicaltest.Tests.tests
{
    public class Pricing : IPricing
    {
        public decimal Checkout(int[] books)
        {
            return books.Length * 8;
        }
    }
}