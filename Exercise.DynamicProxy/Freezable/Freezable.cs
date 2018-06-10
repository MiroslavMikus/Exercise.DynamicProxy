using System.Collections.Generic;
using Castle.DynamicProxy;

namespace Exercise.DynamicProxy
{
    public static class Freezable
    {
        public static readonly IDictionary<object, IFreezable> _freezables = new Dictionary<object, IFreezable>();

        private static readonly ProxyGenerator _generator = new ProxyGenerator();

        public static bool IsFreezable(object obj) => obj != null && _freezables.ContainsKey(obj);

        public static void Freeze(object freezable)
        {
            if (!IsFreezable(freezable))
            {
                throw new System.Exception("not freezable object: " + freezable.GetHashCode());
            }
            _freezables[freezable].Freeze();
        }

        public static bool IsFrozen(object freezable) => IsFreezable(freezable) && _freezables[freezable].IsFrozen;

        public static T MakeFreezable<T>() where T : class, new()
        {
            var interceptor = FreezableInterceptor.Create();
            var proxy = _generator.CreateClassProxy<T>(new CallLoggingInterceptor(), interceptor);
            _freezables.Add(proxy, interceptor);
            return proxy;
        }
    }
}
