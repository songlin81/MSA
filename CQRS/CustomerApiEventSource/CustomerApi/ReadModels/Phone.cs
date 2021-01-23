using CustomerApi.Commons;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerApi.ReadModels
{
	public partial class Phone
    {
		[BsonElement("Type")]
		public PhoneType Type { get; set; }
		[BsonElement("AreaCode")]
		public int AreaCode { get; set; }
		[BsonElement("Number")]
		public int Number { get; set; }
	}
}
