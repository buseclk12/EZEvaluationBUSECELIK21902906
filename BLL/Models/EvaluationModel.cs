using System.ComponentModel;
using BLL.DAL;

namespace BLL.Models
{
    public class EvaluationModel
    {
        public Evaluation Record { get; set; }

        [DisplayName("Title")]
        public string Title => Record.Title;

        [DisplayName("Score")]
        public string Score => Record.Score.ToString();

        //postgre de böyle
        [DisplayName("Date")]
        public string Date => Record.Date.HasValue 
            ? Record.Date.Value.ToUniversalTime().ToString("MM/dd/yyyy HH:mm:ss") 
            : "Unknown";

        [DisplayName("Description")]
        public string Description => Record.Description;

        [DisplayName("User")]
        public string User => Record.User?.UserName;

        // [DisplayName("Evaluateds")]
        // public string Evaluateds => string.Join(", ", Record.EvaluatedEvaluations.Select(e => e.Evaluated.Name));
        //
        // Evaluated bilgilerini göstermek için
        [DisplayName("Evaluateds")]
        public string Evaluateds => string.Join(", ", Record.EvaluatedEvaluations.Select(e => $"{e.Evaluated.Name} {e.Evaluated.Surname}"));


        [DisplayName("Evaluated Count")]
        public int EvaluatedsCount => Record.EvaluatedEvaluations.Count;
        
        public List<int> SelectedEvaluatedIds { get; set; } = new List<int>();

        public List<int> EvaluatedIds => Record.EvaluatedEvaluations.Select(e => e.EvaluatedId).ToList();
    }
}