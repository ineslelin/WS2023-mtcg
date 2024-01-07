using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.FightLogic.Enums;
using ws2023_mtcg.FightLogic;
using ws2023_mtcg.Models;

namespace ws2023_mtcg.Test
{
    internal class BattleTest
    {
        [Test]
        public void CheckElementDependency_Fire()
        {
            var cardA = new Cards()
            {
                Name = "A",
                Element = ElementType.fire,
                Type = CardType.spell,
                Damage = 10
            };

            var cardB = new Cards()
            {
                Name = "B",
                Element = ElementType.normal,
                Type = CardType.spell,
                Damage = 10
            };

            var cardC = new Cards()
            {
                Name = "C",
                Element = ElementType.ice,
                Type = CardType.spell,
                Damage = 10
            };

            var actual = cardA.Attack(cardB);
            Assert.That(actual, Is.EqualTo(cardA));

            actual = cardA.Attack(cardC);
            Assert.That(actual, Is.EqualTo(cardA));
        }

        [Test]
        public void CheckElementDependency_Normal()
        {
            var cardA = new Cards()
            {
                Name = "A",
                Element = ElementType.normal,
                Type = CardType.spell,
                Damage = 10
            };

            var cardB = new Cards()
            {
                Name = "B",
                Element = ElementType.water,
                Type = CardType.spell,
                Damage = 10
            };

            var cardC = new Cards()
            {
                Name = "C",
                Element = ElementType.electric,
                Type = CardType.spell,
                Damage = 10
            };

            var actual = cardA.Attack(cardB);
            Assert.That(actual, Is.EqualTo(cardA));

            actual = cardA.Attack(cardC);
            Assert.That(actual, Is.EqualTo(cardA));
        }

        [Test]
        public void CheckElementDependency_Water()
        {
            var cardA = new Cards()
            {
                Name = "A",
                Element = ElementType.water,
                Type = CardType.spell,
                Damage = 10
            };

            var cardB = new Cards()
            {
                Name = "B",
                Element = ElementType.fire,
                Type = CardType.spell,
                Damage = 10
            };

            var cardC = new Cards()
            {
                Name = "C",
                Element = ElementType.grass,
                Type = CardType.spell,
                Damage = 10
            };

            var actual = cardA.Attack(cardB);
            Assert.That(actual, Is.EqualTo(cardA));

            actual = cardA.Attack(cardC);
            Assert.That(actual, Is.EqualTo(cardA));
        }

        [Test]
        public void CheckElementDependency_Electric()
        {
            var cardA = new Cards()
            {
                Name = "A",
                Element = ElementType.electric,
                Type = CardType.spell,
                Damage = 10
            };

            var cardB = new Cards()
            {
                Name = "B",
                Element = ElementType.fire,
                Type = CardType.spell,
                Damage = 10
            };

            var cardC = new Cards()
            {
                Name = "C",
                Element = ElementType.ice,
                Type = CardType.spell,
                Damage = 10
            };

            var cardD = new Cards()
            {
                Name = "D",
                Element = ElementType.water,
                Type = CardType.spell,
                Damage = 10
            };

            var actual = cardA.Attack(cardB);
            Assert.That(actual, Is.EqualTo(cardA));

            actual = cardA.Attack(cardC);
            Assert.That(actual, Is.EqualTo(cardA));

            actual = cardA.Attack(cardD);
            Assert.That(actual, Is.EqualTo(cardA));
        }

        [Test]
        public void CheckElementDependency_Ice()
        {
            var cardA = new Cards()
            {
                Name = "A",
                Element = ElementType.ice,
                Type = CardType.spell,
                Damage = 10
            };

            var cardB = new Cards()
            {
                Name = "B",
                Element = ElementType.water,
                Type = CardType.spell,
                Damage = 10
            };

            var cardC = new Cards()
            {
                Name = "C",
                Element = ElementType.grass,
                Type = CardType.spell,
                Damage = 10
            };

            var cardD = new Cards()
            {
                Name = "D",
                Element = ElementType.normal,
                Type = CardType.spell,
                Damage = 10
            };

            var actual = cardA.Attack(cardB);
            Assert.That(actual, Is.EqualTo(cardA));

            actual = cardA.Attack(cardC);
            Assert.That(actual, Is.EqualTo(cardA));

            actual = cardA.Attack(cardD);
            Assert.That(actual, Is.EqualTo(cardA));
        }

        [Test]
        public void CheckElementDependency_Grass()
        {
            var cardA = new Cards()
            {
                Name = "A",
                Element = ElementType.grass,
                Type = CardType.spell,
                Damage = 10
            };

            var cardB = new Cards()
            {
                Name = "B",
                Element = ElementType.fire,
                Type = CardType.spell,
                Damage = 10
            };

            var cardC = new Cards()
            {
                Name = "C",
                Element = ElementType.normal,
                Type = CardType.spell,
                Damage = 10
            };

            var cardD = new Cards()
            {
                Name = "D",
                Element = ElementType.electric,
                Type = CardType.spell,
                Damage = 10
            };

            var actual = cardA.Attack(cardB);
            Assert.That(actual, Is.EqualTo(cardA));

            actual = cardA.Attack(cardC);
            Assert.That(actual, Is.EqualTo(cardA));

            actual = cardA.Attack(cardD);
            Assert.That(actual, Is.EqualTo(cardA));
        }

        [Test]
        public void CheckMonsterMonsterFight()
        {
            var cardA = new Cards()
            {
                Name = "A",
                Type = CardType.monster,
                Element = ElementType.fire,
                Damage = 15
            };

            var cardB = new Cards()
            {
                Name = "B",
                Type = CardType.monster,
                Element = ElementType.water,
                Damage = 10
            };

            var actual = cardA.Attack(cardB);

            Assert.That(actual, Is.EqualTo(cardA));
        }

        [Test]
        public void CheckSpecialty_TrollvNormal() 
        {
            var Troll = new Cards()
            {
                Name = "IceTroll"
            };

            var Enemy = new Cards()
            {
                Element = ElementType.normal
            };

            var actual = Troll.Attack(Enemy);
            Assert.That(actual, Is.EqualTo(Troll));

            actual = Enemy.Attack(Troll);
            Assert.That(actual, Is.EqualTo(Troll));
        }

        [Test]
        public void CheckSpecialty_ElfvDragon()
        {
            var Elf = new Cards()
            {
                Name = "FireElf"
            };

            var Dragon = new Cards()
            {
                Name = "Dragon"
            };

            var actual = Elf.Attack(Dragon);
            Assert.That(actual, Is.EqualTo(Elf));

            actual = Dragon.Attack(Elf);
            Assert.That(actual, Is.EqualTo(Elf));
        }

        [Test]
        public void CheckSpecialty_DragonvGoblin()
        {
            var Goblin = new Cards()
            {
                Name = "FireGoblin"
            };

            var Dragon = new Cards()
            {
                Name = "Dragon"
            };

            var actual = Goblin.Attack(Dragon);
            Assert.That(actual, Is.EqualTo(Dragon));

            actual = Dragon.Attack(Goblin);
            Assert.That(actual, Is.EqualTo(Dragon));
        }

        [Test]
        public void CheckSpecialty_KnightvWaterSpell()
        {
            var Knight = new Cards()
            {
                Name = "Knight"
            };

            var WaterSpell = new Cards()
            {
                Name = "WaterSpell",
                Type = CardType.spell
            };

            var actual = Knight.Attack(WaterSpell);
            Assert.That(actual, Is.EqualTo(WaterSpell));

            actual = WaterSpell.Attack(Knight);
            Assert.That(actual, Is.EqualTo(WaterSpell));
        }

        [Test]
        public void CheckSpecialty_KrakenvSpell()
        {
            var Kraken = new Cards()
            {
                Name = "Kraken"
            };

            var Spell = new Cards()
            {
                Name = "Spell",
                Type = CardType.spell
            };

            var actual = Spell.Attack(Kraken);
            Assert.That(actual, Is.EqualTo(Kraken));

            actual = Kraken.Attack(Spell);
            Assert.That(actual, Is.EqualTo(Kraken));
        }

        [Test]
        public void CheckSpecialty_OrkvWizard()
        {
            var Ork = new Cards()
            {
                Name = "Ork"
            };

            var Wizard = new Cards()
            {
                Name = "Wizard"
            };

            var actual = Ork.Attack(Wizard);
            Assert.That(actual, Is.EqualTo(Wizard));

            actual = Wizard.Attack(Ork);
            Assert.That(actual, Is.EqualTo(Wizard));
        }

        [Test]
        public void CheckSpecialty_WizardvFireSpell()
        {
            var FireSpell = new Cards()
            {
                Name = "FireSpell",
                Type = CardType.spell
            };

            var Wizard = new Cards()
            {
                Name = "Wizard"
            };

            var actual = FireSpell.Attack(Wizard);
            Assert.That(actual, Is.EqualTo(FireSpell));

            actual = Wizard.Attack(FireSpell);
            Assert.That(actual, Is.EqualTo(FireSpell));
        }

        [Test]
        public void CheckSpecialty_OrkvRegularSpell()
        {
            var RegularSpell = new Cards()
            {
                Name = "RegularSpell"
            };

            var Ork = new Cards()
            {
                Name = "Ork"
            };

            var actual = RegularSpell.Attack(Ork);
            Assert.That(actual, Is.EqualTo(Ork));

            actual =Ork.Attack(RegularSpell);
            Assert.That(actual, Is.EqualTo(Ork));
        }

        [Test]
        public void CheckCurseBattle()
        {
            var Curse = new Cards()
            {
                Name = "Curse",
                Type = CardType.curse,
                Element = ElementType.curse
            };

            var Monster = new Cards()
            {
                Name = "Monster",
                Type = CardType.monster,
                Damage = 10
            };

            var Spell = new Cards()
            {
                Name = "Spell",
                Type = CardType.spell,
                Damage = 10
            };

            var actual = Curse.Attack(Monster);
            Assert.That(actual, Is.EqualTo(null));

            actual = Curse.Attack(Spell);
            Assert.That(actual, Is.EqualTo(null));

            actual = Curse.Attack(Curse);
            Assert.That(actual, Is.EqualTo(null));
        }
    }
}
