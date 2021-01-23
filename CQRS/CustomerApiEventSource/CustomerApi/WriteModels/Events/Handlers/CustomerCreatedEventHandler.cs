using CQRSlite.Events;
using CustomerApi.ReadModels;
using CustomerApi.ReadModels.Repositories;
using NLog;
using System;
using System.Linq;

namespace CustomerApi.WriteModels.Events.Handlers
{
    public class CustomerCreatedEventHandler : IBusEventHandler
    {
        private readonly CustomerReadModelRepository readModelRepository;

        private Logger logger = LogManager.GetLogger("CustomerCreatedEventHandler");

        public CustomerCreatedEventHandler(CustomerReadModelRepository readModelRepository)
        {
            this.readModelRepository = readModelRepository;
        }

        public Type HandlerType
        {
            get { return typeof(CustomerCreatedEvent); }
        }

        public async void Handle(IEvent @event)
        {
            CustomerCreatedEvent customerCreatedEvent = (CustomerCreatedEvent)@event;
            
            await readModelRepository.Create(new Customer()
            {
                Id = customerCreatedEvent.Id,
                Email = customerCreatedEvent.Email,
                Name = customerCreatedEvent.Name,
                Age = customerCreatedEvent.Age,
                Phones = customerCreatedEvent.Phones.Select(x =>
                    new Phone()
                    {
                        Type = x.Type,
                        AreaCode = x.AreaCode,
                        Number = x.Number
                    }).ToList()
            });

            logger.Info("A new CustomerCreatedEvent has been processed: {0} ({1})", customerCreatedEvent.Id, customerCreatedEvent.Version);
        }
    }
}
