using System;
using ws2023_mtcg.FightLogic;
using ws2023_mtcg.FightLogic.Enums;
using ws2023_mtcg.Server;

namespace ws2023_mtcg
{
    class Program
    {
        static void Main(string[] args)
        {
            // 10001 as port bc they use that in the curl scripts
            HttpServer server = new HttpServer(10001);
            server.Start();

            Menu menu = new Menu();
            menu.Run();

            server.Stop();
        }
    }
}
