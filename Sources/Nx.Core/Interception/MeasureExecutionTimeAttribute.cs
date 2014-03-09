using Ninject.Parameters;

namespace Nx.Interception
{
    public class MeasureExecutionTimeAttribute : InterceptorAttribute
    {
        public MeasureExecutionTimeAttribute(string loggerName)
            : base(typeof(MeasureExecutionTimeInterceptor), new IParameter[]
            {
                new ConstructorArgument("loggerName", loggerName),
            })
        {
        }
    }
}