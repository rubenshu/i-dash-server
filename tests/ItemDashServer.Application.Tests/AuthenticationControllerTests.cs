using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ItemDashServer.Api.Controllers;
using ItemDashServer.Infrastructure.Persistence;
using ItemDashServer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ItemDashServer.Api.Tests
{
    public class AuthenticationControllerTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly AuthenticationController _controller;

        public AuthenticationControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new ApplicationDbContext(options);

            // Add a test user
            var hmac = new System.Security.Cryptography.HMACSHA512();
            var testUser = new User
            {
                Username = "testuser",
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password")),
                PasswordSalt = hmac.Key
            };
            _dbContext.Users.Add(testUser);
            _dbContext.SaveChanges();

            var inMemorySettings = new Dictionary<string, string?>
            {
                {"JwtSettings:Secret", "supersecretkeysupersecretkey123456"},
                {"JwtSettings:ExpiryInMinutes", "60"},
                {"JwtSettings:Issuer", "TestIssuer"},
                {"JwtSettings:Audience", "TestAudience"}
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _controller = new AuthenticationController(_configuration, _dbContext);
        }

        [Fact]
        public void Login_WithValidCredentials_ReturnsTokenAndUser()
        {
            var request = new AuthenticationController.LoginRequest("testuser", "password");
            var result = _controller.Login(request) as OkObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Value);

            var json = JsonSerializer.Serialize(result.Value);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            Assert.True(root.TryGetProperty("token", out _));
            Assert.True(root.TryGetProperty("user", out var userProp));
            Assert.Equal("testuser", userProp.GetProperty("Username").GetString());
        }

        [Fact]
        public void Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            var request = new AuthenticationController.LoginRequest("testuser", "wrongpassword");
            var result = _controller.Login(request);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void Register_WithNewUsername_ReturnsOk()
        {
            var request = new AuthenticationController.LoginRequest("newuser", "newpassword");
            var result = _controller.Register(request);

            Assert.IsType<OkResult>(result);
            Assert.NotNull(_dbContext.Users.SingleOrDefault(u => u.Username == "newuser"));
        }

        [Fact]
        public void Register_WithExistingUsername_ReturnsBadRequest()
        {
            var request = new AuthenticationController.LoginRequest("testuser", "password");
            var result = _controller.Register(request) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Username already exists.", result.Value);
        }
    }
}