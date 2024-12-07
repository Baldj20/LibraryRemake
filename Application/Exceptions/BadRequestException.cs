using FluentValidation.Results;
using System.Text;

namespace Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(ValidationResult result)
            : base(GenerateMessage(result))
        {

        }

        private static string GenerateMessage(ValidationResult result)
        {
            var message = new StringBuilder();
            foreach (var error in result.Errors)
            {
                message.Append($"Error in property \"{error.PropertyName}\": {error.ErrorMessage}\n");
            }

            return message.ToString();
        }
    }
}
