using CustomerApi.Commons;
using CustomerApi.ReadModels;
using CustomerApi.ReadModels.Repositories;
using CustomerApi.WriteModels.Commands;
using CustomerApi.WriteModels.Commands.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CustomerApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerCommandHandler _commandHandlers;
        private readonly CustomerReadModelRepository _readModelRepository;

        public CustomerService(CustomerCommandHandler commandHandlers, CustomerReadModelRepository readModelRepository)
        {
            this._commandHandlers = commandHandlers;
            this._readModelRepository = readModelRepository;
        }

        public async Task<bool> IssueCommandAsync(Command cmd)
        {
            await Task.Run(() =>
            {
                var method = (from meth in typeof(CustomerCommandHandler)
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                              let @params = meth.GetParameters()
                              where @params.Count() == 1 && @params[0].ParameterType == cmd.GetType()
                              select meth).FirstOrDefault();

                if (method == null)
                {
                    var name = cmd.GetType().Name;
                    throw new ServiceException(string.Format("Command handler of {0} not found", name));
                }

                method.Invoke(_commandHandlers, new[] { cmd });
            });

            return true;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _readModelRepository.GetCustomers();
        }

        public async Task<Customer> GetCustomerAsync(Guid orderId)
        {
            return await _readModelRepository.GetCustomer(orderId);
        }

        public async Task<List<Customer>> GetCustomersByEmailAsync(string email)
        {
            return await _readModelRepository.GetCustomerByEmail(email);
        }

    }
}
