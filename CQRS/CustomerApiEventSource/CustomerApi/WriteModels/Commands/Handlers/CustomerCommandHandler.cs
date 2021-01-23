using CQRSlite.Commands;
using CQRSlite.Domain;
using CustomerApi.WriteModels.Domain.Aggregates;
using CustomerApi.WriteModels.VOs;
using System;
using System.Linq;

namespace CustomerApi.WriteModels.Commands.Handlers
{
    public class CustomerCommandHandler : ICommandHandler<CreateCustomerCommand>,
                                        ICommandHandler<UpdateCustomerCommand>,
                                        ICommandHandler<DeleteCustomerCommand>
    {
        private readonly ISession _session;
        private NLog.Logger logger = NLog.LogManager.GetLogger("CustomerCommandHandlers");

        public CustomerCommandHandler(ISession session)
        {
            _session = session;
        }

        public void Handle(CreateCustomerCommand command)
        {
            var item = new CustomerAggregate(
                command.Id,
                command.Email,
                command.Name,
                command.Age,
                command.Phones.Select(x => new Phone()
                {
                    Type = x.Type,
                    AreaCode = x.AreaCode,
                    Number = x.Number
                }).ToList(),
                command.ExpectedVersion);

            _session.Add(item);
            _session.Commit();
        }

        private T Get<T>(Guid id, int? expectedVersion = null) where T : AggregateRoot
        {
            try
            {
                return _session.Get<T>(id, expectedVersion);
            }
            catch (Exception e)
            {
                logger.Error("Cannot get object of type {0} with id={1} ({2}) from session", typeof(T), id, expectedVersion);
                throw e;
            }
        }

        public void Handle(UpdateCustomerCommand command)
        {
            logger.Info("Handling UpdateCustomerCommand {0} ({1})", command.Id, command.ExpectedVersion);

            CustomerAggregate item = Get<CustomerAggregate>(command.Id);
            item.Update(
                command.Id,
                command.Name,
                command.Age,
                command.Phones.Select(x => new Phone()
                {
                    Type = x.Type,
                    AreaCode = x.AreaCode,
                    Number = x.Number
                }).ToList(),
                command.ExpectedVersion);
            _session.Commit();
        }

        public void Handle(DeleteCustomerCommand command)
        {
            CustomerAggregate item = Get<CustomerAggregate>(command.Id);
            item.Delete();
            _session.Commit();
        }
    }
}
