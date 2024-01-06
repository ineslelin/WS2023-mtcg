using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ws2023_mtcg.Models;
using ws2023_mtcg.Server.Repository;

namespace ws2023_mtcg.Test
{
    internal class UserTest
    {
        [Test]  
        public void Read_ReturnUser()
        {
            // arrange
            var userRepositoryMock = new Mock<UserRepository>();

            userRepositoryMock.Setup(repo => repo.Read("TestUser"))
                .Returns(new User { 
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
    }
}
