using Auction.Core.Models;
using Auction.Presentation.Contracts.Products;
using Auction.Presentation.Contracts.Users;
using AutoMapper;

namespace Auction.Presentation.Mappers;

public class AuctionMappingProfile : Profile
{
    public AuctionMappingProfile()
    {
        CreateMap<ProductEntity, GetProductResponse>()
            .ForMember(dest => dest.MaxBet, opt => opt.MapFrom(src => src.Bet != null ? src.Bet.Offer : (decimal?)null))
            .ForMember(dest => dest.MaxBetOwnerId, opt => opt.MapFrom(src => src.Bet != null ? src.Bet.UserId : (Guid?)null));

        CreateMap<AddProductRequest, ProductEntity>();

        CreateMap<UpdateProductRequest, ProductEntity>();

        CreateMap<UserEntity, GetUserInfoResponse>();

        CreateMap<ProductEntity, GetBetInfoResponse>()
            .ForMember(dest => dest.Offer, opt => opt.MapFrom(src => src.Bet != null ? src.Bet.Offer : default))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id));
    }
}
