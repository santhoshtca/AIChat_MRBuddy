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
using MRBuddy.Dialogs;

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
                    Intent="coil",
                    Actions = new List<Dialog>(){ new TRCoilRelatedDialog()}
                },
                new OnIntent()
                {
                   Intent = "combination",
                   Actions = new List<Dialog>() { new CoilCombinationDialog() }
                },
                new OnIntent()
                {
                   Intent = "information",
                   Actions = new List<Dialog>() { new CoilInformationDialog() }
                },
                new OnIntent()
                {
                    Intent = "examcard",
                    Actions = new List<Dialog>(){ new ExamCardDialog() }
                },
                new OnIntent()
                {
                    Intent = "default",
                    Actions = new List<Dialog>(){ new DisplayHelpOption()}
                },
                  new OnIntent()
                {
                    Intent = "exit",
                    Actions = new List<Dialog>(){ new SendActivity("Thank you for chatting with me!! If you would like to continue chat at any moment reply with Menu to get menu items") }
                },
                new OnUnknownIntent()
                {
                   Actions = new List<Dialog>() {new SendActivity("I'm still learning how to talk to people and dont understand your response!Please type Menu to get help menu")}
                },
            };
            
            return triggers;
        }
    
    }
}
