using System.Collections.Generic;
using System.Linq;
using Bogus;
using SalesService.Models;

namespace SalesService.Infrastructure
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

            string[] partTitles = { "FH001", "FH002", "FMX11", "FMX22", "FMM11", "FMM22" };

            Parts = new Faker<Part>()
                        .RuleFor(c => c.ID, f => int.Parse(f.Random.Replace("#####")))
                        .RuleFor(c => c.PartNumber, f => f.Random.Replace("DNM-####"))
                        .RuleFor(s => s.Description, f => f.Company.CompanyName())
                        .RuleFor(c => c.Title, f => f.PickRandom(partTitles))
                        .RuleFor(c => c.StartTime, f => f.Date.Future())
                        .Generate(100).ToList();
        }
    }
}
