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

        [Test]
        public void CheckIfCardFireType()
        {
            // arrange
            var cardRepositoryMock = new Mock<ICardRepository<Cards>>();

            cardRepositoryMock.Setup(repo => repo.Read("TestId"))
                .Returns(new Cards
                {
                    Id = "TestId",
                    Element = ElementType.fire
                });

            // act
            var actual = cardRepositoryMock.Object.Read("TestId");

            // assert
            Assert.That(actual, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo("TestId"));
                Assert.That(actual.Element, Is.EqualTo(ElementType.fire));
            });
        }

        [Test]
        public void CheckIfCardWaterType()
        {
            // arrange
            var cardRepositoryMock = new Mock<ICardRepository<Cards>>();

            cardRepositoryMock.Setup(repo => repo.Read("TestId"))
                .Returns(new Cards
                {
                    Id = "TestId",
                    Element = ElementType.water
                });

            // act
            var actual = cardRepositoryMock.Object.Read("TestId");

            // assert
            Assert.That(actual, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo("TestId"));
                Assert.That(actual.Element, Is.EqualTo(ElementType.water));
            });
        }

        [Test]
        public void CheckIfCardRegularType()
        {
            // arrange
            var cardRepositoryMock = new Mock<ICardRepository<Cards>>();

            cardRepositoryMock.Setup(repo => repo.Read("TestId"))
                .Returns(new Cards
                {
                    Id = "TestId",
                    Element = ElementType.normal
                });

            // act
            var actual = cardRepositoryMock.Object.Read("TestId");

            // assert
            Assert.That(actual, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo("TestId"));
                Assert.That(actual.Element, Is.EqualTo(ElementType.normal));
            });
        }

        [Test]
        public void CheckIfCardElectricType()
        {
            // arrange
            var cardRepositoryMock = new Mock<ICardRepository<Cards>>();

            cardRepositoryMock.Setup(repo => repo.Read("TestId"))
                .Returns(new Cards
                {
                    Id = "TestId",
                    Element = ElementType.electric
                });

            // act
            var actual = cardRepositoryMock.Object.Read("TestId");

            // assert
            Assert.That(actual, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo("TestId"));
                Assert.That(actual.Element, Is.EqualTo(ElementType.electric));
            });
        }

        [Test]
        public void CheckIfCardIceType()
        {
            // arrange
            var cardRepositoryMock = new Mock<ICardRepository<Cards>>();

            cardRepositoryMock.Setup(repo => repo.Read("TestId"))
                .Returns(new Cards
                {
                    Id = "TestId",
                    Element = ElementType.ice
                });

            // act
            var actual = cardRepositoryMock.Object.Read("TestId");

            // assert
            Assert.That(actual, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo("TestId"));
                Assert.That(actual.Element, Is.EqualTo(ElementType.ice));
            });
        }

        [Test]
        public void CheckIfCardGrassType()
        {
            // arrange
            var cardRepositoryMock = new Mock<ICardRepository<Cards>>();

            cardRepositoryMock.Setup(repo => repo.Read("TestId"))
                .Returns(new Cards
                {
                    Id = "TestId",
                    Element = ElementType.grass
                });

            // act
            var actual = cardRepositoryMock.Object.Read("TestId");

            // assert
            Assert.That(actual, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo("TestId"));
                Assert.That(actual.Element, Is.EqualTo(ElementType.grass));
            });
        }

        [Test]
        public void CheckIfCardMonsterCard()
        {
            // arrange
            var cardRepositoryMock = new Mock<ICardRepository<Cards>>();

            cardRepositoryMock.Setup(repo => repo.Read("TestId"))
                .Returns(new Cards
                {
                    Id = "TestId",
                    Type = CardType.monster
                });

            // act
            var actual = cardRepositoryMock.Object.Read("TestId");

            // assert
            Assert.That(actual, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo("TestId"));
                Assert.That(actual.Type, Is.EqualTo(CardType.monster));
            });
        }

        [Test]
        public void CheckIfCardSpellCard()
        {
            // arrange
            var cardRepositoryMock = new Mock<ICardRepository<Cards>>();

            cardRepositoryMock.Setup(repo => repo.Read("TestId"))
                .Returns(new Cards
                {
                    Id = "TestId",
                    Type = CardType.spell
                });

            // act
            var actual = cardRepositoryMock.Object.Read("TestId");

            // assert
            Assert.That(actual, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo("TestId"));
                Assert.That(actual.Type, Is.EqualTo(CardType.spell));
            });
        }

        [Test]
        public void CheckIfCardCurseCard()
        {
            // arrange
            var cardRepositoryMock = new Mock<ICardRepository<Cards>>();

            cardRepositoryMock.Setup(repo => repo.Read("TestId"))
                .Returns(new Cards
                {
                    Id = "TestId",
                    Element = ElementType.curse,
                    Type = CardType.curse
                });

            // act
            var actual = cardRepositoryMock.Object.Read("TestId");

            // assert
            Assert.That(actual, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Id, Is.EqualTo("TestId"));
                Assert.That(actual.Element, Is.EqualTo(ElementType.curse));
                Assert.That(actual.Type, Is.EqualTo(CardType.curse));
            });
        }
    }
}
