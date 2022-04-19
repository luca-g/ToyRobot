using Microsoft.VisualStudio.TestTools.UnitTesting;
using Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using ToyRobot.Core.Tests;

namespace Authentication.Tests
{
    [TestClass()]
    public class JwtServiceTests
    {
        const string Secret = "secret";
        private (string, Guid) CreateToken()
        {
            var userGuid = Guid.NewGuid();
            var settings = new JwtSettings
            {
                Audience = "localhost",
                ExpirationMinutes = null,
                Issuer = "localhost",
                Secret = Secret
            };
            Mock<IOptions<JwtSettings>> options = new();
            options.Setup(x => x.Value).Returns(settings);

            var mock = new MockServicesHelper<JwtService>();

            var jwtService = new JwtService(
                options.Object,
                mock.Logger.Object);

            var claims = new Dictionary<string, object>
            {
                { "userGuid", userGuid }
            };
            var token = jwtService.CreateToken(claims);
            return (token, userGuid);
        }
        private IDictionary<string, object> DecodeToken(string token)
        {
            var settings = new JwtSettings
            {
                Audience = "localhost",
                ExpirationMinutes = null,
                Issuer = "localhost",
                Secret = Secret
            };
            Mock<IOptions<JwtSettings>> options = new();
            options.Setup(x => x.Value).Returns(settings);

            var mock = new MockServicesHelper<JwtService>();

            var jwtService = new JwtService(
                options.Object,
                mock.Logger.Object);

            return jwtService.Decode(token);
        }

        [TestMethod()]
        public void CreateTokenTest()
        {
            var (token, guid) = CreateToken();
            Assert.IsNotNull(token);
        }

        [TestMethod()]
        public void DecodeTest()
        {
            var (token, guid) = CreateToken();
            var claims = DecodeToken(token);
            Assert.IsNotNull(claims);
            Assert.IsNotNull(claims["userGuid"]);
            Assert.AreEqual(claims["userGuid"], guid.ToString());
        }
    }
}