using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Moq;
using ToyRobot.Core.Tests;

namespace Authentication.Tests
{
    [TestClass()]
    public class JwtServiceTests
    {
        private static (string, Guid) CreateToken(bool addClaims = true)
        {
            var userGuid = Guid.NewGuid();
            var settings = new JwtSettings
            {
                Audience = "localhost",
                ExpirationMinutes = null,
                Issuer = "localhost",
                CertificatePath = "d:\\Certificates\\localhost.pfx",
                CertificatePassword = "secretPassword"
            };
            Mock<IOptions<JwtSettings>> options = new();
            options.Setup(x => x.Value).Returns(settings);

            var mock = new MockServicesHelper<JwtService>();

            var jwtService = new JwtService(
                options.Object,
                mock.Logger.Object);

            string token;
            if (addClaims)
            {
                var claims = new Dictionary<string, object>
                {
                    { "userGuid", userGuid }
                };
                token = jwtService.CreateToken(claims);
            }
            else
            {
                token = jwtService.CreateToken();
            }
            return (token, userGuid);
        }
        private static IDictionary<string, object> DecodeToken(string token)
        {
            var settings = new JwtSettings
            {
                Audience = "localhost",
                ExpirationMinutes = null,
                Issuer = "localhost",
                CertificatePath = "d:\\Certificates\\localhost.pfx",
                CertificatePassword = "secretPassword"
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
            var (token, _) = CreateToken();
            Assert.IsNotNull(token);
            Assert.IsTrue(token.Length > 0);
        }
        [TestMethod()]
        public void CreateTokenTest_NoClaims()
        {
            var (token, _) = CreateToken(false);
            Assert.IsNotNull(token);
            Assert.IsTrue(token.Length > 0);
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
        [TestMethod()]
        public void DecodeTest_NoClaims()
        {
            var (token, _) = CreateToken(false);
            Assert.IsNotNull(token);
            Assert.IsTrue(token.Length > 0);
            var claims = DecodeToken(token);
            Assert.IsNotNull(claims);
            Assert.IsTrue(claims.Count == 2);
        }
    }
}