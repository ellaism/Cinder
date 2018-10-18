using System.Collections;
using System.Collections.Generic;
using AutoMapper;

namespace EllaX.Logic.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<TDto> MapTo<TDto>(this IEnumerable sourceList, IMapper mapper)
        {
            return mapper.Map<IEnumerable<TDto>>(sourceList);
        }
    }
}
