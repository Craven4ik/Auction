using Auction.Application.Auth;
using Auction.Core.Models;
using Auction.Domain.Interfaces.Repositories;
using Auction.Domain.Interfaces.Services;

namespace Auction.Application.Services;

public class UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task Register(string userName, string email, string password)
    {
        var hashedPassword = _passwordHasher.Generate(password);

        await _userRepository.Add(userName, email, hashedPassword);
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);

        var result = _passwordHasher.Verify(password, user.PasswordHash);

        if (result == false)
            throw new Exception("Failed to login");

        var token = _jwtProvider.Generate(user);

        return token;
    }
}
