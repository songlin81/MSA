using CQRSlite.Commands;
using System;
using System.Runtime.Serialization;

namespace CustomerApi.WriteModels.Commands
{
    [DataContract]
    [KnownType(typeof(CreateCustomerCommand))]
    [KnownType(typeof(UpdateCustomerCommand))]
    [KnownType(typeof(DeleteCustomerCommand))]
    public abstract class Command : ICommand
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public int ExpectedVersion { get; set; }
    }
}
