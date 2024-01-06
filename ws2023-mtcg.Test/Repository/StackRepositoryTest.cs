using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.FightLogic.Enums;
using ws2023_mtcg.Models;
using ws2023_mtcg.Server.Repository;

namespace ws2023_mtcg.Test.Repository
{
    internal class StackRepositoryTest
    {
        [Test]
        public void Read_ReturnStack()
        {
            // arrange
            var stackRepositoryMock = new Mock<IStackRepository<Cards>>();

            stackRepositoryMock.Setup(repo => repo.Read("TestUser"))
                .Returns(new[]
                {
                    new Cards { Id = "1", Name = "TestCard1" },
                    new Cards { Id = "2", Name = "TestCard2" },
                    new Cards { Id = "3", Name = "TestCard3" }
                });

            // act
            var actual = stackRepositoryMock.Object.Read("TestUser");

            // assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Length, Is.EqualTo(3));

            for (int i = 0; i < actual.Length; i++)
            {
                var card = actual[i];

                Assert.Multiple(() =>
                {
                    Assert.That(actual[i], Is.Not.Null);
                    Assert.That(actual[i].Id, Is.EqualTo($"{i + 1}"));
                    Assert.That(actual[i].Name, Is.EqualTo($"TestCard{i + 1}"));
                });
            }
        }

        [Test]
        public void Create_AddStackToRepository()
        {
            // arrange
            var stackRepositoryMock = new Mock<IStackRepository<Cards>>();
            var stackRepository = stackRepositoryMock.Object;

            var newCard = new Cards
            {
                Id = "TestId",
                Name = "TestCard",
                Damage = 15,
                Element = ElementType.fire,
                Type = CardType.monster,
                Owner = "TestUser"
            };

            // act
            stackRepository.Create(newCard);

            // assert
            stackRepositoryMock.Verify(repo => repo.Create(It.IsAny<Cards>()), Times.Once);
            stackRepositoryMock.Verify(repo => repo.Create(newCard), Times.Once);
        }
    }
}
