using AliceHook.Models;

namespace AliceHook.Engine
{
    public class State
    {
        public User User { get; set; }
        public Step Step { get; set; } = Step.None;
        public string TempUrl { get; set; }
    }

    public enum Step
    {
        None,
        AwaitForUrl,
        AwaitForKeyword
    }
}