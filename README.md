# telegramWebhookPreCheckoutQueryIssue

To reproduce the problem i modificate BotBuilder-Samples from github : 
https://github.com/microsoft/BotBuilder-Samples/tree/master/samples/csharp_dotnetcore/02.echo-bot this is asp core project 

My modificated github project link :
https://github.com/4ININ/telegramWebhookPreCheckoutQueryIssue


Some helpful link for understanding problem
https://core.telegram.org/bots/api#sendinvoice
https://cdn.tranzzo.com/tranzzo-api/index.html#telegram-pay

Parts of code project

Consts.cs

Important fields for bot logic

appsettings.json

Set fields MicrosoftAppId and MicrosoftAppPassword from https://dev.botframework.com/

\Bots\EchoBot.cs

Here my bot send Invoice to user like TelegramBotClient.SendInvoiceAsync(...)

CustomMiddleware.cs

Here my bot checked all incoming updates. In my case i need to get preCheckoutQuery

It is important part becouse after user fiill invoice form and send it, my bot must receive preCheckoutQuery 

https://core.telegram.org/bots/api#precheckoutquery but i receive nothing!
