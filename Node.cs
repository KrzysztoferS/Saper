using System;
using System.Collections.Generic;

namespace Saper
{
    class Node
    {
        //zmienna przechowuje stan pola: empty - puste, bral sasiadujacych min; cyfra: ilosc min na polach sasiadujacych;  mina: pole zawira mine
        public string State
        {
            get;
            set;
        }

        //zmienna przechwuje stan pola tj. czy jest odsloniete czy zaslaoniete.
        public bool Visited
        {
            get;
            set;
        }

        //zmienna X przechowuje pozycje wezla w osi X
        public int X
        {
            get;
            set;
        }

        //zmienna Y przechowuje pozycje wezla w osi Y
        public int Y
        {
            get;
            set;
        }

        //czy pole jest oznaczone flaga jako potencjalne pole z mina
        public bool IsFlagged
        {
            get;
            set;
        }

        // Lista przechowuje referencje do wszytkich sasiadow danego wezla
        private List<Node> neighbours = new List<Node>();

        //przy towrzeniu nowego obiektu przypisuje mu pozycje X i Y;
        public Node(int x, int y, string state, bool visited)
        {
            this.X = x;
            this.Y = y;
            this.State = state;
            this.Visited = visited;
        }

        public Node(int x, int y, string state) : this(x, y, state, false) { }

        public Node(int x, int y) : this(x, y, "empty", false) { }

        //metoda ustala referencje do sasiadow i przechwuje je w liscie neighbours
        public void SetNeighbours(Node[,] nodesField)
        {
            
            //pobiera dlugos i szerokosc pola gry w celu okreslenia liczby sasiadow (Uwaga!!! GetUpperBound zwraca najwyzszy indeks elementu a nie ilosc elementow czyli w przypadku tablicy 10 elementowej zrwoci 9!)
            int x = nodesField.GetUpperBound(0);
            int y = nodesField.GetUpperBound(1);

            //Dodaje sasiadujace z danym wezlem pola, jesli te znajduja sie w obszarze gry
            //lewy gorny rog
            if (this.X-1>=0 && this.Y-1>=0) { neighbours.Add(nodesField[this.X - 1, this.Y-1]); }
            //gora
            if(this.Y-1>=0) { neighbours.Add(nodesField[this.X, this.Y - 1]); }
            //górny prawy róg
            if(this.X+1<=x && this.Y-1>=0) { neighbours.Add(nodesField[this.X + 1, this.Y - 1]); } 
            //lewy
            if(this.X-1>=0) { neighbours.Add(nodesField[this.X - 1, this.Y]); }
            //prawy
            if(this.X+1<=x){ neighbours.Add(nodesField[this.X + 1, this.Y]); }
            //lewy dolny
            if(this.X-1>=0 && this.Y + 1 <= y) { neighbours.Add(nodesField[this.X - 1, this.Y+1]); } 
            //dolny
            if(this.Y+1<=y) { neighbours.Add(nodesField[this.X, this.Y+1]); }
            //dol prawy
            if(this.X+1<=x && this.Y+1<=y) { neighbours.Add(nodesField[this.X + 1, this.Y+1]); }

            CheckNodeState();
        }

        //metoda zwraca liste sasiednich wezlow
        public List<Node> GetNeighbours()
        {
            //Powinna tez sprawdzac czy lista nie jest pusta
            return neighbours;
        }

        //sprawdza czy status wezla sie powinien zmienic z pustego na liczbe, jesli sasiaduje z minami
        private void CheckNodeState()
        {
            int numberOfniegboursMines = 0;
            //sprawdza czy nie powinien sie zmienic status,
           
                foreach (Node node in neighbours)
                {
                    if (node.State == "mine") numberOfniegboursMines++;
                }
                //jesli pole sasiaduje z minami i nie jest mina to zmienia status
                if (numberOfniegboursMines > 0 && this.State!="mine")
                {
                    this.State = numberOfniegboursMines.ToString();
                }
            
        }


    }
}
