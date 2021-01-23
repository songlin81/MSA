using CQRSlite.Events;
using CustomerApi.ReadModels.Repositories;
using NLog;
using System;

namespace CustomerApi.WriteModels.Events.Handlers
{
    public class CustomerDeletedEventHandler : IBusEventHandler
    {
        private readonly CustomerReadModelRepository readModelRepository;

        private Logger logger = LogManager.GetLogger("CustomerDeletedEventHandler");

        public CustomerDeletedEventHandler(CustomerReadModelRepository readModelRepository)
        {
            this.readModelRepository = readModelRepository;
        }

        public Type HandlerType
        {
            get { return typeof(CustomerDeletedEvent); }
        }

        public async void Handle(IEvent @event)
        {
            CustomerDeletedEvent customerDeletedEvent = (CustomerDeletedEvent)@event;
            
            await readModelRepository.Remove(customerDeletedEvent.Id);

            logger.Info("A new CustomerDeletedEvent has been processed: {0} ({1})", customerDeletedEvent.Id, customerDeletedEvent.Version);
        }
    }
}
