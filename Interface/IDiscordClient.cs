﻿using GrindSoft.Models;

namespace GrindSoft.Interface
{
    public interface IDiscordClient
    {
        void UpdateData(SessionContext sessionContext);

        Task<string> FetchUserIdAsync();

        Task<List<MessageRecord>> GetLatestMessagesAsync();

        Task SendMessageAsync(string message, string replyToMessageId = null);

        Task SendTypingAsync();

        Task<string> SendMessageAndGetIdAsync(string message);

        Task DeleteMessageAsync(string messageId);

        void InitializeDeletionMechanism();
    }
}