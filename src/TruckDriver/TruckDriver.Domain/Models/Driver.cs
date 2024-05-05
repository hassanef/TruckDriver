using System.Text.Json;
using System.Text.Json.Serialization;

namespace TruckDriver.Domain.Models
{
    public class Driver
    {
        public string Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Location { get; private set; }
        public Driver(string id, string firstName, string lastName, string location)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Location = location;
        }
    }
}
