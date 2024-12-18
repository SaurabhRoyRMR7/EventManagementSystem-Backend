using EventManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementAPI.Models;

    public class ChatRoom
    {
        public int ChatRoomId { get; set; }
        public string Name { get; set; }
        public int EventId { get; set; }

        // Navigation property to the Event (foreign key relation to an Event, assuming Event exists in your model)
        public virtual Event Event { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<User> Participants { get; set; }
        public Message? LastMessage { get; set; } // Optional last message
    }


