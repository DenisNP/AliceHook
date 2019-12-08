namespace AliceHook.Models
{
    public class SimpleResponse
    {
        public string Text { get; set; }
        public string Tts { get; set; }
        public string[] Buttons { get; set; }
        public bool IsAuthorize { get; set; }
    }
}