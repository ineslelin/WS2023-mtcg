using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ws2023_mtcg.Models;
using ws2023_mtcg.Server.Repository;

namespace ws2023_mtcg.Test.Repository
{
    internal class UserRepositoryTest
    {
        [Test]
        public void Read_ReturnUser()
        {
            // arrange
            var userRepositoryMock = new Mock<IUserRepository<User>>();

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
            Assert.That(actual, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Username, Is.EqualTo("TestUser"));
                Assert.That(actual.Password, Is.EqualTo("TestPassword"));
                Assert.That(actual.Coins, Is.EqualTo(20));
                Assert.That(actual.Elo, Is.EqualTo(100));
                Assert.That(actual.Wins, Is.EqualTo(0));
                Assert.That(actual.Losses, Is.EqualTo(0));
            });
        }

        [Test]
        public void Create_AddUserToRepository()
        {
            // arrange
            var userRepositoryMock = new Mock<IUserRepository<User>>();
            var userRepository = userRepositoryMock.Object;

            var newUser = new User
            {
                Username = "NewUser",
                Password = "NewPassword",
                Coins = 20,
                Elo = 100,
                Wins = 0,
                Losses = 0
            };

            // act
            userRepository.Create(newUser);

            // assert
            userRepositoryMock.Verify(repo => repo.Create(It.IsAny<User>()), Times.Once);
            userRepositoryMock.Verify(repo => repo.Create(newUser), Times.Once);
        }
    }
}
