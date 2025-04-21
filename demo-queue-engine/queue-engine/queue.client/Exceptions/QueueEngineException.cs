namespace queue.client.Exceptions;

public class QueueEngineException : Exception
{
    public QueueEngineException() { }
    public QueueEngineException(string message) 
        : base(message) { }
    public QueueEngineException(string message, Exception inner) 
        : base(message, inner) { }
}