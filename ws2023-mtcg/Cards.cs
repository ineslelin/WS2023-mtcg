using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ws2023_mtcg.Dictionaries;
using ws2023_mtcg.Enums;

namespace ws2023_mtcg
{
    internal class Cards
    {
        public string Name { get; set; }
        public ElementType Element { get; set; }
        public CardType Type { get; set; }

        private MonsterType _monsterType;
        private SpellType _spellType;

        public int Damage { get; set; }
        public bool IsAlive;

        DElementDependency elementDependency = new DElementDependency();
        DCardName cardName = new DCardName();
        DCardElement cardElement = new DCardElement();

        public Cards(MonsterType monster)
        {
            monster = _monsterType;
            this.Name = cardName.MonsterName[monster];
            this.Element = cardElement.MonsterCardElement[monster];
            this.Type = CardType.monster;

            Random random = new Random();
            this.Damage = random.Next(5, 100);

            IsAlive = true;
        }

        public Cards(SpellType spell)
        {
            spell = _spellType;
            this.Name = cardName.SpellName[spell];
            this.Element = cardElement.SpellCardElement[spell];
            this.Type = CardType.spell;

            Random random = new Random();
            this.Damage = random.Next(5, 100);

            IsAlive = true;
        }

        public Cards Attack(Cards target)
        {
            // THIS NEEDS TO BE REDONE, IT NEEDS TO JESUS FUCKING CHRIST
            if(target.Type == CardType.monster)
            {
                switch(this._monsterType)
                {
                    // elves always win against dragons
                    case MonsterType.FireElf when target._monsterType == MonsterType.Dragon:
                    case MonsterType.RegularElf when target._monsterType == MonsterType.Dragon:
                    case MonsterType.WaterElf when target._monsterType == MonsterType.Dragon:
                    // dragons always win against goblins
                    case MonsterType.Dragon when target._monsterType == MonsterType.FireGoblin:
                    case MonsterType.Dragon when target._monsterType == MonsterType.RegularGoblin:
                    case MonsterType.Dragon when target._monsterType == MonsterType.WaterGoblin:
                    // trolls always win against normal type enemies
                    case MonsterType.FireTroll when target.Element == ElementType.normal:
                    case MonsterType.RegularTroll when target.Element == ElementType.normal:
                    case MonsterType.WaterTroll when target.Element == ElementType.normal:
                        return this;
                    // dragons always lose against elves
                    case MonsterType.Dragon when target._monsterType == MonsterType.FireElf:
                    case MonsterType.Dragon when target._monsterType == MonsterType.RegularElf:
                    case MonsterType.Dragon when target._monsterType == MonsterType.WaterElf:
                    // goblins always lose to dragons
                    case MonsterType.FireGoblin when target._monsterType == MonsterType.Dragon:
                    case MonsterType.RegularGoblin when target._monsterType == MonsterType.Dragon:
                    case MonsterType.WaterGoblin when target._monsterType == MonsterType.Dragon:
                        return target;
                }
            }

            if (this.Type == CardType.monster && target.Type == CardType.monster)
                return DamageFight(this, target);
                    
            return ElementFight(this, target);
        }

        public Cards DamageFight(Cards source, Cards target)
        {
            if (source.Damage > target.Damage)
            {
                target.IsAlive = false;
                AnnounceWinner(source, target);

                return source;
            }

            source.IsAlive = false;
            AnnounceWinner(source, target);

            return target;
        }

        public Cards ElementFight(Cards source, Cards target)
        {
            if(source.Element == target.Element)
            {
                return DamageFight(source, target);
            }

            if (target.Element == elementDependency.ElementDependencies[Tuple.Create(source.Element, target.Element)])
            {
                if(source.Damage / 2 > target.Damage * 2)
                {
                    target.IsAlive = false;
                    AnnounceWinner(source, target);

                    return source;
                }  

                source.IsAlive = false;
                AnnounceWinner(source, target);

                return target;
            }

            if (target.Damage / 2 > source.Damage * 2)
            {
                source.IsAlive = false;
                AnnounceWinner(source, target);

                return target;
            }

            target.IsAlive = false;
            AnnounceWinner(source, target);

            return source;
        }

        public void AnnounceWinner(Cards source, Cards target)
        {
            switch(target.Type)
            {
                case CardType.monster when source.Type == CardType.monster:
                    Console.WriteLine(source.Damage + " vs " + target.Damage + " => " + (source.Damage > target.Damage ? source.Name : target.Name)
                        + " defeats " + (source.Damage < target.Damage ? source.Name : target.Name) + "!");
                    break;

                case CardType.monster when source.Type == CardType.spell:
                case CardType.spell when source.Type == CardType.monster:
                case CardType.spell when source.Type == CardType.spell:
                    if(source.Element == target.Element)
                    {
                        Console.WriteLine(source.Damage + " vs " + target.Damage + " => " + (source.Damage > target.Damage ? source.Name : target.Name)
                        + " defeats " + (source.Damage < target.Damage ? source.Name : target.Name) + "!");
                        break;
                    }

                    Console.Write(source.Damage + " vs " + target.Damage + " => ");

                    if (target.Element == elementDependency.ElementDependencies[Tuple.Create(source.Element, target.Element)])
                    {
                        Console.WriteLine(source.Damage / 2 + " vs " + target.Damage * 2 + " => " + (source.Damage / 2 > target.Damage * 2 ? source.Name : target.Name) + " wins!");

                        break;
                    }

                    Console.WriteLine(source.Damage * 2 + " vs " + target.Damage / 2 + " => " + (source.Damage * 2 > target.Damage / 2 ? source.Name : target.Name) + " wins!");

                    break;
            }
        }
    }
}

