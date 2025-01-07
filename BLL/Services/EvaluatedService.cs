using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using System.Linq;

namespace BLL.Services
{
    public class EvaluatedService : Service, IService<Evaluated, EvaluatedModel>
    {
        public EvaluatedService(Db db) : base(db)
        {
        }

        public IQueryable<EvaluatedModel> Query()
        {
            return _db.Evaluateds.Select(e => new EvaluatedModel { Record = e });
        }

        public Service Create(Evaluated record)
        {
            if (_db.Evaluateds.Any(e => e.Name.ToUpper() == record.Name.ToUpper().Trim() && e.Surname.ToUpper() == record.Surname.ToUpper().Trim()))
                return Error("An evaluated person with the same name and surname already exists!");

            record.Name = record.Name.Trim();
            record.Surname = record.Surname.Trim();
            _db.Evaluateds.Add(record);
            _db.SaveChanges();
            return Success("Evaluated person created successfully.");
        }

        public Service Update(Evaluated record)
        {
            throw new NotImplementedException();
        }

        public Service Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}