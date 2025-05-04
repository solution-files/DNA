namespace DNA3.Models {

    public class OpenAiChatResponse {
        public Choice[] Choices { get; set; }
    }

    public class Choice {
        public Message Message { get; set; }
    }

    public class Message {
        public string Role { get; set; }
        public string Content { get; set; }
    }

}
