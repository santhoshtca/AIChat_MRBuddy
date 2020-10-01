// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveExpressions;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

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
            await stepContext.Context.SendActivityAsync(stepContext.Result.ToString() + "is a T/R coil");
            await stepContext.Context.SendActivityAsync("Do you have any other issues?");
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}