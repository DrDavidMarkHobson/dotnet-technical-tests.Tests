using System;
using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using Moq.AutoMock;
using Xunit;

namespace dotnet_technical_test.Tests
{
    /*
     Note, i've written the points as 'rules' in order for them to make sense
     I've also implemented an order of precedence myself as none was specified.
     I also think the answer giving the example may be wrong, so I'm not able to get the same result.
     This stands out in the specific test which fails.

     I believe I can correct this but need to know the following:
     - What is the order of precedence for the rules
     - What was the mathematical proof for the price calculation for the example provided

     I cannot work this out without this information so I am submitting it as is.
     */

    /*
        Problem Description:

        To try and encourage more sales of the 5 different Harry Potter books they sell, 
        a bookshop has decided to offer discounts of multiple-book purchases.

        One copy of any of the five books costs 8 EUR.

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

    public class DiscountHandlerTests
    {
        public Fixture AutoFixture { get; set; }
        public AutoMocker Mocker { get; set; }

        public DiscountHandlerTests()
        {
            AutoFixture = new Fixture();
            Mocker = new AutoMocker();

        }

        //sixth rule:
        //existing book price 8 EUR

        [Fact]
        public void WhenTestOneBook()
        {
            //Arrange
            var customersBooks = new List<Book>() {new Book("Book One")};
            var testVar = new Checkout(customersBooks);
            var subject = Mocker.CreateInstance<DiscountHandler>();

            //Act
            var result = subject.DiscountFromBooks(testVar);

            //Assert
            customersBooks.ForEach(book =>
            {
                result.CheckedOut.Should().Contain(book);
            });

            result.RunningTotal.Should().Be((decimal)8);
        }

        //sixth rule:
        //existing book price 8 EUR

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void WhenTestManyCopiesOfTheSameBook(int numberOfCopies)
        {
            //Arrange
            var customersBook = new Book(AutoFixture.Create<string>());
            var customersBooks = new List<Book>();
            customersBooks.AddMany(() => customersBook, numberOfCopies);

            var testVar = new Checkout(customersBooks);
            var subject = Mocker.CreateInstance<DiscountHandler>();

            //Act
            var result = subject.DiscountFromBooks(testVar);

            //Assert
            customersBooks.ForEach(book =>
            {
                result.CheckedOut.Should().Contain(book);
            });

            result.RunningTotal.Should().Be((decimal)8*numberOfCopies);
        }

        //fifth rule:
        //If, however, you buy two different books, you get a 5% discount on those two books.

        [Fact]
        public void WhenTestTwoDifferentBooks()
        {
            //Arrange
            var customersBook1 = new Book(AutoFixture.Create<string>());
            var customersBook2 = new Book(AutoFixture.Create<string>());
            var customersBooks = new List<Book>();
            customersBooks.Add(customersBook1);
            customersBooks.Add(customersBook2);

            var testVar = new Checkout(customersBooks);
            var subject = Mocker.CreateInstance<DiscountHandler>();

            var bookPrice = (decimal) 8;
            var discount = bookPrice * (decimal)0.05;
            var expectedPrice = (bookPrice - discount) * 2;

            //Act
            var result = subject.DiscountFromBooks(testVar);

            //Assert
            customersBooks.ForEach(book =>
            {
                result.CheckedOut.Should().Contain(book);
            });

            result.RunningTotal.Should().Be(expectedPrice);
        }

        //fourth:
        //If you buy 3 different books, you get a 10% discount.

        [Fact]
        public void WhenTestThreeDifferentBooks()
        {
            //Arrange
            var customersBook1 = new Book(AutoFixture.Create<string>());
            var customersBook2 = new Book(AutoFixture.Create<string>());
            var customersBook3 = new Book(AutoFixture.Create<string>());
            var customersBooks = new List<Book>();
            customersBooks.Add(customersBook1);
            customersBooks.Add(customersBook2);
            customersBooks.Add(customersBook3);

            var testVar = new Checkout(customersBooks);
            var subject = Mocker.CreateInstance<DiscountHandler>();

            var bookPrice = (decimal)8;
            var discount = bookPrice * (decimal)0.1;
            var expectedPrice = (bookPrice - discount) * 3;

            //Act
            var result = subject.DiscountFromBooks(testVar);

            //Assert
            customersBooks.ForEach(book =>
            {
                result.CheckedOut.Should().Contain(book);
            });

            result.RunningTotal.Should().Be(expectedPrice);
        }

        //third:
        //If you buy 4 different books, you get a 20% discount.
        [Fact]
        public void WhenTestFourDifferentBooks()
        {
            //Arrange
            var customersBook1 = new Book(AutoFixture.Create<string>());
            var customersBook2 = new Book(AutoFixture.Create<string>());
            var customersBook3 = new Book(AutoFixture.Create<string>());
            var customersBook4 = new Book(AutoFixture.Create<string>());
            var customersBooks = new List<Book>();
            customersBooks.Add(customersBook1);
            customersBooks.Add(customersBook2);
            customersBooks.Add(customersBook3);
            customersBooks.Add(customersBook4);

            var testVar = new Checkout(customersBooks);
            var subject = Mocker.CreateInstance<DiscountHandler>();

            var bookPrice = (decimal)8;
            var discount = bookPrice * (decimal)0.2;
            var expectedPrice = (bookPrice - discount) * 4;

            //Act
            var result = subject.DiscountFromBooks(testVar);

            //Assert
            customersBooks.ForEach(book =>
            {
                result.CheckedOut.Should().Contain(book);
            });

            result.RunningTotal.Should().Be(expectedPrice);
        }

        //second rule to apply:        
        //If you go the whole hog, and buy all 5, you get a huge 25% discount.
        [Fact]
        public void WhenTestFiveDifferentBooks()
        {
            //Arrange
            var customersBook1 = new Book(AutoFixture.Create<string>());
            var customersBook2 = new Book(AutoFixture.Create<string>());
            var customersBook3 = new Book(AutoFixture.Create<string>());
            var customersBook4 = new Book(AutoFixture.Create<string>());
            var customersBook5 = new Book(AutoFixture.Create<string>());
            var customersBooks = new List<Book>();
            customersBooks.Add(customersBook1);
            customersBooks.Add(customersBook2);
            customersBooks.Add(customersBook3);
            customersBooks.Add(customersBook4);
            customersBooks.Add(customersBook5);

            var testVar = new Checkout(customersBooks);
            var subject = Mocker.CreateInstance<DiscountHandler>();

            var bookPrice = (decimal)8;
            var discount = bookPrice * (decimal)0.25;
            var expectedPrice = (bookPrice - discount) * 5;

            //Act
            var result = subject.DiscountFromBooks(testVar);

            //Assert
            customersBooks.ForEach(book =>
            {
                result.CheckedOut.Should().Contain(book);
            });

            result.RunningTotal.Should().Be(expectedPrice);
        }

        //first rule to apply:
        //Note that if you buy, say, four books, of which 3 are different titles, 
        //you get a 10% discount on the 3 that form part of a set, but the fourth book still costs the original price.

        [Fact]
        public void WhenTestFourBooksOfWhichThreeAreDifferent()
        {
            //Arrange
            var customersBook1 = new Book(AutoFixture.Create<string>());
            var customersBook2 = new Book(AutoFixture.Create<string>());
            var customersBook3 = new Book(AutoFixture.Create<string>());
            var customersBooks = new List<Book>();
            customersBooks.Add(customersBook1);
            customersBooks.Add(customersBook2);
            customersBooks.Add(customersBook3);
            customersBooks.Add(customersBook3);

            var testVar = new Checkout(customersBooks);
            var subject = Mocker.CreateInstance<DiscountHandler>();

            var bookPrice = (decimal)8;
            var discount = bookPrice * (decimal)0.1;
            var expectedPrice = (bookPrice - discount) * 3 + bookPrice;

            //Act
            var result = subject.DiscountFromBooks(testVar);

            //Assert
            customersBooks.ForEach(book =>
            {
                result.CheckedOut.Should().Contain(book);
            });

            result.RunningTotal.Should().Be(expectedPrice);
        }

        //example case from the spec:
        //2 copies of the first book
        //2 copies of the second book
        //2 copies of the third book
        //1 copy of the fourth book
        //1 copy of the fifth book
        //Answer: 51.20 EUR

        //Is this answer correct?
        // I can only get this answer through the application of two discounts.
        // the special 3 distinct + 1, and the 3 book case, minus the extra book.
        // If I apply the rules as they are written, the answer should actually 
        // be 55.20

        [Fact]
        public void WhenTestSpecifiedExample()
        {
            //Arrange 
            // number of books for example
            var customersBook1 = new Book(AutoFixture.Create<string>()); //2
            var customersBook2 = new Book(AutoFixture.Create<string>()); //2
            var customersBook3 = new Book(AutoFixture.Create<string>()); //2
            var customersBook4 = new Book(AutoFixture.Create<string>()); //1
            var customersBook5 = new Book(AutoFixture.Create<string>()); //1 
            var customersBooks = new List<Book>();
            customersBooks.Add(customersBook1);
            customersBooks.Add(customersBook1);
            customersBooks.Add(customersBook2);
            customersBooks.Add(customersBook2);
            customersBooks.Add(customersBook3);
            customersBooks.Add(customersBook3);
            customersBooks.Add(customersBook4);
            customersBooks.Add(customersBook5);

            var testVar = new Checkout(customersBooks);
            var subject = Mocker.CreateInstance<DiscountHandler>();

            //Act
            var result = subject.DiscountFromBooks(testVar);

            //Assert
            customersBooks.ForEach(book =>
            {
                result.CheckedOut.Should().Contain(book);
            });

            //I've spent some time trying to understand why the example is 51.20
            //But I don't see why it is.
            result.RunningTotal.Should().Be((decimal)51.2);
        }

    }
}
