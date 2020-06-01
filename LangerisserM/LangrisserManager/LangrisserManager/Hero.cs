using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangrisserManager
{
    class Hero
    {
        enum rarityID
        {
            R = 1,
            SR = 2,
            SSR = 3
        }   //稀有度，分为R,SR和SSR
        enum awakenID
        {
            未觉醒 = 0,
            一阶觉醒 = 1,
            二阶觉醒未开放 = 2,
            二阶觉醒 = 3
        }   //觉醒状态，分为未觉醒，一阶觉醒，二阶觉醒未开放和二阶觉醒
        enum classID
        {
            步兵,
            枪兵,
            骑兵,
            飞兵,
            水兵,
            工兵,
            刺客,
            僧侣,
            魔物
        }   //兵种，有步兵，枪兵，骑兵，飞兵，水兵，弓兵，刺客，僧侣和魔物

        public int ID { get; set; }
        public string EName { get; set; }
        public string CName { get; set; }
        private rarityID rarity;
        public string Rarity
        {
            get
            {
                //Property get code.
                return rarity.ToString();
            }
            set
            {
                //Property set code.
                rarity = (rarityID)Enum.Parse(typeof(rarityID), value);
            }
        }
        public int RarityID
        {
            get
            {
                //Property get code.
                return (int)rarity;
            }
        }
        public int Stars { get; set; }
        private awakenID awaken;
        public string AwakenState
        {
            get
            {
                //Property get code.
                return awaken.ToString();
            }
            set
            {
                //Property set code.
                awaken = (awakenID)Enum.Parse(typeof(awakenID), value);
            }
        }
        public string Importance { get; set; }
        private classID heroClass;
        public string HeroClass
        {
            get
            {
                //Property get code.
                return heroClass.ToString();
            }
            set
            {
                //Property set code.
                heroClass = (classID)Enum.Parse(typeof(classID), value);
            }
        }
        public int Power { get; set; }
        public int PortraitID { get; set; }
    }
}
