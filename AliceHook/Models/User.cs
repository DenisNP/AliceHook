using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AliceHook.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public List<Webhook> Webhooks { get; set; }
    }
}