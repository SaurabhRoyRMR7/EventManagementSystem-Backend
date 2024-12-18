using EventManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementAPI.Models;

    public class Message
    {
        public int MessageId { get; set; }
        public int ChatRoomId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Avatar { get; set; } // Optional

        public virtual User Sender { get; set; }
        public virtual ChatRoom ChatRoom { get; set; }
    }


