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

        if (user == null)
            throw new Exception("User not found");

        var result = _passwordHasher.Verify(password, user.PasswordHash);

        if (result == false)
            throw new Exception("Failed to login");

        var token = _jwtProvider.Generate(user);

        return token;
    }

    public async Task<UserEntity?> GetById(string token)
    {
        var id = _jwtProvider.GetUserIdFromToken(token);

        if (Equals(id, Guid.Empty))
            return null;

        return await _userRepository.GetById(id);
    }

    public async Task UpdateBalance(string token, decimal amount)
    {
        var id = _jwtProvider.GetUserIdFromToken(token);

        if (Equals(id, Guid.Empty))
            return;

        await _userRepository.UpdateBalance(id, amount);
    }

    public bool CheckToken(string token)
        => _jwtProvider.CheckToken(token);

    public async Task<bool> IsFreeEmail(string email)
        => await _userRepository.IsFreeEmail(email);

    public async Task<decimal> GetUserBalance(string token)
    {
        var id = _jwtProvider.GetUserIdFromToken(token);

        if (Equals(id, Guid.Empty))
            return 0;

        return await _userRepository.GetUserBalance(id);
    }

    public async Task<decimal> GetFreeUserBalance(string token)
    {
        var id = _jwtProvider.GetUserIdFromToken(token);

        if (Equals(id, Guid.Empty))
            return 0;

        return await (_userRepository.GetFreeUserBalance(id));
    }

    public async Task<string> GetUserName(Guid id)
        => await _userRepository.GetUserName(id);

    public bool CheckProductOwner(Guid id, string token)
    {
        var userId = _jwtProvider.GetUserIdFromToken(token);

        return Guid.Equals(userId, id);
    }

    public Guid GetUserIdFromToken(string token)
        => _jwtProvider.GetUserIdFromToken(token);
}
