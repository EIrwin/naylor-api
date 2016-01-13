using System;
using System.Collections.Generic;

namespace Naylor.Data
{
    public class Meeting
    {
        public Guid Id { get; set; }

        public DateTime StartUtc { get; set; }

        public DateTime EndUtc { get; set; }

        public List<User> Attendees { get; set; }

        public bool Active { get; set; }

        public decimal Total { get; set; }

        public Meeting()
        {
            Attendees = new List<User>();
        }
    }
}
