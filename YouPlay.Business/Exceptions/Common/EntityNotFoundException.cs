namespace YouPlay.Business.Exceptions.Common
{
    public class EntityNotFoundException : Exception
    {
        public int StatusCode { get; set; }
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string? message) : base(message)
        {
        }
        public EntityNotFoundException(string? message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
