using CustomerApi.WriteModels.VOs;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CustomerApi.WriteModels.Commands
{
	[DataContract]
	public class UpdateCustomerCommand : Command
	{
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public int Age { get; set; }
		[DataMember]
		public List<Phone> Phones { get; set; }

	}
}
