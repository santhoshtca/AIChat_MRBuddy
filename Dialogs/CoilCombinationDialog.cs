// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace MRBuddy
{
    internal class CoilCombinationDialog : ComponentDialog
    {
        private string Product = string.Empty;
        private string coil1 = string.Empty;
        private string coil2 = string.Empty;
        public CoilCombinationDialog()
        {
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                SystemTypeStepAsync,
                MagnetStrengthAsync,
                FetchCoil1NameAsync,
                FetchCoil2NameAsync,
                RetreiveValueAsync
            
        }));
        
        AddDialog(new TextPrompt(nameof(TextPrompt)));
        AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
       InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> RetreiveValueAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            coil2 = stepContext.Result.ToString();
            string result = CoilData.isCoilCombinationValid(coil1, coil2);
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(result), cancellationToken);
            await stepContext.Context.SendActivityAsync("For more information about Coil Combination , Please refer under Coils tab in User Documentation");
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
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> FetchCoil2NameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            coil1 = stepContext.Result.ToString();
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Please enter the Second coil") }, cancellationToken);
        }
        private async Task<DialogTurnResult> FetchCoil1NameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Please enter the First coil") }, cancellationToken);
        }

        private async Task<DialogTurnResult> MagnetStrengthAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            Product = stepContext.Result.ToString();
            return await stepContext.PromptAsync(nameof(ChoicePrompt),
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("Please enter Main System Type."),
                    Choices = ChoiceFactory.ToChoices(new List<string> { "1.5T","3.0T" }),
                }, cancellationToken);
        }

        private async Task<DialogTurnResult> SystemTypeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = MessageFactory.Text("Please enter the System Type") }, cancellationToken);
        }
    }
}