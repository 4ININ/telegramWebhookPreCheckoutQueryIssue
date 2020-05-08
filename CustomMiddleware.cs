using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate next;

        public CustomMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var a = context.Request;

            await Consts.botClient.SendTextMessageAsync(Consts.chatId, string.Format("CustomMiddleware context.Request\nP: {0} M: {1} H: {2}", a.Path, a.Method, a.Host.Value));

            try
            {
                await WebhookHelper.GetCustomWebhookInfoAsync();

                #region context.Items
                if (context.Items.Values.Count > 0)
                {
                    string contItemsResult = "";
                    foreach (var item in context.Items)
                    {
                        contItemsResult += "[" + item.Key.ToString() + ":" + item.Value.ToString() + "]\n";
                    }

                    await Consts.botClient.SendTextMessageAsync(Consts.chatId, "CustomMiddleware context.Items:\n" + contItemsResult);
                }
                #endregion
     
            }
            catch (System.Exception ex)
            {
                await Consts.botClient.SendTextMessageAsync(Consts.chatId, "CustomMiddleware ex Msg:\n" + ex.Message);
                await Consts.botClient.SendTextMessageAsync(Consts.chatId, "CustomMiddleware ex StackTrace:\n" + ex.StackTrace);
            }
            await next.Invoke(context);
        }

        async Task<string> ParseResponse(HttpContext hc)
        {
            string result = null;
            string streamResult = null;

            if (hc.Request.Body.Length > 0)
            {
                using (StreamReader sr = new StreamReader(hc.Request.Body))
                {
                    streamResult = sr.ReadToEnd();
                }
                result = "context.Response:\n"
                                //  + "HttpContext : " + hc.Response.HttpContext.ToString()
                                //+ "StatusCode : " + hc.Response.StatusCode.ToString()
                                // + "Headers : " + hc.Response.Headers.ToString()
                                // + "Body : " + hc.Response.Body.ToString()
                                // + "ContentLength : " + hc.Response.ContentLength.ToString()
                                // + "ContentType : " + hc.Response.ContentType.ToString()
                                //+ "Cookies : " + hc.Response.Cookies.ToString()
                                //+ "HasStarted : " + hc.Response.HasStarted.ToString()
                                + "STREAM BODY:" + streamResult;
                await Consts.botClient.SendTextMessageAsync(Consts.chatId, "Custom middleware Response:\n" + result);
            }
            else
            {
                result = "context.Response:\n"
                    + "STREAM BODY isNULL";
            }

            return result;
        }

        public async Task LogAsync(IActivity activity)
        {
            await Consts.botClient.SendTextMessageAsync(Consts.chatId, $"From:{activity.From.Id} - To:{activity.Recipient.Id} - Message:{activity.AsMessageActivity()?.Text}");
        }        
    }
}
