using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Models;
using ws2023_mtcg.Server.Repository;
using ws2023_mtcg.FightLogic.Enums;

namespace ws2023_mtcg.Test.Repository
{
    internal class CardRepositoryTest
    {
        [Test]
        public void Read_ReturnCard()
        {
            // arrange
            var cardRepositoryMock = new Mock<ICardRepository<Cards>>();

            cardRepositoryMock.Setup(repo => repo.Read("TestId"))
                .Returns(new Cards
                {
                    Id = "TestId",
                    Name = "TestCard",
                    Damage = 15,
                    Element = ElementType.fire,
                    Type = CardType.monster
                });

            // act
            var actual = cardRepositoryMock.Object.Read("TestId");

            // assert
            Assert.That(actual, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo("TestId"));
                Assert.That(actual.Name, Is.EqualTo("TestCard"));
                Assert.That(actual.Damage, Is.EqualTo(15));
                Assert.That(actual.Element, Is.EqualTo(ElementType.fire));
                Assert.That(actual.Type, Is.EqualTo(CardType.monster));
            });
        }

        [Test]
        public void Create_AddCardToRepository()
        {
            // arrange
            var cardRepositoryMock = new Mock<ICardRepository<Cards>>();
            var cardRepository = cardRepositoryMock.Object;

            var newCard = new Cards
            {
                Id = "TestId",
                Name = "TestCard",
                Damage = 15,
                Element = ElementType.fire,
                Type = CardType.monster
            };

            // act
            cardRepository.Create(newCard);

            // assert
            cardRepositoryMock.Verify(repo => repo.Create(It.IsAny<Cards>()), Times.Once);
            cardRepositoryMock.Verify(repo => repo.Create(newCard), Times.Once);
        }
    }
}
