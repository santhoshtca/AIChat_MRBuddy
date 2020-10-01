// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Actions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Conditions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Input;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Templates;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace MRBuddy
{
    public class MainDialog : ComponentDialog
    {
        private readonly UserState _userState;
        public MainDialog(UserState userState)
            : base(nameof(MainDialog))
        {
            _userState = userState;
            var rootDialog = new AdaptiveDialog();
            rootDialog.Recognizer = IntentRecognizer.CreateRecognizer();
            rootDialog.Triggers = TriggerActions();
            AddDialog(rootDialog);
            InitialDialogId = nameof(AdaptiveDialog);  
        }
        private List<OnCondition> TriggerActions()
        {
            var triggers = new List<OnCondition>
            {
                new OnIntent()
                {
                   Intent = "coil",
                   Actions = CoilOptionDisplay()
                },
                new OnUnknownIntent()
                {
                   Actions = new List<Dialog>() {new SendActivity("I am sorry I didnt understand your response !!")}
                },
            };
            
            return triggers;
        }
        private List<Dialog> CoilOptionDisplay()
        {
            var OptionDisplay = new List<Dialog>();
            var ChoiceOption = new ChoiceInput()
            {
                Property = "turn.MainDialog.choice",
                Style = ListStyle.Auto,
                Prompt = new ActivityTemplate("We can assist you with the below Coil related questions"),
                Choices = new ChoiceSet(new List<Choice>()
                {
                   new Choice("Queries related to coil combination"),
                   new Choice("Queries related to coil information"),
                   new Choice("Queries regarding T/R coils")
                })
                
            };
            var switchcase = new SwitchCondition()
            {
                Condition = "turn.MainDialog.choice",
                Cases = new List<Case>()
                {
                    new Case("Queries related to coil combination", new List<Dialog>() { new CoilCombinationDialog() }),
                    new Case("Queries related to coil information", new List<Dialog>() { new CoilInformationDialog() }),
                    new Case("Queries regarding T/R coils", new List<Dialog>() { new TRCoilRelatedDialog() })
                },
                Default = new List<Dialog>()
                {
                    new SendActivity("Please Select from the option from above menu"),
                }
            };
            OptionDisplay.Add(ChoiceOption);
            OptionDisplay.Add(switchcase);
            return OptionDisplay;
        }
    }
}
