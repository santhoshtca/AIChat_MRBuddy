using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRBuddy
{
    public class Media_Cards
    {
        MediaCardProperties mediaCardPropertiesObj = new MediaCardProperties();

        public Media_Cards(MediaCardProperties mediaCardProperties)
        {
            mediaCardPropertiesObj = mediaCardProperties;
        }
        public Attachment VideoAttachment()
        {
            var videoCard = new VideoCard
            {
                Title = mediaCardPropertiesObj.CardTitle,
                Subtitle = mediaCardPropertiesObj.CardSubtitle,

                Text = mediaCardPropertiesObj.CardText,
                Image = new ThumbnailUrl
                {
                    Url = mediaCardPropertiesObj.ImageURL,
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = mediaCardPropertiesObj.MediaURL,
                    },
                },
                Buttons = new List<CardAction>
                {
                    new CardAction()
                    {
                        Title = mediaCardPropertiesObj.ButtonTitle,
                        Type = ActionTypes.OpenUrl,
                        Value =  mediaCardPropertiesObj.ButtonTitle,

                    },
                },
            };
            return videoCard.ToAttachment();
        }
        public Attachment ImageCard()
        {
            return new Attachment
            {
                Name = mediaCardPropertiesObj.CardTitle,
                ContentType = "image/png",
                ContentUrl = mediaCardPropertiesObj.ImageURL
            };
        }

        public Attachment HeroCard(List<string> optionkey)
        {
            var heroCard = new HeroCard();
            heroCard.Title = mediaCardPropertiesObj.CardTitle;
            heroCard.Buttons = new List<CardAction>();
            foreach (var option in optionkey)
            {
                heroCard.Buttons.Add(new CardAction(ActionTypes.ImBack, option, value: option));
            }
            return heroCard.ToAttachment();
        }
    }
}
