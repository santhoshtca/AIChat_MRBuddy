using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Recognizers;
using System.Collections.Generic;

namespace MRBuddy
{
    public static class IntentRecognizer
    {
        public static  Recognizer CreateRecognizer()
        {
           return new RegexRecognizer
            {
                Intents = new List<IntentPattern>
                {                    
                     new IntentPattern("combination", "(?i)Need help with coil combination"),
                     new IntentPattern("information", "(?i)Need help with coil information"),
                     new IntentPattern("examcard", "(?i)Examcard"),
                     new IntentPattern("coil","(?i)Need help with T/R Coil Information"),
                     new IntentPattern("default" , "(?i)default"),
                     new IntentPattern("default" , "(?i)menu"),
                     new IntentPattern("exit", "(?i)exit")
                }
            };
        }
    }
}

