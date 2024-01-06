using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ws2023_mtcg.FightLogic.Dictionaries;
using ws2023_mtcg.FightLogic.Enums;

namespace ws2023_mtcg.Models
{
    public class Cards
    {
        public string Id;
        public string Name { get; set; }
        public ElementType Element { get; set; }
        public CardType Type { get; set; }
        public int Package;
        public string Owner;

        public double Damage { get; set; }
        public bool IsAlive;
        public bool IsCursed;

        public string output;

        readonly DElementDependency elementDependency = new DElementDependency();

        public Cards()
        {
            IsAlive = true;
            IsCursed = false;
        }

        public Cards Attack(Cards target)
        {
            this.IsAlive = true;
            target.IsAlive = true;

            if(this.Element == ElementType.normal && Regex.IsMatch(target.Name, @"(Fire|Regular|Water|Electric|Grass|Ice)?Troll"))
            {
                output += $"Regular attacks and spell don't affect {target.Name}! {target.Name} defeats {Name}!";

                IsAlive = false;
                return target;
            }

            if (Regex.IsMatch(Name, @"(Fire|Regular|Water|Electric|Grass|Ice)?Troll") && target.Element == ElementType.normal)
            {
                output += $"Regular attacks and spell don't affect {Name}! {Name} defeats {target.Name}!";

                IsAlive = false;
                return this;
            }

            if (Regex.IsMatch(Name, @"(Fire|Regular|Water|Electric|Grass|Ice)?Elf") && target.Name == "Dragon")
            {
                // elves always win agaisnt dragons
                output += $"Due to their age-old acquaintace {Name} knows how to evade all of {target.Name}'s attacks! " +
                    $"{Name} defeats {target.Name}!";

                target.IsAlive = false;
                return this;
            }

            if (Name == "Dragon")
            {
                // dragon always wins against goblin
                if (Regex.IsMatch(target.Name, @"(Fire|Water|Regular|Electric|Grass|Ice)?Goblin"))
                {
                    output += $"{target.Name} is too afraid of {Name} to attack! {Name} defeats {target.Name}!";

                    target.IsAlive = false;
                    return this;
                }

                // elf always defeats dragon
                if (Regex.IsMatch(target.Name, @"(Fire|Water|Regular|Electric|Grass|Ice)?Elf"))
                {
                    output += $"Due to their age-old acquaintace {target.Name} knows how to evade all of {Name}'s attacks! " +
                        $"{target.Name} defeats {Name}!";

                    IsAlive = false;
                    return target;
                }
            }

            if (Regex.IsMatch(Name, @"(Fire|Regular|Water|Electric|Grass|Ice)?Goblin") && target.Name == "Dragon")
            {
                // goblins always loose to dragons
                output += $"{Name} is too afraid of {target.Name} to attack! {target.Name} defeats {Name}!";

                IsAlive = false;
                return target;
            }

            if (Name == "Knight" && target.Name == "WaterSpell")
            {
                // knights always lose to waterspells
                output += $"{Name} drowned from {target.Name}'s attack! {target.Name} defeats {Name}!";

                IsAlive = false;
                return target;
            }

            if (Name == "Kraken" && target.Type == CardType.spell)
            {
                // the kraken is immune to spells
                output += $"Spells don't affect {Name}, rendering {target.Name} useless! " +
                        $"{Name} defeats {target.Name}!";

                target.IsAlive = false;
                return this;
            }

            if (Name == "Ork")  
            {
                if(target.Name == "Wizard")
                {
                    // orks always lose to wizards
                    output += $"{target.Name} took control of {Name}! {target.Name} defeats {Name}!";

                    IsAlive = false;
                    return target;
                }

                if (target.Name == "RegularSpell")
                {
                    output += $"{Name} defeats {target.Name}!";

                    target.IsAlive = false;
                    return this;
                }
            }

            if (Name == "Wizard")
            {
                // wizards always win against orks
                if (target.Name == "Ork")
                {
                    output += $"{Name} took control of {target.Name}! {Name} defeats {target.Name}!";

                    target.IsAlive = false;
                    return this;
                }

                // wizard always loses to firespells (which actually doenst make sense considering wizards are water types but shush)
                if (target.Name == "FireSpell")
                {
                    output += $"{Name}'s robes are very flammable! {target.Name} defeats {Name}!";

                    IsAlive = false;
                    return target;
                }
            }

            if (Type == CardType.spell)
            {
                // kraken is immune to spells
                if (target.Name == "Kraken")
                {
                    output += $"Spells don't affect {target.Name}, rendering {Name} useless! " +
                        $"{target.Name} defeats {Name}!";

                    IsAlive = false;
                    return target;
                }

                if (Name == "FireSpell")
                {
                    // wizard always loses to firespells (which actually doenst make sense considering wizards are water types but shush)
                    if (target.Name == "Wizard")
                    {
                        output += $"{target.Name}'s robes are very flammable! {Name} defeats {target.Name}!";

                        target.IsAlive = false;
                        return this;
                    }
                }

                if (Name == "RegularSpell")
                {
                    // orks are immune to regular spells
                    if (target.Name == "Ork")
                    {
                        output += $"{target.Name} defeats {Name}!";

                        IsAlive = false;
                        return target;
                    }
                }

                if (Name == "WaterSpell")
                {
                    // knight always loses against waterspell
                    if (target.Name == "Knight")
                    {
                        output += $"{target.Name} drowned from {Name}'s attack! {Name} defeats {target.Name}!";

                        target.IsAlive = false;
                        return this;
                    }
                }
            }


            // non special case battles
            if (Type == CardType.monster && target.Type == CardType.monster)
                return DamageFight(this, target);

            if (((Type == CardType.monster || Type == CardType.spell) && target.Type == CardType.curse) ||
                (Type == CardType.curse && (target.Type == CardType.monster || target.Type == CardType.spell)) ||
                (Type == CardType.curse && target.Type == CardType.curse))
                return CurseFight(this, target);

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
            if (source.Element == target.Element)
            {
                return DamageFight(source, target);
            }

            if (target.Element == elementDependency.ElementDependencies[Tuple.Create(source.Element, target.Element)])
            {
                if (source.Damage / 2 > target.Damage * 2)
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

        public Cards CurseFight(Cards source, Cards target)
        {
            if(source.Type == CardType.curse && target.Type != CardType.curse)
            {
                target.Damage /= 2;
                target.IsCursed = true;

                // card is deactivated after use
                source.IsAlive = false;
            }

            if (target.Type == CardType.curse && source.Type != CardType.curse)
            {
                source.Damage /= 2;
                source.IsCursed = true;

                target.IsAlive = false;
            }

            AnnounceWinner(source, target);

            return null;
        }

        public void AnnounceWinner(Cards source, Cards target)
        {
            switch (target.Type)
            {
                case CardType.monster when source.Type == CardType.monster:
                    output += source.Damage + " vs " + target.Damage + " => " + (source.Damage > target.Damage ? source.Name : target.Name)
                        + " defeats " + (source.Damage < target.Damage ? source.Name : target.Name) + "!";
                    break;

                case CardType.monster when source.Type == CardType.spell:
                case CardType.spell when source.Type == CardType.monster:
                case CardType.spell when source.Type == CardType.spell:
                    if (source.Element == target.Element)
                    {
                        output += source.Damage + " vs " + target.Damage + " => " + (source.Damage > target.Damage ? source.Name : target.Name)
                        + " defeats " + (source.Damage < target.Damage ? source.Name : target.Name) + "!";
                        break;
                    }

                    output += source.Damage + " vs " + target.Damage + " => ";

                    if (target.Element == elementDependency.ElementDependencies[Tuple.Create(source.Element, target.Element)])
                    {
                        output += source.Damage / 2 + " vs " + target.Damage * 2 + " => " + (source.Damage / 2 > target.Damage * 2 ? source.Name : target.Name) + " wins!";

                        break;
                    }

                    output += source.Damage * 2 + " vs " + target.Damage / 2 + " => " + (source.Damage * 2 > target.Damage / 2 ? source.Name : target.Name) + " wins!";

                    break;

                case CardType.monster when source.Type == CardType.curse:
                case CardType.spell when source.Type == CardType.curse:
                    output += $"{source.Name} cursed {target.Name} and halved it's damage! There's no winners this round!";
                    break;

                case CardType.curse when source.Type == CardType.monster:
                case CardType.curse when source.Type == CardType.spell:
                    output += $"{target.Name} cursed {source.Name} and halved it's damage! There's no winners this round!";
                    break;

                case CardType.curse when source.Type == CardType.curse:
                    output += "Since both cards are curse cards nothing happened! There's no winners this round!";
                    break;
            }
        }

        public double MegaBuff()
        {
            return Damage * 2;
        }

        public double ResetMegaBuff()
        {
            return Damage / 2;
        }
        public static List<Cards> ResetCurse(List<Cards> deck)
        {
            foreach(var d in deck)
            {
                if(d.IsCursed == true)
                {
                    d.Damage *= 2;
                }
            }

            return deck;
        }
    }
}

