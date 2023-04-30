using AutoMapper;
using System.Linq.Expressions;

namespace FDC.Caixa.Infra.IoC.AutoMapper
{
    public static class AutoMapperExtensions
    {
        private static IMapper _mapper;

        public static IMapper RegisterMap(this IMapper mapper)
        {
            _mapper = mapper;
            return mapper;
        }

        public static T MapTo<T>(this object value)
        {
            return _mapper.Map<T>(value);
        }

        public static IEnumerable<T> EnumerableTo<T>(this object value)
        {
            return _mapper.Map<IEnumerable<T>>(value);
        }

        public static IQueryable<T> QueryableTo<T>(this object value)
        {
            return _mapper.Map<IQueryable<T>>(value);
        }

        public static IQueryable<T> ProjectTo<T>(this IQueryable value)
        {
            return _mapper.ProjectTo(value, null, Array.Empty<Expression<Func<T, object>>>());
        }
    }
}
