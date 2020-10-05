// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace MRBuddy
{
    internal class DisplayHelpOption : ComponentDialog
    {
        public DisplayHelpOption()
        {
              AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
                {
                    DisplayOptionAsync,
                }));

                AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
                InitialDialogId = nameof(WaterfallDialog);
        }

        

        private static async Task<DialogTurnResult> DisplayOptionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            MediaCardProperties mediaCardProperties = new MediaCardProperties();
            mediaCardProperties.CardTitle = "Hello !! I can assist you with following queries?";
            Media_Cards media_Cards = new Media_Cards();
            List<string> options = new List<string>();
            options.Add("Need help with Examcards");
            options.Add("Need help with Coil Combination");
            options.Add("Need help with Coil Information");
            options.Add("Need help with T/R Coil Information");

            var promptMessage = MessageFactory.Attachment(media_Cards.HeroCard(options, mediaCardProperties));
            await stepContext.Context.SendActivityAsync(promptMessage, cancellationToken);
            return await stepContext.EndDialogAsync();
        }
    }
}