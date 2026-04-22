using Api.Common.DI;
using AutoMapper;

namespace Api.Common
{
    public static class MapperExtend
    {
        /// <summary>
        ///  类型映射，字段名称一一对应（用于生成新的对象）
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source">数据源对象</param>
        public static TDestination MapTo<TDestination>(this object source)
            where TDestination : class
        {
            if (source == null)
                return default(TDestination);

            var mapper = DIContainer.Instance.GetInstance<IMapper>();
            return mapper.Map<TDestination>(source);
        }

        /// <summary>
        ///  类型映射，字段名称一一对应（用于两个已存在的对象）
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">数据源对象</param>
        /// <param name="destination">需要返回的对象</param>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
            where TDestination : class
            where TSource : class
        {
            if (source == null)
                return default(TDestination);

            var mapper = DIContainer.Instance.GetInstance<IMapper>();
            return mapper.Map(source, destination);
        }
    }
}
