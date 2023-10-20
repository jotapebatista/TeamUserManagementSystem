using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamUserManagementSystem.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required(ErrorMessage = "Team name is required")]
        [MaxLength(100)]
        public string TeamName { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<UserTeam>? UserTeams { get; set; } 
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress]
        [MaxLength(50)]
        public string UserEmail { get; set; }

        public int? TeamId { get; set; }

        [ForeignKey("TeamId")]
        public List<UserTeam>? UserTeams { get; set; }
    }

    public class UserTeam
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
