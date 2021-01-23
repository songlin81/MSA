using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace CustomerApi.ReadModels.Repositories
{
    public class CustomerReadModelRepository
	{
		private const string _customerDB = "CustomerDB";
		private const string _customerCollection = "Customers";
		private IMongoDatabase _db;

		public CustomerReadModelRepository()
		{
			MongoClient _client = new MongoClient("mongodb://localhost:27017");
			_db = _client.GetDatabase(_customerDB);

			//_db.DropCollection(_customerCollection);
			//_db.CreateCollection(_customerCollection);
		}

		public Task<List<Customer>> GetCustomers()
		{
			return Task.Run(() =>
			{
				return _db.GetCollection<Customer>(_customerCollection).Find(_ => true).ToList();
			});
		}

		public Task<Customer> GetCustomer(Guid id)
		{
			return Task.Run(() =>
			{
				return _db.GetCollection<Customer>(_customerCollection).Find(customer => customer.Id.Equals(id)).SingleOrDefault();
			});
		}

		public Task<List<Customer>> GetCustomerByEmail(string email)
		{
			return Task.Run(() =>
			{
				return _db.GetCollection<Customer>(_customerCollection).Find(customer => customer.Email == email).ToList();
			});
        }

		public Task<bool> Create(Customer customer)
		{
			return Task.Run(() =>
			{
				_db.GetCollection<Customer>(_customerCollection).InsertOne(customer);
				return true;
			});
		}

		public Task<bool> Update(Customer customer)
		{
			return Task.Run(() =>
			{
				var filter = Builders<Customer>.Filter.Where(_ => _.Id == customer.Id);

				_db.GetCollection<Customer>(_customerCollection).ReplaceOne(filter, customer);

                return true;
			});
		}

		public Task<bool> Remove(Guid id)
		{
			return Task.Run(() =>
			{
				var filter = Builders<Customer>.Filter.Where(_ => _.Id.Equals(id));
				var operation = _db.GetCollection<Customer>(_customerCollection).DeleteOne(filter);

                return true;
			});
		}
	}
}