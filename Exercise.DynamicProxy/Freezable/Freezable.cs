using System;
using System.Collections.Generic;
using System.Linq;
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
            var interceptor = new FreezableInterceptor();

            var proxy = _generator.CreateClassProxy<T>(new CallLoggingInterceptor(), new FreezableInterceptor());

            _freezables.Add(proxy, interceptor);

            return proxy;
        }

        public static bool RegisterFreezable(object freezable)
        {
            var interceptor = ProxyHelper.GetInterceptorsField(freezable).FirstOrDefault(a => a.GetType() == typeof(FreezableInterceptor));

            if (interceptor is IFreezable freezableInterceptor)
            {
                try
                {
                    _freezables.Add(freezable, freezableInterceptor);

                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;

        }
    }
}
