namespace Application.Exceptions
{
    public class BookIsOutException : Exception
    {
        public BookIsOutException()
            : base("This book is out")
        {

        }
    }
}
