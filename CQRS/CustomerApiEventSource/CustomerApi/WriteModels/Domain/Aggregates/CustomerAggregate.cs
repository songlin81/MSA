using CQRSlite.Domain;
using CustomerApi.WriteModels.Events;
using CustomerApi.WriteModels.VOs;
using System;
using System.Collections.Generic;

namespace CustomerApi.WriteModels.Domain.Aggregates
{
    public class CustomerAggregate : AggregateRoot
    {
        private string email;
        private string name;
        private int age;
        private List<Phone> phones;

        private void Apply(CustomerCreatedEvent e)
        {
            Version = e.Version++;
            email = e.Email;
            age = e.Age;
            phones = e.Phones;
        }

        private void Apply(CustomerUpdatedEvent e)
        {
            Version = e.Version++;
            name = e.Name;
            age = e.Age;
            phones = e.Phones;
        }

        private void Apply(CustomerDeletedEvent e)
        {
            Version = e.Version++;
        }

        private CustomerAggregate() { }

        public CustomerAggregate(Guid id, string email, string name, int age, List<Phone> phones, int version)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("email");
            }
            else if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name");
            }
            else if (age == 0)
            {
                throw new ArgumentException("age");
            }
            else if (phones == null || phones.Count == 0)
            {
                throw new ArgumentException("phones");
            }

            Id = id;
            ApplyChange(new CustomerCreatedEvent(id, email, name, age, phones, version));
        }

        public void Update(Guid id, string name, int age, List<Phone> phones, int version)
        {
            ApplyChange(new CustomerUpdatedEvent(id, name, age, phones, version));
        }

        public void Delete()
        {
            ApplyChange(new CustomerDeletedEvent(Id, Version));
        }
    }
}
