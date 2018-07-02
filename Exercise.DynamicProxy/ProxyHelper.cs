using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Exercise.DynamicProxy
{
    public class ProxyHelper
    {
        public const string FieldName = "__interceptors";
        public static FieldInfo GetInfo(object obj, string fieldName) => obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Single(a => a.Name == fieldName);
        public static IInterceptor[] GetInterceptorsField(object service) => GetInfo(service, FieldName).GetValue(service) as IInterceptor[];

        public static void ExcudeInterceptors(object service, params Type[] interceptorTypes2exclude)
        {
            var fieldVal = GetInterceptorsField(service);

            var newInterceptors = fieldVal.Where(x => !interceptorTypes2exclude.Contains(x.GetType())).ToArray();

            GetInfo(service, FieldName).SetValue(service, newInterceptors);
        }

        public static void ExcudeInterceptors(object service, params IInterceptor[] interceptorTypes2exclude)
        {
            var fieldVal = GetInterceptorsField(service);

            var newInterceptors = fieldVal.Except(interceptorTypes2exclude);

            GetInfo(service, FieldName).SetValue(service, newInterceptors.ToArray());
        }

        public static void AddInterceptor<T>(object service, int position = -1) where T : IInterceptor, new()
        {
            AddInterceptor(service, new T(), position);
        }

        public static void AddInterceptor(object service,Func<IInterceptor> constructor, int position = -1)
        {
            AddInterceptor(service, constructor(), position);
        }

        public static void AddInterceptor(object service, IInterceptor interceptor, int position = -1)
        {
            var newInterceptors = GetInterceptorsField(service).ToList();

            if (position == -1)
            {
                newInterceptors.Add(interceptor);
            }
            else
            {
                newInterceptors.Insert(position, interceptor);
            }

            GetInfo(service, FieldName).SetValue(service, newInterceptors.ToArray());
        }
    }
}
