using System.Collections.Generic;
using System.Linq;

namespace dotnet_technical_test.Tests
{
    public class DiscountHandler : IHandleDiscounts
    {
        private readonly decimal NormalPrice = 8;
        private decimal FivePercentDiscount (decimal price)=> 
            price - (price * (decimal)0.05);
        private decimal TenPercentDiscount(decimal price) =>
            price - (price * (decimal)0.1);
        private decimal TwentyPercentDiscount(decimal price) =>
            price - (price * (decimal)0.2);
        private decimal TwentyFivePercentDiscount(decimal price) =>
            price - (price * (decimal)0.25);
        public Checkout DiscountFromBooks(Checkout customerCheckout)
        {
            //first rule to apply:
            //Note that if you buy, say, four books, of which 3 are different titles, 
            //you get a 10% discount on the 3 that form part of a set, but the fourth book still costs the original price.

            if (customerCheckout.CustomerBasket.Any()
                && customerCheckout.CustomerBasket.Count() > 3)
            {
                List<Book> booksToRemove = new List<Book>();
                var distinctBooks = customerCheckout.CustomerBasket.Distinct().ToList();
                var twoOfTheSameBook = distinctBooks.Where(
                    book =>
                    {
                        var duplicatesOfThisBook = 
                            customerCheckout.CustomerBasket.Where(
                                differentBook => book.Name == differentBook.Name)
                                .ToList();
                        return duplicatesOfThisBook.Count() > 1;
                    }).Distinct().ToList();
                if (distinctBooks.Count() > 3 && twoOfTheSameBook.Any())
                {

                    booksToRemove.AddRange(distinctBooks.Take(3));
                    booksToRemove.ForEach(book =>
                    {
                        customerCheckout.CustomerBasket.Remove(book);
                        customerCheckout.CheckedOut.Add(book);
                        customerCheckout.RunningTotal += TenPercentDiscount(NormalPrice);
                    });
                    var currentDistinctBook = twoOfTheSameBook.Take(1).First();
                    booksToRemove.Add(currentDistinctBook);
                        customerCheckout.CustomerBasket.Remove(currentDistinctBook);
                        customerCheckout.CheckedOut.Add(currentDistinctBook);
                        customerCheckout.RunningTotal += NormalPrice;
                }
            }

            //second rule to apply:        
            //If you go the whole hog, and buy all 5, you get a huge 25% discount.

            if (customerCheckout.CustomerBasket.Any()
                && customerCheckout.CustomerBasket.Count() > 4)
            {
                List<Book> booksToRemove = new List<Book>();
                var differentBooks = customerCheckout.CustomerBasket.Distinct();
                if (differentBooks.Count() > 4)
                {
                    booksToRemove.AddRange(differentBooks);
                    booksToRemove.ForEach(book =>
                    {
                        customerCheckout.CustomerBasket.Remove(book);
                        customerCheckout.CheckedOut.Add(book);
                        customerCheckout.RunningTotal += TwentyFivePercentDiscount(NormalPrice);
                    });
                }
            }

            //third:
            //If you buy 4 different books, you get a 20% discount.

            if (customerCheckout.CustomerBasket.Any()
                && customerCheckout.CustomerBasket.Count() > 3)
            {
                List<Book> booksToRemove = new List<Book>();
                var differentBooks = customerCheckout.CustomerBasket.Distinct();
                if (differentBooks.Count() > 3)
                {

                    booksToRemove.AddRange(differentBooks);
                    booksToRemove.ForEach(book =>
                    {
                        customerCheckout.CustomerBasket.Remove(book);
                        customerCheckout.CheckedOut.Add(book);
                        customerCheckout.RunningTotal += TwentyPercentDiscount(NormalPrice);
                    });
                }
            }

            //fourth:
            //If you buy 3 different books, you get a 10 % discount.

            if (customerCheckout.CustomerBasket.Any()
                && customerCheckout.CustomerBasket.Count() > 2)
            {
                List<Book> booksToRemove = new List<Book>();
                var differentBooks = customerCheckout.CustomerBasket.Distinct();
                if (differentBooks.Count() > 2)
                {

                booksToRemove.AddRange(differentBooks);
                booksToRemove.ForEach(book =>
                {
                    customerCheckout.CustomerBasket.Remove(book);
                    customerCheckout.CheckedOut.Add(book);
                    customerCheckout.RunningTotal += TenPercentDiscount(NormalPrice);
                });
                }
            }

            //fifth rule;
            //If, however, you buy two different books,
            //you get a 5% discount on those two books.

            if (customerCheckout.CustomerBasket.Any() 
                   && customerCheckout.CustomerBasket.Count()>1)
            {
                List<Book> booksToRemove = new List<Book>();

                for (int i = 0; i < customerCheckout.CustomerBasket.Count(); i++)
                {
                    for (int ii = i+1; ii < customerCheckout.CustomerBasket.Count(); ii++)
                    {
                        if (i != ii &&
                            customerCheckout.CustomerBasket[i].Name !=
                            customerCheckout.CustomerBasket[ii].Name)
                        {
                            if (!booksToRemove.Contains(customerCheckout.CustomerBasket[i]))
                            {
                                booksToRemove.Add(customerCheckout.CustomerBasket[i]);
                            }
                            booksToRemove.Add(customerCheckout.CustomerBasket[ii]);
                        }
                    }
                }
                booksToRemove.ForEach(book =>
                {
                    customerCheckout.CustomerBasket.Remove(book);
                    customerCheckout.CheckedOut.Add(book);
                    customerCheckout.RunningTotal += FivePercentDiscount(NormalPrice);
                });
            }

            //sixth rule;
            //existing book price 8 EUR
            while (customerCheckout.CustomerBasket.Any())
            {
                List<Book> booksToRemove = new List<Book>();
                customerCheckout.CustomerBasket.ForEach(book =>
                {
                    booksToRemove.Add(book);
                });
                booksToRemove.ForEach(book =>
                {
                    customerCheckout.CustomerBasket.Remove(book);
                    customerCheckout.CheckedOut.Add(book);
                    customerCheckout.RunningTotal += NormalPrice;
                });
            }

            return customerCheckout;
        }
    }
}