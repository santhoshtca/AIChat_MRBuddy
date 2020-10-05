using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MRBuddy
{
    public class Media_Cards
    {
                
        public Attachment VideoAttachment(MediaCardProperties mediaCardPropertiesObj)
        {
            var videoCard = new VideoCard
            {
                Title = mediaCardPropertiesObj.CardTitle,
                Subtitle = mediaCardPropertiesObj.CardSubtitle,

                Text = mediaCardPropertiesObj.CardText,
                Image = new ThumbnailUrl
                {
                    Url = mediaCardPropertiesObj.URL,
                },
                Media = new List<MediaUrl>
                {
                    new MediaUrl()
                    {
                        Url = mediaCardPropertiesObj.URL,
                    },
                },
               
            };
            return videoCard.ToAttachment();
        }
        public Attachment ImageCard(MediaCardProperties mediaCardPropertiesObj)
        {
            return new Attachment
            {
                Name = mediaCardPropertiesObj.CardTitle,
                ContentType = "image/png",
                ContentUrl = mediaCardPropertiesObj.URL
            };
        }

        /// <summary>
        /// File attachment methoed returns an attachment type of all extension
        /// </summary>
        /// <returns></returns>
        public Attachment FileAttachment(MediaCardProperties mediaCardPropertiesObj)
        {
            return new Attachment
            {
                Name = mediaCardPropertiesObj.CardTitle,
                ContentType = "application/*",
                ContentUrl = mediaCardPropertiesObj.URL
            };
        }

        public Attachment HeroCard(List<string> optionkey, MediaCardProperties mediaCardPropertiesObj)
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

        public Attachment ThumbnailCard(MediaCardProperties mediaCardPropertiesObj)
        {
            var thumbnailCard = new ThumbnailCard
            {
                Title = mediaCardPropertiesObj.CardTitle,
                Subtitle = mediaCardPropertiesObj.CardSubtitle,
                Text = mediaCardPropertiesObj.CardText,
                Images = new List<CardImage> { new CardImage(mediaCardPropertiesObj.URL) }
            };

            return thumbnailCard.ToAttachment();
        }
    }
}
