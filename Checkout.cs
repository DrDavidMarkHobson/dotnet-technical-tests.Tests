using System.Collections.Generic;

namespace dotnet_technical_test.Tests
{
    public class Checkout
    {
        public Checkout(List<Book> customerBasket)
        {
            CustomerBasket = customerBasket;
            CheckedOut = new List<Book>();
            RunningTotal = 0;
        }
        public List<Book> CustomerBasket { get; }
        public List<Book> CheckedOut;
        public decimal RunningTotal { get; set; }
    }
}