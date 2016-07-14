using System;

namespace PetaPocoDemo.ORM
{
    /// <summary>
    /// 仓储基类（单数据库单表模式)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleRepository<T> : PetaPocoRepository<T> where T : new()
    {
        public SingleRepository()
        {
            ConnectionStringName = SingleDbFactory.GetKeyFrom(this);
        } 
    }

    /// <summary>
    /// 数据库描述特性(单体）
    /// </summary> 
    [AttributeUsage(AttributeTargets.Class)]
    public class SingleDbFactory : Attribute
    {
        public readonly string ConnectionName;

        public SingleDbFactory(string connectionName)
        {
            ConnectionName = connectionName;
        }

        public static string GetKeyFrom(object target)
        {
            var objectType = target.GetType();
            var attributes = objectType.GetCustomAttributes(typeof(SingleDbFactory), true);
            if (attributes.Length > 0)
            {
                var attribute = (SingleDbFactory)attributes[0];
                return attribute.ConnectionName;
            }

            return "DefaultDB";
        }
    }
}
