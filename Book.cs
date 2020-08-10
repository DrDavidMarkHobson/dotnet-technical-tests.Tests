namespace dotnet_technical_test.Tests
{
    public class Book
    {
        public Book(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
    }
}