using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            this._monsterType = monster;
            this.Name = cardName.MonsterName[monster];
            this.Element = cardElement.MonsterCardElement[monster];
            this.Type = CardType.monster;

            Random random = new Random();
            this.Damage = random.Next(5, 100);

            IsAlive = true;
        }

        public Cards(SpellType spell)
        {
            this._spellType = spell;
            this.Name = cardName.SpellName[spell];
            this.Element = cardElement.SpellCardElement[spell];
            this.Type = CardType.spell;

            Random random = new Random();
            this.Damage = random.Next(5, 100);

            IsAlive = true;
        }

        public Cards Attack(Cards target)
        {
            // the collapse of shame pt.2: my push to github is lost :(
            if(Regex.IsMatch(this.Name, @"(Fire|Regular|Water)?Elf"))
            {
                // elves always win agaisnt dragons
                if (target.Name == "Dragon")
                {
                    Console.WriteLine($"Due to their age-old acquaintace {this.Name} knows how to evade all of {target.Name}'s attacks! " +
                        $"{this.Name} defeats {target.Name}!");

                    target.IsAlive = false;
                    return this;
                }

                // trolls always win to normal enemies
                if (this.Element == ElementType.normal && target.Name == "Troll")
                {
                    Console.WriteLine($"Regular attacks and spell don't affect {target.Name}! {target.Name} defeats {this.Name}!");

                    this.IsAlive = false;
                    return target;
                }
            }
            if(Regex.IsMatch(this.Name, @"(Fire|Regular|Water)?Goblin"))
            {
                // goblins always loose to dragons
                if (target.Name == "Dragon")
                {
                    Console.WriteLine($"{this.Name} is too afraid of {target.Name} to attack! {target.Name} defeats {this.Name}!");

                    this.IsAlive = false;
                    return target;
                }

                // trolls always win to normal enemies
                if (this.Element == ElementType.normal && target.Name == "Troll")
                {
                    Console.WriteLine($"Regular attacks and spell don't affect {target.Name}! {target.Name} defeats {this.Name}!");

                    this.IsAlive = false;
                    return target;
                }
            }
            if(Regex.IsMatch(this.Name, @"(Fire|Regular|Water)?Troll"))
            {
                // trolls always win to normal enemies
                if (target.Element == ElementType.normal)
                {
                    Console.WriteLine($"Regular attacks and spell don't affect {this.Name}! {this.Name} defeats {target.Name}!");

                    target.IsAlive = false;
                    return this;
                }
            }
            if(this._monsterType == MonsterType.Dragon)
            {
                // dragon always wins against goblin
                if (Regex.IsMatch(target.Name, @"(Fire|Water|Regular)?Goblin"))
                {
                    Console.WriteLine($"{target.Name} is too afraid of {this.Name} to attack! {this.Name} defeats {target.Name}!");

                    target.IsAlive = false;
                    return this;
                }

                // elf always defeats dragon
                if (Regex.IsMatch(target.Name, @"(Fire|Water|Regular)?Elf"))
                {
                    Console.WriteLine($"Due to their age-old acquaintace {target.Name} knows how to evade all of {this.Name}'s attacks! " +
                        $"{target.Name} defeats {this.Name}!");

                    this.IsAlive = false;
                    return target;
                }
            }
            if(this._monsterType == MonsterType.Knight)
            {
                // knights always lose to waterspells
                if (target.Name == "WaterSpell")
                {
                    Console.WriteLine($"{this.Name} drowned from {target.Name}'s attack! {target.Name} defeats {this.Name}!");

                    this.IsAlive = false;
                    return target;
                }

                // trolls always win to normal enemies
                if (target.Name == "Troll")
                {
                    Console.WriteLine($"Regular attacks and spell don't affect {target.Name}! {target.Name} defeats {this.Name}!");

                    this.IsAlive = false;
                    return target;
                }
            }
            if(this._monsterType == MonsterType.Kraken)
            {
                // the kraken is immune to spells
                if (target.Type == CardType.spell)
                {
                    Console.WriteLine($"Spells don't affect {this.Name}, rendering {target.Name} useless! " +
                        $"{this.Name} defeats {target.Name}!");

                    target.IsAlive = false;
                    return this;
                }
            }
            if(this._monsterType == MonsterType.Ork)
            {
                // orks always lose to wizards
                if (target.Name == "Wizard")
                {
                    Console.WriteLine($"{target.Name} took control of {this.Name}! {target.Name} defeats {this.Name}!");

                    this.IsAlive = false;
                    return target;
                }

                // trolls always win to normal enemies
                if (target.Name == "Troll")
                {
                    Console.WriteLine($"Regular attacks and spell don't affect {target.Name}! {target.Name} defeats {this.Name}!");

                    this.IsAlive = false;
                    return target;
                }
            }
            if(this._monsterType == MonsterType.Wizard)
            {
                // wizards always win against orks
                if (target.Name == "Ork")
                {
                    Console.WriteLine($"{this.Name} took control of {target.Name}! {this.Name} defeats {target.Name}!");

                    target.IsAlive = false;
                    return this;
                }

                // wizard always loses to firespells (which actually doenst make sense considering wizards are water types but shush)
                if (target.Name == "FireSpell")
                {
                    Console.WriteLine($"{this.Name}'s robes are very flammable! {target.Name} defeats {this.Name}!");

                    this.IsAlive = false;
                    return target;
                }
            }
            if(this._spellType == SpellType.FireSpell)
            {
                // wizard always loses to firespells (which actually doenst make sense considering wizards are water types but shush)
                if (target.Name == "Wizard")
                {
                    Console.WriteLine($"{target.Name}'s robes are very flammable! {this.Name} defeats {target.Name}!");

                    target.IsAlive = false;
                    return this;
                }

                // kraken is immune to spells
                if (target.Name == "Kraken")
                {
                    Console.WriteLine($"Spells don't affect {target.Name}, rendering {this.Name} useless! " +
                        $"{target.Name} defeats {this.Name}!");

                    this.IsAlive = false;
                    return target;
                }
            }
            if(this._spellType == SpellType.RegularSpell)
            {
                // orks are immune to regular spells
                if (target.Name == "Ork")
                {
                    Console.WriteLine($"{target.Name} defeats {this.Name}!");

                    this.IsAlive = false;
                    return target;
                }

                // kraken is immune to spells
                if (target.Name == "Kraken")
                {
                    Console.WriteLine($"Spells don't affect {target.Name}, rendering {this.Name} useless! " +
                        $"{target.Name} defeats {this.Name}!");

                    this.IsAlive = false;
                    return target;
                }

                // trolls always win to normal enemies
                if (this.Element == ElementType.normal && target.Name == "Troll")
                {
                    Console.WriteLine($"Regular attacks and spell don't affect {target.Name}! {target.Name} defeats {this.Name}!");

                    this.IsAlive = false;
                    return target;
                }
            }
            if(this._spellType == SpellType.WaterSpell)
            {
                // knight always loses against waterspell
                if (target.Name == "Knight")
                {
                    Console.WriteLine($"{target.Name} drowned from {this.Name}'s attack! {this.Name} defeats {target.Name}!");

                    target.IsAlive = false;
                    return this;
                }

                // kraken is immune to spells
                if (target.Name == "Kraken")
                {
                    Console.WriteLine($"Spells don't affect {target.Name}, rendering {this.Name} useless! " +
                        $"{target.Name} defeats {this.Name}!");

                    this.IsAlive = false;
                    return target;
                }
            }

            // non special case battles
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

