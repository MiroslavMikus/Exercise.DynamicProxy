using Castle.DynamicProxy;

namespace Exercise.DynamicProxy.Test
{
    public class CounterInterceptor : IInterceptor
    {
        public int CallsCount { get; set; }

        public void Intercept(IInvocation invocation)
        {
            CallsCount++;

            invocation.Proceed();
        }
    }
}
