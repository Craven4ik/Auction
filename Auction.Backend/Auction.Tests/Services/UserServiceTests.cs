using Auction.Application.Auth;
using Auction.Application.Services;
using Auction.Core.Models;
using Auction.Domain.Interfaces.Repositories;
using Moq;

namespace Auction.Tests.Services;

public class UserServiceTests
{
    [Fact]
    public async Task Register_Should_Add_User_To_Repository()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var mockPasswordHasher = new Mock<IPasswordHasher>();
        var mockJwtProvider = new Mock<IJwtProvider>();

        var userService = new UserService(mockUserRepository.Object, mockPasswordHasher.Object, mockJwtProvider.Object);

        // Act
        await userService.Register("test", "test@test.com", "password");

        // Assert
        mockUserRepository.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Login_Should_Return_Token_On_Valid_Credentials()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var mockPasswordHasher = new Mock<IPasswordHasher>();
        var mockJwtProvider = new Mock<IJwtProvider>();

        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            PasswordHash = "hashedPassword"
        };

        mockUserRepository.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(user);
        mockPasswordHasher.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        mockJwtProvider.Setup(x => x.Generate(user)).Returns("toekn");

        var userService = new UserService(mockUserRepository.Object, mockPasswordHasher.Object, mockJwtProvider.Object);

        // Act
        var token = await userService.Login("test@test.com", "password");

        // Assert
        Assert.NotNull(token);
        mockJwtProvider.Verify(x => x.Generate(user), Times.Once);
    }

    [Fact]
    public async Task Login_Should_Throw_Exception_On_Invalid_Credentials()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var mockPasswordHasher = new Mock<IPasswordHasher>();
        var mockJwtProvider = new Mock<IJwtProvider>();

        var userService = new UserService(mockUserRepository.Object, mockPasswordHasher.Object, mockJwtProvider.Object);

        // Act and Assert
        await Assert.ThrowsAsync<Exception>(() => userService.Login("test@test.com", "wrongPassword"));
    }

    [Fact]
    public async Task GetById_Should_Return_User_On_Valid_Token()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var mockPasswordHasher = new Mock<IPasswordHasher>();
        var mockJwtProvider = new Mock<IJwtProvider>();

        mockJwtProvider.Setup(x => x.GetUserIdFromToken(It.IsAny<string>())).Returns(Guid.NewGuid());
        var user = new UserEntity { Id = Guid.NewGuid() };
        mockUserRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(user);

        var userService = new UserService(mockUserRepository.Object, mockPasswordHasher.Object, mockJwtProvider.Object);

        // Act
        var result = await userService.GetById("validToken");

        // Assert
        Assert.Equal(user, result);
    }

    [Fact]
    public async Task GetById_Should_Return_Null_On_Invalid_Token()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var mockPasswordHasher = new Mock<IPasswordHasher>();
        var mockJwtProvider = new Mock<IJwtProvider>();

        mockJwtProvider.Setup(x => x.GetUserIdFromToken(It.IsAny<string>())).Returns(Guid.Empty);

        var userService = new UserService(mockUserRepository.Object, mockPasswordHasher.Object, mockJwtProvider.Object);

        // Act
        var result = await userService.GetById("invalidToken");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void CheckToken_Should_Return_False_On_Invalid_Token()
    {
        // Arrange
        var mockJwtProvider = new Mock<IJwtProvider>();

        var userService = new UserService(null!, null!, mockJwtProvider.Object);

        // Act
        var result = userService.CheckToken("invalidToken");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async void IsFreeEmail_Should_Return_True_On_Free_Email()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();

        mockUserRepository.Setup(x => x.IsFreeEmail(It.IsAny<string>())).ReturnsAsync(true);

        var userService = new UserService(mockUserRepository.Object, null!, null!);

        // Act
        var result = await userService.IsFreeEmail("free@email.com");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async void GetUserBalance_Should_Return_Balance_On_Valid_Token()
    {
        // Arrange
        var mockJwtProvider = new Mock<IJwtProvider>();
        var mockUserRepository = new Mock<IUserRepository>();

        mockJwtProvider.Setup(x => x.GetUserIdFromToken(It.IsAny<string>())).Returns(Guid.NewGuid());
        mockUserRepository.Setup(x => x.GetUserBalance(It.IsAny<Guid>())).ReturnsAsync(100);

        var userService = new UserService(mockUserRepository.Object, null!, mockJwtProvider.Object);

        // Act
        var result = await userService.GetUserBalance("validToken");

        // Assert
        Assert.Equal(100, result);
    }
}
