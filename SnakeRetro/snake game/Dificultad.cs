using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace snake_game
{
    class Dificultad
    {
        public int Min = 3, Seg = 0;
        public int configuracionDificultad()
        {
            return 1;
        }

        public void Cronometro()
        {
            Min = 2;
            Seg = 50;
        }

        public int ModoJuego()
        {
            return 1;
        }

    }
}
