using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg
{
    internal abstract class Cards
    {
        private string _name;
        private ElementType _element;
        private CardType _type;
        private int _damage;

        public Cards(string _name, ElementType _element, CardType _type, int _damage)
        {
            this._name = _name;
            this._element = _element;
            this._type = _type;
            this._damage = _damage;
        }

        public abstract void Attack();
    }
}
