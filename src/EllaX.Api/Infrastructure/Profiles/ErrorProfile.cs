using System;
using AutoMapper;
using EllaX.Api.Infrastructure.Dtos;
using EllaX.Api.Infrastructure.HttpResponses;

namespace EllaX.Api.Infrastructure.Profiles
{
    public class ErrorProfile : Profile
    {
        public ErrorProfile()
        {
            CreateMap<ErrorHttpResponse, ErrorHttpResponseDto>();
            CreateMap<Exception, ExceptionDto>();
        }
    }
}
