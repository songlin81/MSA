using CQRSlite.Events;
using CustomerApi.ReadModels;
using CustomerApi.ReadModels.Repositories;
using NLog;
using System;
using System.Linq;

namespace CustomerApi.WriteModels.Events.Handlers
{
    public class CustomerUpdatedEventHandler : IBusEventHandler
    {
        private readonly CustomerReadModelRepository readModelRepository;
        
        private Logger logger = LogManager.GetLogger("CustomerUpdatedEventHandler");

        public CustomerUpdatedEventHandler(CustomerReadModelRepository readModelRepository)
        {
            this.readModelRepository = readModelRepository;
        }

        public Type HandlerType
        {
            get { return typeof(CustomerUpdatedEvent); }
        }

        public async void Handle(IEvent @event)
        {
            CustomerUpdatedEvent customerUpdatedEvent = (CustomerUpdatedEvent)@event;
            
            Customer customer = await readModelRepository.GetCustomer(@event.Id);

            await readModelRepository.Update(new Customer()
            {
                Id = customerUpdatedEvent.Id,
                Email = customer.Email,
                Name = customerUpdatedEvent.Name != null ? customerUpdatedEvent.Name : customer.Name,
                Age = customerUpdatedEvent.Age != 0 ? customerUpdatedEvent.Age : customer.Age,
                Phones = customerUpdatedEvent.Phones != null ? customerUpdatedEvent.Phones.Select(x =>
                    new Phone()
                    {
                        Type = x.Type,
                        AreaCode = x.AreaCode,
                        Number = x.Number
                    }).ToList() : customer.Phones
            });

            logger.Info("A new CustomerUpdatedEvent has been processed: {0} ({1})", customerUpdatedEvent.Id, customerUpdatedEvent.Version);
        }
    }
}
