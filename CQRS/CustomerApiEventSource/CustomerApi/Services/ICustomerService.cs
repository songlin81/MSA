using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using CustomerApi.ReadModels;
using CustomerApi.WriteModels.Commands;

namespace CustomerApi.Services
{
    [ServiceContract]
    public interface ICustomerService
    {
        [OperationContract]
        Task<bool> IssueCommandAsync(Command cmd);

        [OperationContract]
        Task<List<Customer>> GetAllCustomersAsync();

        [OperationContract]
        Task<Customer> GetCustomerAsync(Guid custId);

        [OperationContract]
        Task<List<Customer>> GetCustomersByEmailAsync(string email);

    }
}
