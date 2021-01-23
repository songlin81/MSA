using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace CustomerApi.ReadModels
{
	public class Customer
    {
		[BsonElement("Id")]
		public Guid Id { get; set; }
		[BsonElement("Email")]
		public string Email { get; set; }
		[BsonElement("Name")]
		public string Name { get; set; }
		[BsonElement("Age")]
		public int Age { get; set; }
		[BsonElement("Phones")]
		public List<Phone> Phones;
	}
}
