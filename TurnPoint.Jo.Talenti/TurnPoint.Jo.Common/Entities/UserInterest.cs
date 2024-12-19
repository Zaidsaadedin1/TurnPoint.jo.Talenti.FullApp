using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnPoint.Jo.Common.Entities
{
    public class UserInterest
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int InterestId { get; set; }
        public InterestsLookup Interest { get; set; } = null!;
    }
}
