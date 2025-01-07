using System.ComponentModel.DataAnnotations;

namespace BLL.DAL
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string UserName { get; set; }

        [Required]
        [StringLength(25)]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
