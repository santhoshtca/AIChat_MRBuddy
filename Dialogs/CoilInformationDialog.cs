// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveExpressions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace MRBuddy
{
    internal class CoilInformationDialog : ComponentDialog
    {
        public CoilInformationDialog()
        {

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                CoilNameStepAsync,
                RetreiveValueAsync,
            }));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            InitialDialogId = nameof(WaterfallDialog);
        }

        private static async Task<DialogTurnResult> CoilNameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Pleas enter the coilname") }, cancellationToken);
        }

        private static async Task<DialogTurnResult> RetreiveValueAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            CoilModel coil = CoilData.GetCoilDatails(stepContext.Result.ToString());

            if (coil != null)
            {
                MediaCardProperties mediaCardProperties = new MediaCardProperties();
                mediaCardProperties.CardTitle = coil.CoilName + " (" + coil.CoilType + ")";
                mediaCardProperties.CardSubtitle = coil.Applications;
                mediaCardProperties.CardText = coil.Design;
                mediaCardProperties.URL = coil.ImagePath;

                Media_Cards mediaCards = new Media_Cards();
                Attachment attachment = mediaCards.ThumbnailCard(mediaCardProperties);

                await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(attachment));
            }
            else
            {
                await stepContext.Context.SendActivityAsync(stepContext.Result.ToString() +" not found!");
            }
           await stepContext.Context.SendActivityAsync("For more information about Coils , Please refer under Coils tab in User Documentation");
           var reply = MessageFactory.Text("Do you have other queries ");
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
            {
                new CardAction() { Title = "Yes", Type = ActionTypes.ImBack, Value = "default" },
                new CardAction() { Title = "No", Type = ActionTypes.ImBack, Value = "exit"}
            },
            };
            reply.InputHint = InputHints.ExpectingInput;
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}

