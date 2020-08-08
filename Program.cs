using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper
{
    class Program
    {
        public static GameManager gameManager;
        public static InputManager inputManager;

        static void Main(string[] args)
        {

            bool quitGame = false;

            while (quitGame == false)
            {
                gameManager = new GameManager();
                inputManager = new InputManager(gameManager);
                inputManager.GetInput();

                while (GameManager.gameIsOn)
                {

                    gameManager.Display();
                    inputManager.GetInput();

                }
                Console.WriteLine("Czy chcesz zagrac ponownie?");
                Console.WriteLine("T - tak");

                ConsoleKey key = Console.ReadKey().Key;

                if (key != ConsoleKey.T && key!=ConsoleKey.Enter) 
                {
                    quitGame = true;
                } 
            }
        }

    }

   
}
