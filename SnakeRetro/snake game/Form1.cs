using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Media;// using para sonidos

namespace snake_game
{
    public partial class Form1 : Form
    {
        SoundPlayer player = new SoundPlayer();// objeto para sonido
        Random randcomida = new Random(); // objeto para determinar pociosion de la comida
        Graphics papel;// crea el campo de juego
        snake snakes = new snake(); //crea la serpiente
        Comida comida; // crea la comida
        bool izquierda = false;
        bool derecha = false;
        bool arriba = false;
        bool abajo = false;
        int score = 0;// --- puntuacion
        int ModoJuego = 1;
        int Seg = 0;
        int Min = 0;
        int UnSegundo = 10;
        public Form1()
        {
            InitializeComponent();
            // mostrar pantalla de reglas
            DificultadSnake();
            reglas regla = new reglas();
            regla.Show();
            comida = new Comida(randcomida);//posiciona la comida
        }
        // evento paint (dibuja la serpiente y la comida)
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            papel = e.Graphics;
            snakes.velocidad();

            comida.dibujodecomida(papel);
            snakes.dibujaSnake(papel);
           
        }

        public void DificultadSnake()
        {
            Dificultad dificultad = new Dificultad();
            if (dificultad.configuracionDificultad() == 2)
                lblNivDif.Text = "Normal";
            else if (dificultad.configuracionDificultad() == 1)
                lblNivDif.Text = "Fácil";
            else if (dificultad.configuracionDificultad() == 3)
                lblNivDif.Text = "Dificil";

            if (dificultad.ModoJuego() == 1)
                ModoJuego = 1;
            if (dificultad.ModoJuego() == 2)
            {
                ModoJuego = 2;
                dificultad.Cronometro();
                
                Seg = dificultad.Seg;
                Min = dificultad.Min;

                lblTemporizador.Visible = true;

                lblTemporizador.Text = Min.ToString() + ":" + Seg.ToString();
            }

        }
        // evento de las teclas
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // si preciona la barra space (con la que inicia el juego)
            if (e.KeyData == Keys.Space)
            {
                player.SoundLocation = "C:/Users/Moises (Chá)/Desktop/universidad/6to Semestre/Ingeniería de Software 2022-1/csharpproyect/SnakeRetro/snake game/Resources/ini.wav";// direccion del sonido
                player.Play();// suena sonido

                timer1.Enabled = true;
                tutosymaslabel.Text= "";
                spaceBarLabel.Text = "";
                abajo = false;
                arriba = false;
                izquierda = false;
                derecha = true;
            }
            // si preciona abajo
            if (e.KeyData == Keys.Down && arriba == false)
            {
                abajo = true;
                arriba = false;
                derecha = false;
                izquierda = false;
            }
            // si presiona arriba 
            if (e.KeyData == Keys.Up && abajo == false)
            {
                abajo = false;
                arriba = true;
                derecha = false;
                izquierda = false;
            }
            // si presiona izquierda
            if (e.KeyData == Keys.Left && derecha == false)
            {
                abajo = false;
                arriba = false;
                derecha = false;
                izquierda = true;
            }
            // si presiona derecha
            if (e.KeyData == Keys.Right && izquierda == false)
            {
                abajo = false;
                arriba = false;
                derecha = true;
                izquierda = false;
            }
        }

        // uso del timer para controlar el juego
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblScore.Text = score.ToString();// indica la puntuacion actual

            if (abajo) {
                snakes.movimientoabajo(); // mueve la serpiente hacia abajo
            }
            if (arriba) {
                snakes.movimientoarriba(); // mueve la serpiente hacia arriba
            }
            if (derecha) {
                snakes.movimientoderecha(); // mueve la serpiente hacia la derecha
            }
            if (izquierda) {
                snakes.movimientoizquierda(); // mueve la serpiente hacia izquierda
            }
            
            this.Invalidate(); // "repinta el mapa" con esto vemos el movimiento de la serpiente 
            
            colision();// revisa si choca contra con alguna de las paredes
            
            //----- si no choca entonces crece si se topa con comida ------

            for (int i = 0; i < snakes.SnakeRec.Length; i++) // determina el tamaño de la serpiente
            {
                // si la serpiente choca con comida
                if (snakes.SnakeRec[i].IntersectsWith(comida.comidarec))
                {
                    player.SoundLocation = "C:/Users/Moises (Chá)/Desktop/universidad/6to Semestre/Ingeniería de Software 2022-1/csharpproyect/SnakeRetro/snake game/Resources/pop.wav";// direccion del sonido
                    player.Play();// suena sonido

                    score += 1; // la puntuacion sube de 1 en 1
                    snakes.crecimientodeSnake(); // la serpiente crece
                    comida.locaciondecomida(randcomida);// reaparece la comida en otro lugar

                    //Checar si es buena posicion este codigo
                    player.SoundLocation = "C:/Users/Moises (Chá)/Desktop/universidad/6to Semestre/Ingeniería de Software 2022-1/csharpproyect/SnakeRetro/snake game/Resources/pop.wav";
                    player.Play();
                }
            }

            if(ModoJuego == 2) 
            {
                UnSegundo--;
                if (UnSegundo == 0)
                {
                    UnSegundo = 10;
                    Seg--;
                }
                string Minu = Min.ToString();
                string Segu = Seg.ToString();
                if (Seg < 10) Segu = "0" + Seg.ToString();
                if (Min < 10) Minu = "0" + Min.ToString(); ;
                
                if(Seg == 0 && Min > 0)
                {
                    Min -= 1;
                    Seg = 60;
                }

                if(Seg <= 0 && Min <= 0)
                {
                    timer1.Stop();
                    MessageBox.Show("Se acabo", "GAME OVER", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                lblTemporizador.Text = Minu + ":" + Segu;


            }
        }

        public void colision() // si la serpiente choca con la pared
        {
            for (int i = 1; i < snakes.SnakeRec.Length; i++) // verifica el tamaño de snake
            {
                if (snakes.SnakeRec[0].IntersectsWith(snakes.SnakeRec[2])) // si choca con ella misma * (solo los primeros 3 cuadros)
                {
                    player.SoundLocation = "C:/Users/Moises (Chá)/Desktop/universidad/6to Semestre/Ingeniería de Software 2022-1/csharpproyect/SnakeRetro/snake game/Resources/boom.wav";// direccion del sonido
                    player.Play();// suena sonido
                    reiniciar(); // se reinicia el juego 
                }
            }
            if (snakes.SnakeRec[0].X < 170 || snakes.SnakeRec[0].X > 1000) // si choca ala izquierda o derecha 
            {
                player.SoundLocation = "C:/Users/Moises (Chá)/Desktop/universidad/6to Semestre/Ingeniería de Software 2022-1/csharpproyect/SnakeRetro/snake game/Resources/boom.wav";// direccion del sonido
                player.Play();// suena sonido
                reiniciar();// se reinicia el juego 

            }
            if (snakes.SnakeRec[0].Y < 180 || snakes.SnakeRec[0].Y > 550) // si choca abajor o arriba 
            {
                player.SoundLocation = "C:/Users/Moises (Chá)/Desktop/universidad/6to Semestre/Ingeniería de Software 2022-1/csharpproyect/SnakeRetro/snake game/Resources/boom.wav";
                player.Play();
                reiniciar();// se reinicia el juego 
            }

        
            
        }

        public void reiniciar()// se reinicia el juego 
        {
            timer1.Enabled = false; // timer se apaga
            snakes = new snake(); // se crea una nueva snake 
            MessageBox.Show("GAME OVER \n Puntuacion : "+score.ToString()); // mensaje de final del juego
            lblScore.Text = "0"; // reinicia la puntuacion en el label
            score = 0; // reinicia la puntuacion en la variable
            spaceBarLabel.Text = "Presiona Barra Espaciadora para comenzar"; //
            tutosymaslabel.Text = "Tutoriales y mas;"; //
        


        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        #region ingnorar
        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {
         
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }
        #endregion


        private void pictureBox3_Click(object sender, EventArgs e)
        {
            timer1.Stop(); // pausa del juego
            spaceBarLabel.Text = "                               --- Pausa ---";
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            timer1.Start(); // continuar jugando
            spaceBarLabel.Text = "";
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Close(); // cerrar juego
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            player.SoundLocation = "C:/Users/Moises (Chá)/Desktop/universidad/6to Semestre/Ingeniería de Software 2022-1/csharpproyect/SnakeRetro/snake game/Resources/pop.wav";
            player.Play();
            // mostrar pantalla de reglas
            reglas regla = new reglas();
            regla.Show();
        }

        
    
    }
}
