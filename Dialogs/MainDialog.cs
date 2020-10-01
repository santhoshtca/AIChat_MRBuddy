// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Actions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Conditions;

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
                    /*
                    for each recog pattern ontent code should be added here
                     */

                    //new OnIntent() - to be replaced
                    //{
                    //    Actions = new List<Dialog>() { new CoilDialog() }
                    //}
                    new OnUnknownIntent()
                    {
                        Actions = new List<Dialog>() {new SendActivity("I am sorry I didnt understand your response !!")}
                    },
                };
            return triggers;
        }
    }
}
