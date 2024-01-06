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

        // do those dependencies make sense? no
        // do i care? no, whomp whomp
        public Dictionary<Tuple<ElementType, ElementType>, ElementType> ElementDependencies = new Dictionary<Tuple<ElementType, ElementType>, ElementType>()
        {
            // FIRE EFFECTIVE AGAINST NORMAL AND ICE
            { new Tuple<ElementType, ElementType>(ElementType.fire, ElementType.normal), ElementType.fire },
            { new Tuple<ElementType, ElementType>(ElementType.normal, ElementType.fire), ElementType.fire },
            { new Tuple<ElementType, ElementType>(ElementType.fire, ElementType.ice), ElementType.fire },
            { new Tuple<ElementType, ElementType>(ElementType.ice, ElementType.fire), ElementType.fire },

            // NORMAL EFFECTIVE AGAINST WATER AND ELECTRIC
            { new Tuple<ElementType, ElementType>(ElementType.water, ElementType.normal), ElementType.normal },
            { new Tuple<ElementType, ElementType>(ElementType.normal, ElementType.water), ElementType.normal },
            { new Tuple<ElementType, ElementType>(ElementType.normal, ElementType.electric), ElementType.normal },
            { new Tuple<ElementType, ElementType>(ElementType.electric, ElementType.normal), ElementType.normal },

            // WATER EFFECTIVE AGAINST FIRE AND GRASS
            { new Tuple<ElementType, ElementType>(ElementType.fire, ElementType.water), ElementType.water },
            { new Tuple<ElementType, ElementType>(ElementType.water, ElementType.fire), ElementType.water },
            { new Tuple<ElementType, ElementType>(ElementType.water, ElementType.grass), ElementType.water },
            { new Tuple<ElementType, ElementType>(ElementType.grass, ElementType.water), ElementType.water },

            // ELECTRIC EFFECTIVE AGAINST FIRE, ICE AND WATER
            { new Tuple<ElementType, ElementType>(ElementType.electric, ElementType.ice), ElementType.electric },
            { new Tuple<ElementType, ElementType>(ElementType.ice, ElementType.electric), ElementType.electric },
            { new Tuple<ElementType, ElementType>(ElementType.electric, ElementType.water), ElementType.electric },
            { new Tuple<ElementType, ElementType>(ElementType.water, ElementType.electric), ElementType.electric },
            { new Tuple<ElementType, ElementType>(ElementType.electric, ElementType.fire), ElementType.electric },
            { new Tuple<ElementType, ElementType>(ElementType.fire, ElementType.electric), ElementType.electric },

            // ICE EFFECTIVE AGAINST WATER, GRASS AND NORMAL
            { new Tuple<ElementType, ElementType>(ElementType.ice, ElementType.grass), ElementType.ice },
            { new Tuple<ElementType, ElementType>(ElementType.grass, ElementType.ice), ElementType.ice },
            { new Tuple<ElementType, ElementType>(ElementType.normal, ElementType.ice), ElementType.ice },
            { new Tuple<ElementType, ElementType>(ElementType.ice, ElementType.normal), ElementType.ice },
            { new Tuple<ElementType, ElementType>(ElementType.water, ElementType.ice), ElementType.ice },
            { new Tuple<ElementType, ElementType>(ElementType.ice, ElementType.water), ElementType.ice },

            // GRASS IS EFFECTIVE AGAINST FIRE, NORMAL AND ELECTRIC
            { new Tuple<ElementType, ElementType>(ElementType.electric, ElementType.grass), ElementType.grass },
            { new Tuple<ElementType, ElementType>(ElementType.grass, ElementType.electric), ElementType.grass },
            { new Tuple<ElementType, ElementType>(ElementType.fire, ElementType.grass), ElementType.grass },
            { new Tuple<ElementType, ElementType>(ElementType.grass, ElementType.fire), ElementType.grass },
            { new Tuple<ElementType, ElementType>(ElementType.normal, ElementType.grass), ElementType.grass },
            { new Tuple<ElementType, ElementType>(ElementType.grass, ElementType.normal), ElementType.grass },
        };
    }

}
