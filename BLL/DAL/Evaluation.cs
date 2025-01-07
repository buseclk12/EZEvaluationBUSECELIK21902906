using System.ComponentModel.DataAnnotations;

namespace BLL.DAL
{
    public class Evaluation
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; }

        public decimal Score { get; set; }

        public DateTime? Date { get; set; } = DateTime.UtcNow; // Default to UTC

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<EvaluatedEvaluation> EvaluatedEvaluations { get; set; } = new List<EvaluatedEvaluation>();
    }
}