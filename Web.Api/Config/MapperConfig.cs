using AutoMapper;
using Basket.ApiModel.RequestModels;
using Basket.ApiModel.ResponseModels;
using Basket.Dto.Dto;
using Basket.Dto.RequestDto;

namespace Web.Api.Config
{
    public class MapperConfig
    {
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<BasketDto, BasketResponseModel>();
                CreateMap<AddToBasketRequestModel, AddToBasketRequestDto>();
                CreateMap<UserDto, UserResponseModel>();
            }
        }
    }
}
