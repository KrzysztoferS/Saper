using Saper.Intrfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Saper
{
    class GameManager
    {
        static public bool gameIsOn;
        static public int time=0;
        static public bool isDisplaing = false;
         
        //zmienne przechowuje wielkosc planszy, domyslnie 10x10
        private int[] levelSize = { 10, 10 };
        private int numberOfMines;
        private static Node[,] level;
        private double maxPercentOfMines = 0.3;
        //ile pol zostalo odslonietych
        private int numberOfVisitedNodes = 0;


        //konstruktor klasy pobiera wielkosc pola gry na starcie
        public GameManager()
        {
            gameIsOn = true;
            int height, width;
            Console.WriteLine("Podaj dlugosc pola gry (domyslnie 10):");
            int.TryParse(Console.ReadLine(), out height);

            if (height > 0) levelSize[0] = height;
            
            Console.WriteLine("Podaj szerokość pola gry(domyślnie 10):");
            int.TryParse(Console.ReadLine(), out width);

            if (width > 0) levelSize[1] = width;

            Console.WriteLine("Podaj ilosc min do rozstawienia:");
            int.TryParse(Console.ReadLine(), out numberOfMines);
            //ogranicza ilosc min na planszy do maksymalnie 5% wszystkich pol
            if (numberOfMines > (levelSize[0] * levelSize[1]) * maxPercentOfMines  || numberOfMines==0) numberOfMines = Convert.ToInt32(((levelSize[0] * levelSize[1]) * maxPercentOfMines));
         

            

            Console.WriteLine($"Gra ma pole {levelSize[0]}x{levelSize[1]} i zawiera {numberOfMines} min");
            
            CreateLevel();
            PutMinesOnPlace(numberOfMines);
            Console.ReadKey();
            Console.SetWindowSize((levelSize[1] + 2)*3, levelSize[0] +10);

            Console.Clear();

            Display();




            //odpala odliczanie czasu na odzielnym watku dzieki czemu nie blokuje glownej gry
            time = 0;
            Task timer = new Task(CountTime);
            timer.Start();
        }

        private void CreateLevel()
        {

            level = new Node[levelSize[0], levelSize[1]];

            for (int x = 0; x < levelSize[0]; x++)
            {
                for (int y = 0; y < levelSize[1]; y++)
                {
                    level[x, y] = new Node(x, y);
                }
            }

        }

        public void Display()
        {
            if (gameIsOn == false)
                return;

            if (isDisplaing == false)
            {
                isDisplaing = true;
                Console.SetCursorPosition(0, 0);
                //To mozna zamienic na petel uzalezniona od liczby poziomych elementow
                //Console.WriteLine(" A B C D E F G H I J K L M N O P Q R S T U V W X Y Z");
                Console.WriteLine($"Czas: {time}sek");
                for (int i = 0; i < levelSize[0]; i++)
                {


                    for (int z = 0; z < levelSize[1]; z++)
                    {
                        DrawSymbol(i, z);

                    }
                    //domyslne wartosci obszarow poza polem gry
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" " + i);
                    Console.WriteLine();

                    

                   
                }

                isDisplaing = false;

                Console.WriteLine("Użyj strzałek żeby poruszać się między polami");
                Console.WriteLine("Enter - by odsłonić pole");
                Console.WriteLine("f - by oflagować pole");
                Console.WriteLine("q- by wyjść");
            }
        }

        //metoda ustawia na planszy losowo miny i zmniena status sasiadujacych z minami wezlow
        private void PutMinesOnPlace(int numberOfMines)
        {
            //Lista przechowuje wszystkie Nody z planszy w liscie i z niej wybiera w ktory wezel wstawic bombe po czym usuwa ten wezel z listy by go ponownie nie wybrac
            List<Node> nodesList = new List<Node>();

            foreach (Node i in level)
            {
                nodesList.Add(i);
            }
            
            Random rnd = new Random();

            for (int i=numberOfMines; i>=1; i--)
            {
                
                int node = rnd.Next(0, nodesList.Count);
                //zmienia status wezla na mina
                nodesList[node].State = "mine";
                //usuwa wezel z liosty zeby nie zostal wybany ponownie
                nodesList.RemoveAt(node);
            }

            //ustawia sasiadow dla kazdego wezla
            foreach(Node i in level)
            {
                i.SetNeighbours(level);
            }
           

        }

        //metoda rysuje odpowieni symbol w zalezniosci od statusu wezla za input przyjmuje wsp. wezla
        private void DrawSymbol(int x, int y)
        {
            //przechwouje referencje do sprawdzanegeo wezla
            Node node = level[x, y];
            //przechowuje znak do narysowania
            string sign = "+";

            //Sprawdzic czy wezel odkryty i jesli nie to rysuje symbol nieodkrytego pola i czy nie oflagowany
            if (node.Visited == false)
            {
                if (node.IsFlagged == true)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    sign = "f";
                } else
                {
                    Console.BackgroundColor = ConsoleColor.Gray ;
                    Console.ForegroundColor = ConsoleColor.White;
                    sign = "+";
                }

            } else
            {
                if (node.State == "mine")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.DarkRed ;
                    sign = "*";
                }
                else if (node.State == "empty")
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Black;
                    sign = " ";
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    //ustawic rozny kolor dla roznych wartosci
                    switch (node.State)
                        {
                        case "1":
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case "2":
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case "3":
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                        case "4":
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                        case "5":
                            Console.ForegroundColor = ConsoleColor.DarkGreen ;
                            break;
                        case "6":
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case "7":
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        case "8":
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }

                    sign = node.State;
                }

            }
            /*
             * Jesli pozycja obecnego wezla jest taka jak pozycja podswietlonego wezla to zmienia kolor tla na tego wezla
             * */
            if (x == InputManager.activeNode[0] && y == InputManager.activeNode[1]) Console.BackgroundColor = ConsoleColor.Green;

            Console.Write($" {sign} ");
        }

        //metoda flaguje pole
        public void SetFlag(int x, int y)
        {
            if (level[x, y].IsFlagged == false)
                level[x, y].IsFlagged = true;
            else level[x, y].IsFlagged = false;
        }

        /* metoda odslania element i wywoluje metode decydujaca o tym co dale
         * jesli po odslonieciu jest mina to koniec gry
         * jesli jest pole z cyferka to odslania tylko to
         * jesli jest puste pole to odkrywa wszystkie sasiadujace puste pola az do pol z cyferkami
         * */
        public void ShowNode(int x, int y)
        {
            Node node = level[x, y];

            if (node.Visited == false)
            {
                node.Visited = true;
                numberOfVisitedNodes++;
                Display();

                if (node.State == "mine")
                {
                    //odpala metode game over
                    GameOver();
                } else if (node.State == "empty")
                {
                    //odpala metode przeszukujaca wglab przez wszystkie puste pola i sprawdza czy wygrana
                    OpenEmptyFields(x, y);
                    CheckIfWin();
                }
                else
                {
                    CheckIfWin();
                }

                CheckIfWin();
            }
        }

        public int[] GetLevelSize()
        {
            return levelSize;
        }

        public void CheckIfWin()
        {
            if (numberOfVisitedNodes + numberOfMines == level.Length)
            {
                Display();
                GameManager.gameIsOn = false;
                Console.WriteLine($"Wygrales! Zajeło ci to {time}sek. Gratulacje");
               
                Console.ReadKey();
            }
        }

        public void GameOver()
        {
            Console.WriteLine("Przegrales");
            gameIsOn = false;
            Console.ReadKey();
        }

        //metoda otwiera wszystkie puste saisadujace pola
        public void OpenEmptyFields(int x, int y)
        {
            //lista zawiera wezly ktore nalezy przeszukac
            List<Node> nodesToCheck = new List<Node>();
            Node currentNode = level[x, y];

            //przeszukuje sasiadow danego wezla i jesli nie sa otwarte to dodaje do listy
            foreach(Node i in currentNode.GetNeighbours())
            {
                if (i.Visited == false)
                {
                    nodesToCheck.Add(i);
                    i.Visited = true;
                    numberOfVisitedNodes++;
                    if(i.IsFlagged==false && i.State == "empty")
                    {
                        OpenEmptyFields(i.X, i.Y);
                    }
                }

            }


            
        }

        private async void CountTime()
        {
            while (gameIsOn)
            {
                time++;
                System.Threading.Thread.Sleep(1000);
                Display();
            }
        }
    }

}
