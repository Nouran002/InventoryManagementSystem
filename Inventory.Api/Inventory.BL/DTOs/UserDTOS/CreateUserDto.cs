using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.DTOs.UserDTOS
{
    public class CreateUserDto: UserDto
    {
        public string Password { get; set; }

    }
}
