using System;
using System.Collections.Generic;
using System.Text;


namespace Saper
{

    class InputManager
    {
        //ta klasa odpowiada za pobieranie inputu od gracza i przekazywania go do Game Manager'a

        //przechwowuje pozycje aktywnego wezla, czyli tego na kttorym jest kursor aktualnie
        public static int[] activeNode = { 0, 0 };

        //przechowuje referencje do obiektu gameManagera (nie do klasy);
        GameManager MyGameManager { get; set; }

        public InputManager( )
        {
            Console.WriteLine("Input manager stworzony"); 
        }

        public InputManager(GameManager gameManager) : base()
        {
            this.MyGameManager = gameManager;
        }

        public void GetInput()
        {
            int x = activeNode[0];
            int y = activeNode[1];
            int[] levelSize;
            //pobiera rozmiar planszy od game manager, jesli nie ma game managera to wychodzi z funkcji
            if (MyGameManager != null)
            {
                levelSize = MyGameManager.GetLevelSize();
            }
            else
            {
                Console.WriteLine("Brak obiektu Game Manager");
                return; 
            }

            
            ConsoleKey keyPressed = Console.ReadKey().Key;

            switch (keyPressed)
            {
                case ConsoleKey.UpArrow:
                   // Console.WriteLine("Nacisnieto strzalke w gore");
                    x--;
                    if (x < 0) x = 0;
                    break;
                case ConsoleKey.DownArrow:
                   // Console.WriteLine("Nacisnieto strzalke w dol");
                    x++;
                    if (x > levelSize[0] - 1) x = levelSize[0] - 1;
                    break;
                case ConsoleKey.LeftArrow:
                  //  Console.WriteLine("Nacisnieto strzalke w lewo");
                    y--;
                    if (y < 0) y = 0;
                    break;
                case ConsoleKey.RightArrow:
                   // Console.WriteLine("Nacisnieto strzalke w prawo");
                    y++;
                    if (y > levelSize[1] - 1) y = levelSize[1] - 1;
                    break;
                case ConsoleKey.Q:
                   // Console.WriteLine("  To chuj wychodze");
                    GameManager.gameIsOn = false;
                    break;
                case ConsoleKey.Enter:
                   // Console.WriteLine("Odkrywam pole");
                    MyGameManager.ShowNode(x, y);
                    break;
                case ConsoleKey.F:
                   // Console.WriteLine("Flaguje pole");       
                        this.MyGameManager.SetFlag(activeNode[0], activeNode[1]);
                    break;
                default:
                    GetInput();
                    break;
            }

            activeNode[0] = x;
            activeNode[1] = y;
        }

    }

}