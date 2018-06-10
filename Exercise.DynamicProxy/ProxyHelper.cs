using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Exercise.DynamicProxy
{
    public class ProxyHelper
    {
        public const string FieldName = "__interceptors";
        public static FieldInfo GetInfo(object obj, string fieldName) => obj.GetType().GetField(fieldName);
        public static IInterceptor[] GetInterceptorsField(object service) => GetInfo(service, FieldName).GetValue(service) as IInterceptor[];

        public static void ExcudeInterceptors(object service, params Type[] interceptorTypes2exclude)
        {
            var fieldVal = GetInterceptorsField(service);

            var newInterceptors = fieldVal.Where(x => !interceptorTypes2exclude.Contains(x.GetType())).ToArray();

            GetInfo(service, FieldName).SetValue(service, newInterceptors);
        }

        public static void AddInterceptor<T>(object service, T interceptorType2add, int position = -1) where T : IInterceptor, new()
        {
            var newInterceptors = GetInterceptorsField(service).ToList();

            if (position == -1)
                newInterceptors.Add(new T());
            else
                newInterceptors.Insert(position, new T());

            GetInfo(service, FieldName).SetValue(service, newInterceptors.ToArray());
        }
    }
}
