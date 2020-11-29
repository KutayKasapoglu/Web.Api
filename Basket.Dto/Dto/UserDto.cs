
using System;

namespace Basket.Dto.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Username { get; set; }
    }
}
