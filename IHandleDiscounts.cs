namespace dotnet_technical_test.Tests
{
    public interface IHandleDiscounts
    {
        Checkout DiscountFromBooks(Checkout customerCheckout);
    }
}