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
        public string Type = "text_notice";
        
        [JsonProperty("source")]
        public CardSource Source { get; set; }
        
        [JsonProperty("main_title")]
        public CardDesc Title { get; set; }
        
        [JsonProperty("emphasis_content")]
        public CardDesc EmphasisContent { get; set; }
        
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
    }
}