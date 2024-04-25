using Auction.Core.Models;

namespace Auction.Application.Auth;

public interface IJwtProvider
{
    string Generate(UserEntity userEntity);
}
