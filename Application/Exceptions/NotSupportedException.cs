namespace Application.Exceptions
{
    public class NotSupportedException : Exception
    {
        public NotSupportedException(string source, string actionName)
            : base($"{source} does not support {actionName} action")
        {

        }
    }
}
