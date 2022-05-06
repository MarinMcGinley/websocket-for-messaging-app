using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class User : BaseEntity
    {
        public User(
            string Email,
            string Password,
            string FirstName,
            string LastName
        ) {
            this.Email = Email;
            this.Password = Password;
            this.FirstName = FirstName;
            this.LastName = LastName;
        }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}