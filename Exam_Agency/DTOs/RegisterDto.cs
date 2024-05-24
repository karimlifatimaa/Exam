using System.ComponentModel.DataAnnotations;

namespace Exam_Agency.DTOs
{
    public class RegisterDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Surname { get; set; }
        [Required]
        
        [MaxLength(100)]
        public string Username { get; set; }
        [Required]
        [MinLength(7)]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        [DataType(DataType.Password),Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
