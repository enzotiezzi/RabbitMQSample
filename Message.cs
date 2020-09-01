using System;
using System.Collections.Generic;
using System.Text;

namespace LetsTalkToCBYK
{
    class Message
    {
        public Guid Id { get; set; }
        public double TimeStamp { get; set; }
        public string Content { get; set; }
        public string Sender { get; set; }
    }
}
