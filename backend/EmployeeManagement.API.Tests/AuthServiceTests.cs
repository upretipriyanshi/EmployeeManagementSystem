using EmployeeManagement.API.DTOs;
using EmployeeManagement.API.Models;
using EmployeeManagement.API.Repositories;
using EmployeeManagement.API.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace EmployeeManagement.API.Tests;

public class AuthServiceTests
{
    private Mock<IAuthRepository> _repository = null!;
    private Mock<IPasswordHasher<User>> _passwordHasher = null!;
    private Mock<IJwtService> _jwtService = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = new Mock<IAuthRepository>();
        _passwordHasher = new Mock<IPasswordHasher<User>>();
        _jwtService = new Mock<IJwtService>();
    }

    [Test]
    public async Task LoginAsync_ReturnsTokenAndUser_WhenCredentialsAreValid()
    {
        var user = new User
        {
            UserId = 12,
            FirstName = "Rita",
            LastName = "Example",
            Email = "rita@example.com",
            PasswordHash = "hashed-password",
            IsActive = true,
            Role = new Role { RoleName = "Manager" }
        };
        _repository.Setup(repository => repository.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);
        _passwordHasher.Setup(hasher => hasher.VerifyHashedPassword(user, user.PasswordHash, "secret"))
            .Returns(PasswordVerificationResult.Success);
        _jwtService.Setup(service => service.GenerateToken(user)).Returns("jwt-token");
        var service = new AuthService(_repository.Object, _passwordHasher.Object, _jwtService.Object);

        var result = await service.LoginAsync(new LoginDto { Email = user.Email, Password = "secret" });

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result!.UserId, Is.EqualTo(user.UserId));
            Assert.That(result.Role, Is.EqualTo("Manager"));
            Assert.That(result.Token, Is.EqualTo("jwt-token"));
        });
    }

    [Test]
    public async Task LoginAsync_ReturnsNull_WhenUserDoesNotExist()
    {
        _repository.Setup(repository => repository.GetUserByEmailAsync("missing@example.com"))
            .ReturnsAsync((User?)null);
        var service = new AuthService(_repository.Object, _passwordHasher.Object, _jwtService.Object);

        var result = await service.LoginAsync(new LoginDto { Email = "missing@example.com", Password = "secret" });

        Assert.That(result, Is.Null);
        _passwordHasher.Verify(hasher => hasher.VerifyHashedPassword(
            It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task LoginAsync_ReturnsNull_WhenPasswordIsInvalid()
    {
        var user = new User { Email = "rita@example.com", PasswordHash = "hashed", IsActive = true };
        _repository.Setup(repository => repository.GetUserByEmailAsync(user.Email)).ReturnsAsync(user);
        _passwordHasher.Setup(hasher => hasher.VerifyHashedPassword(user, user.PasswordHash, "wrong"))
            .Returns(PasswordVerificationResult.Failed);
        var service = new AuthService(_repository.Object, _passwordHasher.Object, _jwtService.Object);

        var result = await service.LoginAsync(new LoginDto { Email = user.Email, Password = "wrong" });

        Assert.That(result, Is.Null);
        _jwtService.Verify(jwt => jwt.GenerateToken(It.IsAny<User>()), Times.Never);
    }
}