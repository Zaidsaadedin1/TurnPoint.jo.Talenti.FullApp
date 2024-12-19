using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnPoint.Jo.Common.Common.UserDtos
{
    public class LoginUserDto
    {
        public string EmailOrPhone { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
