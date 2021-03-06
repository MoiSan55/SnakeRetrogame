using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace snake_game
{
    public class Comida
    {
        private int x, y, width, height; // posicion y coordenadas
        private SolidBrush pincel;// dibuja la comida
        public Rectangle comidarec;//la comida

        public Comida(Random randFood) // creacion de la primera comida
        {
            x = randFood.Next(18, 87) * 10;//180; 190, 970; 540 ---- 790, 350
            y = randFood.Next(19, 54) * 10; //1000; 550
            pincel = new SolidBrush(Color.Red); // color de la comida
            width = 10;
            height= 10;
            comidarec = new Rectangle(x, y, width, height);
}
        public void locaciondecomida(Random randFood) // posicion de la comida
        {
            x = randFood.Next(18, 87) * 10;
            y = randFood.Next(19, 54) * 10;
        
        }

        public void dibujodecomida(Graphics paper)// dibujo de la comida en el papel (campo de juego)
        {
            comidarec.X = x;
            comidarec.Y = y;
            paper.FillRectangle(pincel, comidarec);
        }
        
    }
}
