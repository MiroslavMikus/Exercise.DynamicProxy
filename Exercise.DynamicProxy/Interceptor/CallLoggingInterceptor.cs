using System;
using Castle.DynamicProxy;

namespace Exercise.DynamicProxy
{
    public class CallLoggingInterceptor : IInterceptor
    {
        private int indentation;

        public void Intercept(IInvocation invocation)
        {
            try
            {
                indentation++;
                Console.WriteLine(string.Format("{0} ! {1}", new string(' ', indentation), invocation.Method.Name));
                invocation.Proceed();
            }
            finally
            {
                indentation--;
            }
        }
    }


}
