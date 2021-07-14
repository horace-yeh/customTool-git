using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionHelper
{
    //Reference : https://docs.microsoft.com/en-us/dotnet/api/system.reflection?view=net-5.0

    public class ReflectionTool
    {
        /// <summary>
        /// 使用Reflection取得物件屬性內容
        /// </summary>
        /// <param name="model">物件</param>
        /// <param name="PropertyName">屬性名稱</param>
        /// <returns></returns>
        public object GetClassPropertyValue(object model, string PropertyName)
        {
            Type t = model.GetType();
            PropertyInfo pi = t.GetProperty(PropertyName);
            return pi.GetValue(model, null);
        }

        /// <summary>
        /// 使用Reflection設定物件屬性內容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="PropertyName"></param>
        /// <param name="value"></param>
        public void SetClassPropertyValue<T>(ref T model, string PropertyName, object value)
        {
            Type t = model.GetType();
            PropertyInfo pi = t.GetProperty(PropertyName);
            pi.SetValue(model, value);
        }

        /// <summary>
        /// 使用Reflection將Class轉Dictionary
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetClassToDictionary(object model)
        {
            Type t = model.GetType();
            PropertyInfo[] piArr = t.GetProperties();
            var dict = piArr.ToDictionary(x => x.Name, x => x.GetValue(model, null));
            return dict;
        }
    }
}
