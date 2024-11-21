using System.ComponentModel.DataAnnotations;
using Reddit.Models;

namespace Reddit.Dtos
{
    public class UserDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public User CreateUser() {
             return new User { Name = Name, Email = Email };
        }
    }
}
