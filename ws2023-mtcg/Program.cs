using System;
using ws2023_mtcg.FightLogic;
using ws2023_mtcg.FightLogic.Enums;
using ws2023_mtcg.Server;
using ws2023_mtcg.Server.Repository;

namespace ws2023_mtcg
{
    class Program
    {
        static void Main(string[] args)
        {
            // DATABASE NAME: mtcgdb
            // USERNAME: admin
            // PASSWORD: 1234
            DatabaseHandler.Start();

            HttpServer server = new HttpServer(10001);
            server.Start();

            server.Stop();
        }
    }
}
