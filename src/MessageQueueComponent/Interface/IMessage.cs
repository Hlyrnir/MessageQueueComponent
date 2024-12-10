namespace MessageQueueComponent.Interface
{
    public interface IMessage
    {
        string Content { get; init; }
        TimeSpan ElapsedTime { get; }
        string Header { get; init; }
        Guid Id { get; }
        bool IsObsolete { get; }
        TimeSpan Lifetime { get; }
        DateTimeOffset PostedAt { get; }
        MessageType Type { get; init; }
    }
}