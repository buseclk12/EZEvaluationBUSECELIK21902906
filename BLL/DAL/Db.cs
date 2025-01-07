using Microsoft.EntityFrameworkCore;

namespace BLL.DAL
{
    public class Db : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        
        public DbSet<Evaluation> Evaluations { get; set; }

        public DbSet<Evaluated> Evaluateds { get; set; }


        public Db(DbContextOptions options) : base(options)
        {
        }
    }
}
