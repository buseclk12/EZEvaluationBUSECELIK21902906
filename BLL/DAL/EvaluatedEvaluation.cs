namespace BLL.DAL
{
    public class EvaluatedEvaluation
    {
        public int Id { get; set; }

        public int EvaluatedId { get; set; }
        public Evaluated Evaluated { get; set; }

        
        public int EvaluationId { get; set; }
        public Evaluation Evaluation { get; set; }
    }
}