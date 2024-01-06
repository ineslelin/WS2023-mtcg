using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.Models
{
    public class User
    {
        public int id;
        public string Username { get; set; }
        public string Password;
        public int Coins { get; set; }
        public int Elo { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public List<Cards> Stack = new List<Cards>();
        public List<Cards> Deck = new List<Cards>();

        public string Name { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }

        public User()
        {
            Coins = 20;
            Elo = 100;
            Wins = 0;
            Losses = 0;
        }

        //public void SetWinningELO()
        //{
        //    Elo += 3;
        //    Wins++;
        //}

        //public void SetLosingELO()
        //{
        //    Elo -= 5;
        //    Losses++;
        //}

        public void AddToStack(List<Cards> wonCards)
        {
            foreach (Cards w in wonCards)
            {
                if (!w.IsAlive)
                {
                    w.IsAlive = true;
                    Stack.Add(w);
                }
            }
        }

        public void RemoveFromStack(List<Cards> lostCards)
        {
            foreach (Cards w in lostCards)
            {
                if (!w.IsAlive)
                {
                    Stack.Remove(w);
                }
            }
        }
    }
}
