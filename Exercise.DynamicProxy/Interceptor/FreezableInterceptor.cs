using System;
using Castle.DynamicProxy;

namespace Exercise.DynamicProxy
{
    public class FreezableInterceptor : IInterceptor, IFreezable
    {
        private FreezableInterceptor(){}

        public static FreezableInterceptor Create()
        {
            return new FreezableInterceptor();
        }

        public bool IsFrozen { get; private set; }

        public void Freeze() => IsFrozen = true;

        public void Intercept(IInvocation invocation)
        {
            if (IsFrozen && invocation.Method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Object is Frozen");
            }
            invocation.Proceed();
        }
    }


}
