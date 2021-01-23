using CQRSlite.Events;
using System;

namespace CustomerApi.WriteModels.Events.Handlers
{
    public interface IBusEventHandler
    {
        Type HandlerType { get; }
        void Handle(IEvent @event);
    }
}
