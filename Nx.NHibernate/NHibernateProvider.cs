using FluentNHibernate;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System.Reflection;

namespace Nx
{
    public class NHibernateProvider
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var cfg = MsSqlConfiguration
                        .MsSql2005
                        .ConnectionString("")
                        .ShowSql()
                        .ConfigureProperties(new NHibernate.Cfg.Configuration());

                    var persistenceModel = new PersistenceModel();

                    //persistenceModel.Conventions. = (prop => prop.Name + "Id");
                    //persistenceModel.Conventions.get = (prop => prop.Name + "Id");
                    persistenceModel.AddMappingsFromAssembly(Assembly.Load(Assembly.GetExecutingAssembly().FullName));
                    persistenceModel.Configure(cfg);
                    _sessionFactory = cfg.BuildSessionFactory();
                }

                return _sessionFactory;
            }
        }
    }
}