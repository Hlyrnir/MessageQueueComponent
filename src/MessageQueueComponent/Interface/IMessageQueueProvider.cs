using System;
using System.Collections.Generic;

namespace MessageQueueComponent.Interface
{
    public interface IMessageQueueProvider
    {
        bool HasPendingMessage { get; }

        event EventHandler? TimerElapsed;
        event EventHandler? QueueChanged;

        void AddErrorMessageToQueue(string sHeader, string sContent);
        void AddInformationMessageToQueue(string sHeader, string sContent);
        void AddMessageToQueue(string sHeader, string sContent, MessageType enumType);
        void AddSuccessMessageToQueue(string sHeader, string sContent);
        void AddWarningMessageToQueue(string sHeader, string sContent);
        IList<IMessage> GetPendingMessage();
        void RemoveMessage(IMessage msgActual);
    }
}