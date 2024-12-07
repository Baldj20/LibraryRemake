namespace Application.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException()
               : base("You do not have rights to do this action")
        {

        }
    }
}
