using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Models
{
    public class DisplayMessage: ChatMessage
    {
        public string FullName { get; set; }
        public string DisplayId { get; set; }

    }
}
