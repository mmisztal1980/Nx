using System.Data.Entity;

namespace Nx.EF.IntegrationTests
{
    public class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(string connectionStringName)
            : base(connectionStringName)
        {
        }

        public DbSet<TestEntity> TestEntities { get; set; }
    }
}