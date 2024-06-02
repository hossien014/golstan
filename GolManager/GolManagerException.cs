public class GolManagerException : Exception
{
    public int StatusCode { get; set; }
    public GolManagerException(string message, int _status) : base(message)
    {
        StatusCode = _status;
    }


}