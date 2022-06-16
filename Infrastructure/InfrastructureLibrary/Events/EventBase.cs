using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLibrary.Events
{
    public class EventBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public EventBase()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }

        public EventBase(Guid id, DateTime createdDate)
        {
            Id = id;
            CreatedDate = createdDate;
        }
    }
}
