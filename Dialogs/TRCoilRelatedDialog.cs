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
    internal class TRCoilRelatedDialog : ComponentDialog
    {       
        public TRCoilRelatedDialog()
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
            string coilType = CoilData.GetCoilType(stepContext.Result.ToString());
            await stepContext.Context.SendActivityAsync(coilType);

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