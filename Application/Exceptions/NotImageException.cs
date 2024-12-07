namespace Application.Exceptions
{
    public class NotImageException : Exception
    {
        public NotImageException()
            :base("Given file is not an image")
        {
            
        }
    }
}
