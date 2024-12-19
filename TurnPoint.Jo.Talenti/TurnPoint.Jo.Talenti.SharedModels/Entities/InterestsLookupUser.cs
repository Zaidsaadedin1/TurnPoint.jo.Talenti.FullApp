using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnPoint.Jo.APIs.Entities;

namespace TurnPoint.Jo.Talenti.SharedModels.Entities
{
    public class InterestsLookupUser
    {  
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int InterestId { get; set; }
        public InterestsLookup Interest { get; set; } = null!;
    }

}
