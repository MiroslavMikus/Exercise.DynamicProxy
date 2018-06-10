using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Exercise.DynamicProxy
{
    public class ProxyHelper
    {
        private const string FieldName = "__interceptors";
        private static FieldInfo GetInfo(object obj, string fieldName) => obj.GetType().GetField(fieldName);

        private IInterceptor[] GetInterceptorsField(object service) => GetInfo(service, FieldName).GetValue(service) as IInterceptor[];

        private void ExcudeInterceptors(object service, params Type[] interceptorTypes2exclude)
        {
            var fieldVal = GetInterceptorsField(service);

            var newInterceptors = fieldVal.Where(x => !interceptorTypes2exclude.Contains(x.GetType())).ToArray();

            GetInfo(service, FieldName).SetValue(service, newInterceptors);
        }

        private void AddInterceptor<T>(object service, T interceptorType2add, int position) where T : IInterceptor, new()
        {
            var newInterceptors = GetInterceptorsField(service).ToList();

            newInterceptors.Insert(position, new T());

            GetInfo(service, FieldName).SetValue(service, newInterceptors.ToArray());
        }
    }
}
