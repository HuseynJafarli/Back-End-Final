namespace YouPlay.Business.Exceptions.Common
{
    public class InvalidIdException : Exception
    {
        public InvalidIdException()
        {
        }

        public InvalidIdException(string? message) : base(message)
        {
        }
    }
}
