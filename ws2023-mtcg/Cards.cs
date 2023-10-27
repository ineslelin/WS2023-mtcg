using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg
{
    internal abstract class Cards
    {
        public string Name { get; set; }
        public ElementType Element { get; set; }
        public CardType Type { get; set; }
        public int Damage { get; set; }
        public bool IsAlive;

        DElementDependency elementDependency = new DElementDependency();

        public Cards(string name, ElementType element, CardType type)
        {
            this.Name = name;
            this.Element = element;
            this.Type = type;

            Random random = new Random();
            this.Damage = random.Next(5, 100);

            IsAlive = true;
        }

        public abstract Cards Attack(Cards target);

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

            //if((source._element == ElementType.water && target._element == ElementType.fire) ||
            //   (source._element == ElementType.fire && target._element == ElementType.normal) ||
            //   (source._element == ElementType.normal && target._element == ElementType.water)) 
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

