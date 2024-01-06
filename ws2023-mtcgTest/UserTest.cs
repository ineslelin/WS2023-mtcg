using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Models;
using ws2023_mtcg.Server.Repository;

namespace ws2023_mtcgTest
{
    internal class UserTest
    {
        [Test]
        public void Read_ReturnUser()
        {
            // arrange
            var userRepositoryMock = new Mock<UserRepository>();
            userRepositoryMock.Setup(repo => repo.Read("TestUser"))
                .Returns(new User
                {
                    Username = "TestUser",
                    Password = "TestPassword",
                    Coins = 20,
                    Elo = 100,
                    Wins = 0,
                    Losses = 0
                });

            // act
            var actual = userRepositoryMock.Object.Read("TestUser");

            // assert
            Assert.AreEqual();
        }
    }
}
