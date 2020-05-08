using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace Microsoft.BotBuilderSamples.Bots
{
    public static class WebhookHelper
    {
        public static async Task GetCustomWebhookInfoAsync()
        {
            ///TODO: GetWebhookInfo
            WebhookInfo webhookInfo = await Consts.botClient.GetWebhookInfoAsync();
            string allUpd = "[";
            if (webhookInfo.AllowedUpdates != null)
            {
                foreach (var item in webhookInfo.AllowedUpdates)
                {
                    allUpd += item.ToString() + "|";
                }
                allUpd += "]";
            }
            else
            {
                allUpd = "null";
            }

            await Consts.botClient.SendTextMessageAsync(Consts.chatId, "CustomMiddleware GetWebhookInfoAsync:" +
                "\nUrl : " + webhookInfo.Url +
                "\nHas cust sert : " + webhookInfo.HasCustomCertificate.ToString() +
                "\nPending upd : " + webhookInfo.PendingUpdateCount.ToString() +
                "\nLastErrorDate : " + webhookInfo.LastErrorDate.ToString() +
                "\nLastErrorMsg : " + webhookInfo.LastErrorMessage +
                "\nMax conn : " + webhookInfo.MaxConnections.ToString() +
                "\nAllowedUpdates : " + allUpd);
        }
    }
}
