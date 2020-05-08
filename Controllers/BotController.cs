// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.BotBuilderSamples.Bots;

namespace Microsoft.BotBuilderSamples.Controllers
{
    // This ASP Controller is created to handle a request. Dependency Injection will provide the Adapter and IBot
    // implementation at runtime. Multiple different IBot implementations running at different endpoints can be
    // achieved by specifying a more specific type for the bot constructor argument.
    [Route("api/messages")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter Adapter;
        private readonly IBot Bot;

        public BotController(IBotFrameworkHttpAdapter adapter, IBot bot)
        {
            Adapter = adapter;
            Bot = bot;
        }

        [HttpPost, HttpGet]
        public async Task<IActionResult> PostAsync()
        {
            try
            {
                // Delegate the processing of the HTTP POST to the adapter.
                // The adapter will invoke the bot.
                await Adapter.ProcessAsync(Request, Response, Bot);

                await Consts.botClient.SendTextMessageAsync(Consts.chatId, string.Format("PostAsync context.Request\nP: {0} M: {1} H: {2}",
                    Request.Path, Request.Method, Request.Host.Value));
            }
            catch (System.Exception ex)
            {
                await Consts.botClient.SendTextMessageAsync(Consts.chatId, "PostAsync error \n" + ex.Message);
                await Consts.botClient.SendTextMessageAsync(Consts.chatId, "PostAsync error \n" + ex.StackTrace);
            }

            return Ok();
        }
    }
}
