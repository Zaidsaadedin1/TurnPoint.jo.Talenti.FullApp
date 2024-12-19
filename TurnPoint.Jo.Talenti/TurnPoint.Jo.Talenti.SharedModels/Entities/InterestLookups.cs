using System.Text.Json.Serialization;
using TurnPoint.Jo.Talenti.SharedModels.Entities;

namespace TurnPoint.Jo.APIs.Entities
{
    public class InterestsLookup
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public List<InterestsLookupUser> Users { get; set; } = new List<InterestsLookupUser>();

    }
}
