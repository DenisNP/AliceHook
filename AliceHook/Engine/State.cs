using AliceHook.Models;

namespace AliceHook.Engine
{
    public class State
    {
        public User User { get; set; }
        public Step Step { get; set; } = Step.None;
        public string TempUrl { get; set; }
        public string LastResult { get; set; }
        public string LastError { get; set; }

        public void Clear()
        {
            Step = Step.None;
            TempUrl = "";
            ClearLastResult();
        }

        public bool HasLastResult()
        {
            return !LastResult.IsNullOrEmpty() || !LastError.IsNullOrEmpty();
        }

        public void ClearLastResult()
        {
            LastResult = "";
            LastError = "";
        }
    }

    public enum Step
    {
        None,
        AwaitForUrl,
        AwaitForKeyword,
        Examples
    }
}