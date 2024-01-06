using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.FightLogic.Enums;
using ws2023_mtcg.Models;
using ws2023_mtcg.Server.Repository;
using ws2023_mtcg.Server.Res;

namespace ws2023_mtcg.Test.Repository
{
    internal class TradingRepositoryTest
    {
        [Test]
        public void Read_ReturnTradingDeal()
        {
            // arrange
            var tradingRepositoryMock = new Mock<ITradingRepository<TradingDeal>>();

            tradingRepositoryMock.Setup(repo => repo.Read("TestId"))
                .Returns(new TradingDeal
                {
                    Id = "TestId",
                    CardToTrade = "TestCardId",
                    Username = "TestUser",
                    Type = CardType.monster,
                    MinimumDamage = 15
                });

            // act
            var actual = tradingRepositoryMock.Object.Read("TestId");

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo("TestId"));
                Assert.That(actual.CardToTrade, Is.EqualTo("TestCardId"));
                Assert.That(actual.Username, Is.EqualTo("TestUser"));
                Assert.That(actual.Type, Is.EqualTo(CardType.monster));
                Assert.That(actual.MinimumDamage, Is.EqualTo(15));
            });
        }

        [Test]
        public void Create_AddTradingDealToRepository()
        {
            // arrange
            var tradingRepositoryMock = new Mock<ITradingRepository<TradingDeal>>();
            var tradingRepository = tradingRepositoryMock.Object;

            var newTradingDeal = new TradingDeal
            {
                Id = "TestId",
                CardToTrade = "TestCardId",
                Username = "TestUser",
                Type = CardType.monster,
                MinimumDamage = 15
            };

            // act
            var username = "TestUser";
            tradingRepository.Create(newTradingDeal, username);

            // assert
            tradingRepositoryMock.Verify(repo => repo.Create(It.IsAny<TradingDeal>(), It.IsAny<string>()), Times.Once);
            tradingRepositoryMock.Verify(repo => repo.Create(newTradingDeal, username), Times.Once);
        }
    }
}
