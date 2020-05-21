using System;
using System.Collections.Generic;

namespace api.audit.web.example.Models
{
	public class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IdClient { get; set; }
        public Referrer Referrer { get; set; }
        public IList<EventVisitor> Visitors { get; set; }
        public EventScheduling Scheduling { get; set; }
        public EventCommonArea Area { get; set; }
        public EventStatus Status { get; set; }
        public DateTime Created { get; set; }
        public string Creator { get; set; }

        public Event()
        {

            Referrer = new Referrer();
            Scheduling = new EventScheduling();
        }

        public void SetClient(string idClient)
        {
            IdClient = idClient;
        }

        public void SetNewEvent(string creator)
        {
            Status = EventStatus.Pending;
            Created = DateTime.Now;
            Creator = creator;
        }
    }
}

