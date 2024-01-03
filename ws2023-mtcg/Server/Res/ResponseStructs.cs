using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.FightLogic.Enums;

namespace ws2023_mtcg.Server.Res
{
    // just a bunch of structs based on the components part of the api
    struct UserCredentials
    {
        public string Username;
        public string Password;
    }

    struct UserData
    {
        public string Name;
        public string Bio;
        public string Image;
    }

    struct UserStats
    {
        public string Name;
        public int Elo;
        public int Wins;
        public int Losses;
    }

    struct UserScoreboard
    {
        public int Rank;
        public string Name;
        public int Elo;
    }

    struct Card
    {
        public string Id;
        public string Name;
        public double Damage;
    }

    struct TradingDeal
    {
        public string Id;
        public string CardToTrade;
        public CardType Type;
        public double MinimumDamage;
    }
}
