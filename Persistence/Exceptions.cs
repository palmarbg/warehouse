namespace Persistence
{
    public class JSonError : Exception
    {
        public JSonError(string message) : base(message) { }
        public JSonError() : base() { }
    }

    public class InvalidRobotPositionException : Exception
    {
        public InvalidRobotPositionException(string message) : base(message) { }
        public InvalidRobotPositionException() : base() { }
    }
    public class InvalidMapDetailsException : Exception
    {
        public InvalidMapDetailsException(string message) : base(message) { }
        public InvalidMapDetailsException() : base() { }
    }
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException(string message) : base(message) { }
        public InvalidArgumentException() : base() { }
    }
}
