using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AliceHook.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public virtual List<Webhook> Webhooks { get; set; } = new List<Webhook>();
    }
}