using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace dotnettechnicaltest.Tests.tests {

/*
 *        One copy of any of the five books costs 8 EUR.

        Your mission is to write a piece of code to calculate the price of 
        any conceivable shopping basket (containing only Harry Potter books), 
        giving as big a discount as possible.

        For example, how much does this basket of books cost?

        2 copies of the first book
        2 copies of the second book
        2 copies of the third book
        1 copy of the fourth book
        1 copy of the fifth book
        Answer: 51.20 EUR   **** 
        < this is the result of applying the first and forth rule?
        < it appears to also discount a book for free?
        < can i see the working out for this result?

        RULES:

        first rule to apply:
        Note that if you buy, say, four books, of which 3 are different titles, 
        you get a 10% discount on the 3 that form part of a set, but the fourth book still costs the original price.

        second rule to apply:        
        If you go the whole hog, and buy all 5, you get a huge 25% discount.

        third:
        If you buy 4 different books, you get a 20% discount.
        
        fourth:
        If you buy 3 different books, you get a 10% discount.

        fifth:
        If, however, you buy two different books, you get a 5% discount on those two books.

        sixth:
        existing book price 8 EUR
 */

    public class PricingTests
    {
        private Fixture autofixture;

        public PricingTests()
        {
            autofixture = new Fixture();
        }

        [Fact]
        public void WhenEmptyCheckout()
        {
            //Arrange
            var subject = new Pricing();

            //Act

            var result = subject.Checkout(new int[0] {});

            //Assert
            result.Should().Be(0);
        }

        private List<int> bookList => new List<int>();

        [Fact]
        public void WhenOneTypeOfBookInCheckout()
        {
            //Arrange
            var subject = new Pricing();
            //Act

            var result = subject.Checkout(new []{1});

            //Assert
            result.Should().Be(8);
        }

    }
}
