using MessageQueueComponent.Interface;
using System;
using System.Collections.Generic;
using System.Timers;
using Timer = System.Timers.Timer;

namespace MessageQueueComponent
{
    internal class MessageQueueProvider : IMessageQueueProvider, IDisposable
    {
        private readonly IList<IMessage> lstMessage;
        private Timer tmrTimer;

        public event EventHandler? QueueChanged;
        public event EventHandler? TimerElapsed;

        public MessageQueueProvider()
        {
            lstMessage = new List<IMessage>();

            tmrTimer = new Timer();
            tmrTimer.Interval = 500;
            tmrTimer.AutoReset = true;
            tmrTimer.Elapsed += OnTimerElapsed;
            tmrTimer.Start();
        }

        public bool HasPendingMessage
        {
            get { return lstMessage.Count > 0; }
        }

        public IList<IMessage> GetPendingMessage()
        {
            ClearObsoleteMessage();

            return lstMessage;
        }

        public void AddMessageToQueue(string sHeader, string sContent, MessageType enumType)
        {
            switch (enumType)
            {
                case MessageType.Information:
                    lstMessage.Add(Message.Create(sHeader, sContent, new TimeSpan(0, 0, 1), DateTimeOffset.UtcNow, MessageType.Information));
                    break;
                case MessageType.Success:
                    lstMessage.Add(Message.Create(sHeader, sContent, new TimeSpan(0, 0, 1), DateTimeOffset.UtcNow, MessageType.Success));
                    break;
                case MessageType.Warning:
                    lstMessage.Add(Message.Create(sHeader, sContent, new TimeSpan(0, 0, 10), DateTimeOffset.UtcNow, MessageType.Warning));
                    break;
                case MessageType.Error:
                    lstMessage.Add(Message.Create(sHeader, sContent, new TimeSpan(0, 0, 60), DateTimeOffset.UtcNow, MessageType.Error));
                    break;
                default:
                    break;
            }

            NotifyQueueChanged();
        }

        public void AddInformationMessageToQueue(string sHeader, string sContent)
        {
            lstMessage.Add(Message.Create(sHeader, sContent, new TimeSpan(0, 0, 5), DateTimeOffset.UtcNow, MessageType.Information));

            NotifyQueueChanged();
        }

        public void AddSuccessMessageToQueue(string sHeader, string sContent)
        {
            lstMessage.Add(Message.Create(sHeader, sContent, new TimeSpan(0, 0, 5), DateTimeOffset.UtcNow, MessageType.Success));

            NotifyQueueChanged();
        }

        public void AddWarningMessageToQueue(string sHeader, string sContent)
        {
            lstMessage.Add(Message.Create(sHeader, sContent, new TimeSpan(0, 1, 0), DateTimeOffset.UtcNow, MessageType.Warning));

            NotifyQueueChanged();
        }

        public void AddErrorMessageToQueue(string sHeader, string sContent)
        {
            lstMessage.Add(Message.Create(sHeader, sContent, TimeSpan.MaxValue, DateTimeOffset.UtcNow, MessageType.Error));

            NotifyQueueChanged();
        }

        public void RemoveMessage(IMessage msgActual)
        {
            if (lstMessage.Contains(msgActual) == false)
                return;

            lstMessage.Remove(msgActual);

            NotifyQueueChanged();
        }

        private void ClearObsoleteMessage()
        {
            bool bQueueChanged = false;

            for (int i = lstMessage.Count + -1; i >= 0; i--)
            {
                if (lstMessage[i].IsObsolete == true)
                {
                    lstMessage.RemoveAt(i);
                    bQueueChanged = true;
                }
            }

            if (bQueueChanged == true)
                NotifyQueueChanged();
        }

        private void OnTimerElapsed(object? oSender, ElapsedEventArgs evntArg)
        {
            ClearObsoleteMessage();

            NotifyTimerElapsed();
        }

        private void NotifyQueueChanged()
        {
            OnQueueChangedEvent();
        }

        private protected virtual void OnQueueChangedEvent()
        {
            EventHandler? evntHandler = QueueChanged;

            if (evntHandler is not null)
                evntHandler(this, EventArgs.Empty);
        }

        private void NotifyTimerElapsed()
        {
            OnTimerElapsedEvent();
        }

        private protected virtual void OnTimerElapsedEvent()
        {
            EventHandler? evntHandler = TimerElapsed;

            if (evntHandler is not null)
                evntHandler(this, EventArgs.Empty);
        }

        void IDisposable.Dispose()
        {
            if (tmrTimer is not null)
            {
                tmrTimer.Elapsed -= OnTimerElapsed;
                tmrTimer.Stop();
            }
        }
    }
}