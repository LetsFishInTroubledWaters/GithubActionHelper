using Newtonsoft.Json;

namespace GithubActionHelper.Client;

public class CardNotificationRequest
{
    [JsonProperty("msgtype")] 
    public string Type = "template_card";

    [JsonProperty("template_card")]
    public CardTemplate Template { get; set; }
    
    public class CardTemplate
    {
        [JsonProperty("card_type")]
        public string Type = "news_notice";
        
        [JsonProperty("source")]
        public CardSource Source { get; set; }
        
        [JsonProperty("main_title")]
        public CardDesc Title { get; set; }

        [JsonProperty("card_image")] 
        public Image CardImage = new();
        
        [JsonProperty("image_text_area")]
        public ImageText ImageTextArea { get; set; }
        
        [JsonProperty("vertical_content_list")]
        public List<CardDesc> VerticalContentList { get; set; }
        
        [JsonProperty("horizontal_content_list")]
        public List<CardContent> Content { get; set; }
        
        [JsonProperty("jump_list")]
        public List<Jump> JumpList { get; set; }
        
        [JsonProperty("card_action")]
        public Action CardAction { get; set; }
        public class CardSource
        {
            [JsonProperty("icon_url")]
            public string IconUrl { get; set; }
            
            [JsonProperty("desc")]
            public string Desc { get; set; }

            [JsonProperty("desc_color")]
            public long Color;
        }

        public class CardDesc
        {
            [JsonProperty("title")]
            public string Title { get; set; }
            
            [JsonProperty("desc")]
            public string Desc { get; set; }
        }

        public class CardContent
        {
            [JsonProperty("keyname")]
            public string KeyName { get; set; }
            
            [JsonProperty("value")]
            public string Value { get; set; }
        }

        public class Jump
        {
            [JsonProperty("type")]
            public long Type = 1;
            
            [JsonProperty("title")]
            public string Title { get; set; }
            
            [JsonProperty("url")]
            public string Url { get; set; }
        }

        public class Action
        {
            [JsonProperty("type")]
            public long Type = 1;

            [JsonProperty("url")]
            public string Url { get; set; }
        }

        public class Image
        {
            [JsonProperty("url")]
            public string Url =
                "https://i.kym-cdn.com/entries/icons/original/000/002/144/You_Shall_Not_Pass!_0-1_screenshot.jpg";

            [JsonProperty("aspect_ratio")] 
            public double Ratio = 1.75;
        }

        public class ImageText
        {
            [JsonProperty("type")] 
            public long Type = 1;

            [JsonProperty("image_url")] 
            public string ImageUrl = "https://cdn.icon-icons.com/icons2/317/PNG/512/sign-error-icon_34362.png";
            
            [JsonProperty("url")] 
            public string Url { get; set; }
            
            [JsonProperty("title")]
            public string Title { get; set; }
            
            [JsonProperty("desc")]
            public string Desc { get; set; }
        }
    }
}