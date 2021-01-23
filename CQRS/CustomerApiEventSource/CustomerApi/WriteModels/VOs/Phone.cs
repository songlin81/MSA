using CustomerApi.Commons;
using System.Runtime.Serialization;

namespace CustomerApi.WriteModels.VOs
{
	[DataContract]
	public class Phone
	{
		[DataMember]
		public PhoneType Type { get; set; }
		[DataMember]
		public int AreaCode { get; set; }
		[DataMember]
		public int Number { get; set; }
	}
}
