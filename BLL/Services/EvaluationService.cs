using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class EvaluationService : Service, IService<Evaluation, EvaluationModel>
    {
        public EvaluationService(Db db) : base(db)
        {
        }

        public IQueryable<EvaluationModel> Query()
        {
            return _db.Evaluations
                .Include(e => e.User)
                .Include(e => e.EvaluatedEvaluations)
                .ThenInclude(ee => ee.Evaluated)
                .Select(e => new EvaluationModel { Record = e });
        }

        
        public Service Create(Evaluation record)
        {
            record.Date = record.Date.HasValue ? record.Date.Value.ToUniversalTime() : DateTime.UtcNow;
            _db.Evaluations.Add(record);
            _db.SaveChanges();
            return Success("Evaluation created successfully.");
        }

        public Service Update(Evaluation record)
        {
            var entity = _db.Evaluations.Find(record.Id);
            if (entity == null) return Error("Evaluation not found!");

            entity.Title = record.Title;
            entity.Score = record.Score;
            entity.Date = record.Date.HasValue ? record.Date.Value.ToUniversalTime() : DateTime.UtcNow;
            entity.Description = record.Description;
            _db.Evaluations.Update(entity);
            _db.SaveChanges();
            return Success("Evaluation updated successfully.");
        }

        public Service Delete(int id)
        {
            var entity = _db.Evaluations.Find(id);
            if (entity == null) return Error("Evaluation not found!");

            _db.Evaluations.Remove(entity);
            _db.SaveChanges();
            return Success("Evaluation deleted successfully.");
        }
    }
}