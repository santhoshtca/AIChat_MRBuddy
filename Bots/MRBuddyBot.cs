// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MRBuddy
{
    public class MRBuddyBot<T> : ActivityHandler where T : Dialog
    {
        protected readonly BotState ConversationState;
        protected readonly Dialog Dialog;
        protected readonly ILogger Logger;
        protected readonly BotState UserState;

        public MRBuddyBot(ConversationState conversationState, UserState userState, T dialog, ILogger<MRBuddyBot<T>> logger)
        {
            ConversationState = conversationState;
            UserState = userState;
            Dialog = dialog;
            Logger = logger;
        }

        protected override async Task OnMembersAddedAsync(
          IList<ChannelAccount> membersAdded,
          ITurnContext<IConversationUpdateActivity> turnContext,
          CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                // Greet anyone that was not the target (recipient) of this message.
                // To learn more about Adaptive Cards, see https://aka.ms/msbot-adaptivecards for more details.
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    
                    var welcomeCard = CreateAdaptiveCardAttachment();
                    var response = MessageFactory.Attachment(welcomeCard, ssml: "Welcome to Bot Framework!");
                    await turnContext.SendActivityAsync(response, cancellationToken);

                    MediaCardProperties mediaCardProperties = new MediaCardProperties();
                    mediaCardProperties.CardTitle = "Hello !! I can assist you with following queries?";

                    Media_Cards media_Cards = new Media_Cards();
                    List<string> options = new List<string>();
                    options.Add("Need help with Examcards");
                    options.Add("Need help with Coil Combination");
                    options.Add("Need help with Coil Information");
                    options.Add("Need help with T/R Coil Information");

                    var promptMessage = (Activity)MessageFactory.Attachment(media_Cards.HeroCard(options, mediaCardProperties));
                    await turnContext.SendActivityAsync(promptMessage, cancellationToken);
                }
            }
        }

        private Attachment CreateAdaptiveCardAttachment()
        {
            FileStream stream = File.Open(".\\Cards\\welcomeCard.json", FileMode.Open);

            {
                using (var reader = new StreamReader(stream))
                {
                    var adaptiveCard = reader.ReadToEnd();
                    return new Attachment()
                    {
                        ContentType = "application/vnd.microsoft.card.adaptive",
                        Content = JsonConvert.DeserializeObject(adaptiveCard),
                    };
                }
            }
        }
        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occurred during the turn.
            await ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            Logger.LogInformation("Running dialog with Message Activity.");

            // Run the Dialog with the new message Activity.
            await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
        }
    }
}
