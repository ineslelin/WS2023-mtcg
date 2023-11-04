using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.FightLogic.Enums;

namespace ws2023_mtcg.FightLogic.Dictionaries
{
    public class DElementDependency
    {
        public DElementDependency()
        {

        }

        public Dictionary<Tuple<ElementType, ElementType>, ElementType> ElementDependencies = new Dictionary<Tuple<ElementType, ElementType>, ElementType>()
        {
            { new Tuple<ElementType, ElementType>(ElementType.fire, ElementType.normal), ElementType.fire },
            { new Tuple<ElementType, ElementType>(ElementType.normal, ElementType.fire), ElementType.fire },
            { new Tuple<ElementType, ElementType>(ElementType.water, ElementType.normal), ElementType.normal },
            { new Tuple<ElementType, ElementType>(ElementType.normal, ElementType.water), ElementType.normal },
            { new Tuple<ElementType, ElementType>(ElementType.fire, ElementType.water), ElementType.water },
            { new Tuple<ElementType, ElementType>(ElementType.water, ElementType.fire), ElementType.water },
        };
    }

}
