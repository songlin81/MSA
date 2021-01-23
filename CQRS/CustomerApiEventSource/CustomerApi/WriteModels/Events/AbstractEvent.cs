using CQRSlite.Events;
using System;

namespace CustomerApi.WriteModels.Events
{
    public class AbstractEvent : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}