// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyText = $"Echo: {turnContext.Activity.Text}";
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
            await TranzzoSendInvoiceAsync(Consts.chatId, "15,0.0", "Buy something");
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Tranzzo payment test app";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }

        // Documentation https://core.telegram.org/bots/api#sendinvoice
        // Tranzzo Documentation https://cdn.tranzzo.com/tranzzo-api/index.html#telegram-pay
        public static async Task TranzzoSendInvoiceAsync(int _chatId, string _price, string _headerDescription)
        {
            int ticketPrice = Int32.Parse(_price.Replace(".", "").Replace(",", ""));

            int chatId = _chatId; // _dialogID; // Unique identifier for the target private chat
            string title = _headerDescription; // Product name, 1-32 characters
            string description = "Description"; // Product description, 1-255 characters
            string payload = "test-payload-123-456-789"; // Bot-defined invoice payload, 1-128 bytes. This will not be displayed to the user, use for your internal processes.
            string providerToken = Consts.testTranzzoProviderToken; // Payments provider token, obtained via Botfather
            string startParameter = "test-startParameter-123-456-789"; // Unique deep-linking parameter that can be used to generate this invoice when used as a start parameter
            string currency = "UAH"; // Three-letter ISO 4217 currency code, see more on currencies

            LabeledPrice[] prices = {
                new LabeledPrice(_headerDescription, ticketPrice) //240 = 2,4uah минималка ~0.1$
                //new LabeledPrice("Позиция 2", 25000),
            };

            // Optional Parameters
            string providerData = null; // JSON-encoded data about the invoice, which will be shared with the payment provider. A detailed description of required fields should be provided by the payment provider.
            string photoUrl = "https://thumbs.dreamstime.com/b/money-evolution-isometric-concept-barter-to-cryptocurrency-money-evolution-isometric-concept-barter-to-cryptocurrency-136711190.jpg"; // URL of the product photo for the invoice. Can be a photo of the goods or a marketing image for a service. People like it better when they see what they are paying for
            int photoSize = 0; // Photo size
            int photoWidth = 0; // Photo width
            int photoHeight = 0; // Photo height
            bool needName = true; // Pass True, if you require the user's full name to complete the order
            bool needPhoneNumber = false; // Pass True, if you require the user's phone number to complete the order
            bool needEmail = true; // Pass True, if you require the user's email address to complete the order
            bool needShippingAddress = false; // Pass True, if you require the user's shipping address to complete the order
            bool isFlexible = false; // Pass True, if the final price depends on the shipping method
            bool disableNotification = false; // Sends the message silently. Users will receive a notification with no sound.
            int replyToMessageId = 0; // If the message is a reply, ID of the original message
            InlineKeyboardMarkup replyMarkup = null; // A JSON-serialized object for an inline keyboard.If empty, one 'Pay total price' button will be shown.If not empty, the first button must be a Pay button
            System.Threading.CancellationToken cancellationToken = default;
            bool sendPhoneNumberToProvider = false; // new fields v15.4.0
            bool sendEmailToProvider = false;       // new fields v15.4.0

            try
            {
                Task<Message> a = Consts.botClient.SendInvoiceAsync(
                    chatId, title, description, payload, providerToken, startParameter, currency, prices,

                    providerData, photoUrl, photoSize, photoWidth, photoHeight, needName, needPhoneNumber, needEmail,
                    needShippingAddress, isFlexible, disableNotification, replyToMessageId, replyMarkup, cancellationToken,
                    sendPhoneNumberToProvider, sendEmailToProvider
                 );

                await Consts.botClient.SendTextMessageAsync(chatId, "postSendInvoice");
            }
            catch (Exception ex)
            {
                await Consts.botClient.SendTextMessageAsync(chatId, "sendInvoice error \n" + ex.Message);
                await Consts.botClient.SendTextMessageAsync(chatId, "sendInvoice error \n" + ex.StackTrace);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
