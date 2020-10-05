using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MRBuddy.Dialogs
{
    public class ExamCardDialog : ComponentDialog
    {
       
        public ExamCardDialog()
            : base(nameof(ExamCardDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new AttachmentPrompt(nameof(AttachmentPrompt)));
            
            var waterfallSteps = new WaterfallStep[]
            {
                InitialStepAsync,
                SelectHelpType,
                ViewHelp,
                FinalStepAsync
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> ViewHelp(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {            
            var message = stepContext.Context.Activity.Text.ToLower();
            Media_Cards media_Cards = new Media_Cards();

            if (message.Contains("video"))
            {
                var attachment = GetVideoPromptMessage();
                var promptMessage = (Activity)MessageFactory.Attachment(attachment);
                await stepContext.Context.SendActivityAsync(promptMessage, cancellationToken);
            }    
            else if(message.Contains("document"))
            {
                var attachment = GetDocumentPromptMessage();
                var promptMessage = (Activity)MessageFactory.Attachment(attachment);
                await stepContext.Context.SendActivityAsync(promptMessage, cancellationToken);
            }
            
            return await stepContext.NextAsync(null, cancellationToken);
        }

        private Attachment GetVideoPromptMessage()
        {
            MRBuddyConstants _mrBuddyConstants = new MRBuddyConstants();
            MediaCardProperties mediaCardPropertiesObj = new MediaCardProperties();
            Media_Cards _mediaCardObj = new Media_Cards();
            Attachment videoAttachment = new Attachment(); ;
            if (SelectedExamcardQuery.Contains("create"))
            {
                mediaCardPropertiesObj.CardTitle = "Create ExamCard Video";
                mediaCardPropertiesObj.CardSubtitle = "This is the Sample video of how to create ExamCard";
                mediaCardPropertiesObj.CardText = _mrBuddyConstants.CreateExamCardteVideoPath;
                mediaCardPropertiesObj.URL = _mrBuddyConstants.CreateExamCardteVideoPath;

            }
            if (SelectedExamcardQuery.Contains("import"))
            {
                mediaCardPropertiesObj.CardTitle = "Import ExamCard Video";
                mediaCardPropertiesObj.CardSubtitle = "This is the Sample video of how to Import ExamCard";
                mediaCardPropertiesObj.URL = _mrBuddyConstants.ImportExamCardVideoPath;

            }
            if (SelectedExamcardQuery.Contains("export"))
            {
                mediaCardPropertiesObj.CardTitle = "Export ExamCard Video";
                mediaCardPropertiesObj.CardSubtitle = "This is the Sample video of how to Export ExamCard";
                mediaCardPropertiesObj.URL = _mrBuddyConstants.ExportExamImportVideoPath;
            }

            videoAttachment = _mediaCardObj.VideoAttachment(mediaCardPropertiesObj);
            return videoAttachment;
        }

        private Attachment GetDocumentPromptMessage()
        {
            MRBuddyConstants _mrBuddyConstants = new MRBuddyConstants();
            MediaCardProperties mediaCardPropertiesObj = new MediaCardProperties();
            Media_Cards _mediaCardObj = new Media_Cards();
            Attachment docAttachment = new Attachment(); ;
            if (SelectedExamcardQuery.Contains("create"))
            {
                mediaCardPropertiesObj.CardTitle = "Create ExamCard Helpdoc";
                mediaCardPropertiesObj.CardSubtitle = "This is help document on how to Create ExamCard";
                mediaCardPropertiesObj.URL = _mrBuddyConstants.CreateExamCardDOCPath;

            }
            if (SelectedExamcardQuery.Contains("import"))
            {
                mediaCardPropertiesObj.CardTitle = "Import ExamCard Helpdoc";
                mediaCardPropertiesObj.CardSubtitle = "This is help document on how to Import ExamCard";
                mediaCardPropertiesObj.URL = _mrBuddyConstants.ImportExamCardDOCPath;

            }
            if (SelectedExamcardQuery.Contains("export"))
            {
                mediaCardPropertiesObj.CardTitle = "Export ExamCard Helpdoc";
                mediaCardPropertiesObj.CardSubtitle = "This is help document on how to Import ExamCard";
                mediaCardPropertiesObj.URL = _mrBuddyConstants.ExportExamImportDOCPath;

            }

            docAttachment = _mediaCardObj.FileAttachment(mediaCardPropertiesObj);
            return docAttachment;
        }

        private async Task<DialogTurnResult> SelectHelpType(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            
            var message = stepContext.Context.Activity.Text;
            SelectedExamcardQuery = message.ToLower();

            var reply = MessageFactory.Text("What kind of help you prefer");
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
            {
                new CardAction() { Title = "Video walkthrough", Type = ActionTypes.ImBack, Value = "Video walkthrough" },
                new CardAction() { Title = "Document instruction", Type = ActionTypes.ImBack, Value = "Document instruction"}
            },
            };
            reply.InputHint = InputHints.ExpectingInput;
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);
            return new DialogTurnResult(DialogTurnStatus.Waiting);

        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var reply = MessageFactory.Text("Do you have other queries");
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

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var reply = MessageFactory.Text("Select an option");
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
            {
                new CardAction() { Title = "Create Examcard", Type = ActionTypes.ImBack, Value = "Create" },
                new CardAction() { Title = "Import Examcard", Type = ActionTypes.ImBack, Value = "Import"},
                new CardAction() { Title = "Export Examcard", Type = ActionTypes.ImBack, Value = "Export"},
            },
            };
            reply.InputHint = InputHints.ExpectingInput;
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);
            return new DialogTurnResult(DialogTurnStatus.Waiting);
        }

        private string SelectedExamcardQuery { get; set; }
    }
}
