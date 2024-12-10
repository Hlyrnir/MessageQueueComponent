using System.Diagnostics;
using MessageQueueComponent.Interface;

namespace MessageQueueComponent
{
    internal sealed class Message : IMessage
    {
        private Stopwatch swWatch;

        private Guid guId;
        private string sHeader;
        private string sContent;

        private TimeSpan tsLifespan;
        private DateTimeOffset dtPostedAt;

        private MessageType tMessage;

        private Message(string sHeader, string sContent, TimeSpan tsLifespan, DateTimeOffset dtPostedAt, MessageType tMessage)
        {
            swWatch = new Stopwatch();
            swWatch.Start();

            guId = Guid.NewGuid();

            this.sHeader = sHeader;
            this.sContent = sContent;

            this.tsLifespan = tsLifespan;
            this.dtPostedAt = dtPostedAt;

            this.tMessage = tMessage;
        }

        public Guid Id { get => guId; }
        public string Header { get => sHeader; init => sHeader = value; }
        public string Content { get => sContent; init => sContent = value; }

        public DateTimeOffset PostedAt { get => dtPostedAt; }
        public TimeSpan ElapsedTime { get => swWatch.Elapsed; }
        public TimeSpan Lifetime { get => tsLifespan; }

        public MessageType Type { get => tMessage; init => tMessage = value; }

        public bool IsObsolete
        {
            get
            {
                if (swWatch.Elapsed < tsLifespan)
                    return false;

                return true;
            }
        }

        public static Message Create(string sHeader, string sContent, TimeSpan tsLifespan, DateTimeOffset dtPostedAt, MessageType tMessage)
        {
            return new Message(
                sHeader: sHeader,
                sContent: sContent,
                tsLifespan: tsLifespan,
                dtPostedAt: dtPostedAt,
                tMessage: tMessage);
        }
    }
}