using System.Collections.Generic;
using System.Linq;
using Bogus;
using SalesAPI.Models;

namespace SalesAPI.Infrastructure
{
    public class DataStore
    {
        public List<Order> Orders { get; set; }
        public List<Part> Parts { get; set; }

        public DataStore()
        {
            LoadFakeData();
        }

        private void LoadFakeData()
        {
            Orders = new Faker<Order>()
                 .RuleFor(s => s.ID, f => int.Parse(f.Random.Replace("#####")))
                 .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                 .RuleFor(s => s.LastName, f => f.Name.LastName())
                 .RuleFor(s => s.Email, (f, s) => f.Internet.Email(s.FirstName, s.LastName))
                 .RuleFor(s => s.IsFullTime, f => f.Random.Bool())
                 .Generate(50).ToList();

            string[] partTitles = { "FM001", "FM002", "FM003", "FX332", "FX345", "FX898" };

            Parts = new Faker<Part>()
                        .RuleFor(c => c.ID, f => int.Parse(f.Random.Replace("#####")))
                        .RuleFor(c => c.PartNumber, f => f.Random.Replace("DNM-####"))
                        .RuleFor(c => c.Title, f => f.PickRandom(partTitles))
                        .RuleFor(s => s.Description, f => f.Company.CompanyName())
                        .RuleFor(c => c.StartTime, f => f.Date.Future())
                        .Generate(100).ToList();
        }
    }
}