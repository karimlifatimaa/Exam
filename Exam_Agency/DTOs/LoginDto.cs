using System.ComponentModel.DataAnnotations;

namespace Exam_Agency.DTOs
{
    public class LoginDto
    {
        [Required]
        [MaxLength(100)]    
        public string UsernameOrEmail { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemember { get; set; }
    }
}
