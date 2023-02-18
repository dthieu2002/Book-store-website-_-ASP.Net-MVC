using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BTL_ASPDotNet2022.Models
{
    public class Account
    {
        [Key]
        [Required(ErrorMessage = "Don't leave username blank!")] // Modified the message will send back while text is blank
        [StringLength(100)]
        public string? Username { get; set; }

        [Required]
        [StringLength(100)]
        public string? Password { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Full Name")]
        public string? FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }

        [Required]
        [StringLength(10)]
        public string? Gender { get; set; }

        [Required]
        [StringLength(200)]
        public string? Address { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }


        [StringLength(10)]
        public string? Role { get; set; } // "master" || "staff" || "customer"

        [DisplayName("Register Date")]
        [DataType(DataType.Date)]
        public DateTime? RegisterDate { get; set; }



        // Not necessary constructor
        // Note: If have construct more than 0 parameter, must declare construct zero parameter
        
        public Account()
        {

        }

        public Account(string Username, string Password, string FullName, DateTime BirthDay, string Gender, string Address, string Phone, string Role, DateTime RegisterDate)
        {
            this.Username = Username;
            this.Password = Password;
            this.FullName = FullName;
            this.BirthDay = BirthDay;
            this.Gender = Gender;
            this.Address = Address;
            this.Phone = Phone;
            this.Role = Role;
            this.RegisterDate = RegisterDate;
        }
    }
}
