namespace Shared.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
        public NotFoundException(string message, object key)
            : base($"Entity \" {message}\" - Key: {key} was not found.")
        {

        }
    }
}
