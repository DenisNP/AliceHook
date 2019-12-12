using System.Collections.Generic;

namespace AliceHook.Models
{
    public class AliceEmpty
    {
    }

    public class Interfaces
    {
        public AliceEmpty Screen { get; set; }
    }

    public class Meta
    {
        public string Locale { get; set; }
        public string Timezone { get; set; }
        public string ClientId { get; set; }
        public Interfaces Interfaces { get; set; }
    }

    public class Markup
    {
        public bool DangerousContext { get; set; }
    }

    public class Tokens
    {
        public int Start { get; set; }
        public int End { get; set; }
    }

    public class Entity
    {
        public Tokens Tokens { get; set; }
        public string Type { get; set; }
        public object Value { get; set; }
    }

    public class Nlu
    {
        public List<string> Tokens { get; set; }
        public List<Entity> Entities { get; set; }
    }

    public class Request
    {
        public string Command { get; set; }
        public string OriginalUtterance { get; set; }
        public string Type { get; set; }
        public Markup Markup { get; set; }
        public Dictionary<string, string> Payload { get; set; }
        public Nlu Nlu { get; set; }
    }

    public class Session
    {
        public bool New { get; set; }
        public int MessageId { get; set; }
        public string SessionId { get; set; }
        public string SkillId { get; set; }
        public string UserId { get; set; }
    }

    public class AliceRequest
    {
        public Meta Meta { get; set; }
        public AliceEmpty AccountLinkingCompleteEvent { get; set; }
        public Request Request { get; set; }
        public Session Session { get; set; }
        public string Version { get; set; }

        public bool HasScreen()
        {
            return Meta?.Interfaces?.Screen != null;
        }
        
        public bool IsAccountLinking()
        {
            return AccountLinkingCompleteEvent != null;
        }

        public bool IsPing()
        {
            if (Request == null)
            {
                return false;
            }

            return Request.Command.ToLower() == "ping";
        }
    }
}