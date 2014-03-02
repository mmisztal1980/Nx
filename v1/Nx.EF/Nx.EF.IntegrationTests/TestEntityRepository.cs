using Nx.Logging;
using System;

namespace Nx.EF.IntegrationTests
{
    public class TestEntityRepository : Repository<ModelContext, TestEntity, int>, ITestEntityRepository
    {
        public TestEntityRepository(ILogFactory logFactory, IContextFactory contextFactory)
            : base(logFactory, contextFactory, "TestEntityRepository")
        {
        }

        protected override Func<ModelContext, System.Data.Entity.DbSet<TestEntity>> SourceSelector
        {
            get { return ctx => ctx.TestEntities; }
        }
    }
}