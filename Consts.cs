using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Microsoft.BotBuilderSamples.Bots
{
    public static class Consts
    {
        public static readonly string hostUrl = @"https://your_bot_url";

        // obtain from @botfather https://core.telegram.org/bots#6-botfather
        public static readonly string botToken = @"your token here";
        public static readonly TelegramBotClient botClient = new TelegramBotClient(botToken);

        public static readonly int chatId = 234224353; //Telegram ChatID Like 234224353

        // Tranzzo payments provider token, guide here https://cdn.tranzzo.com/tranzzo-api/index.html#telegram-pay
        public static readonly string testTranzzoProviderToken = "410694247:TEST:0c5307bb-4064-4c27-89e8-7a24e9f0e987";
    }
}
