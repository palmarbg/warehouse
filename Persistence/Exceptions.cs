namespace Persistence
{
    public class JSonError : Exception
    {
        public JSonError(string message) : base(message) { }
        public JSonError() : base() { }
    }
}
