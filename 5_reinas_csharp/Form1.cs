using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace _5_reinas_csharp
{
    public class Pos
    {
        public int x, y;
    }

    public partial class Form1 : Form
    {
        static Image reinaImg;

        static PictureBox[,] boxes;

        public Form1()
        {
            InitializeComponent();
            boxes = new PictureBox[,] {
                { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5 },
                { pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10 },
                { pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15},
                { pictureBox16, pictureBox17, pictureBox18, pictureBox19, pictureBox20},
                { pictureBox21, pictureBox22,pictureBox23, pictureBox24, pictureBox25}
            };
            string absolutePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"coronachida.png");
            reinaImg = Image.FromFile(absolutePath);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //elmein();
            backtrack();
        }

        private void Ajedrez_Load(object sender, EventArgs e)
        {

        }

        public static class GlobalData
        {
            public static int n = 5;
            public static int[] reinas = new int[n];
        }

        static async Task colocarReina(int i, bool solucion)
        {
            if (i >= GlobalData.n)
                return;

            GlobalData.reinas[i] = 0;

            int k = 0;
            do
            {
                GlobalData.reinas[i] = k;
                k++;
                solucion = false;

                mostrarAjedrez();
                await Task.Delay(1);
                await colocarReina(i + 1, solucion);
                estaSolucionado();
                //System.Threading.Thread.Sleep(1000);
                //if (esPosible(i))
                //{
                //await colocarReina(i + 1, solucion);
                //}

            } while ((k < GlobalData.n));
        }

        static bool esPosible(int i)
        {
            bool v = true;
            for (int t = 0; t < GlobalData.n; t++)
            {
                if (i != t && GlobalData.reinas[t] != -1 && GlobalData.reinas[i] != -1)
                {
                    if (GlobalData.reinas[i] == GlobalData.reinas[t])
                    {
                        return false;
                    }
                    if (Math.Abs(GlobalData.reinas[i] - GlobalData.reinas[t]) == Math.Abs(i - t))
                    {
                        return false;
                    }
                }
            }
            return v;
        }

        static bool esPosible2(int[,] tablero, int x, int y)
        {
            for (int i = 0; i < GlobalData.n; i++)
            {
                for(int j = 0; j < GlobalData.n; j++)
                {
                    if(tablero[i,j] == 1)
                    {
                        if(i == x || j == y || (Math.Abs(i-x) == Math.Abs(y-j)))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        static bool estaSolucionado()
        {
            for (int i = 0; i < GlobalData.n; i++)
            {
                if (GlobalData.reinas[i] != -1)
                {
                    if (!esPosible(i))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            MessageBox.Show("Solucion encontrada");
            return true;
        }

        static void mostrarAjedrez()
        {
            int[,] tablero = new int[GlobalData.n, GlobalData.n];


            for (int i = 0; i < GlobalData.n; i++)
            {
                for (int j = 0; j < GlobalData.n; j++)
                {
                    tablero[i, j] = 0;
                }
            }

            //Console.WriteLine("Mostrando arreglo");
            //for (int i = 0; i < GlobalData.n; i++)
            //{
                //Console.WriteLine("", GlobalData.reinas[i], "@");
            //}

            //Console.WriteLine();


            //VALOR DEL ARREGLO AL TABLERO
            for (int i = 0; i < GlobalData.n; i++)
            {
                for (int j = 0; j < GlobalData.n; j++)
                {
                    if (GlobalData.reinas[j] >= 0)
                    {
                        tablero[GlobalData.reinas[j], j] = 1;
                    }
                }
            }

            //MOSTRAR TABLERO
            //Console.WriteLine("MOSTRANDO TABLERO");
            for (int i = 0; i < GlobalData.n; i++)
            {
                for (int j = 0; j < GlobalData.n; j++)
                {
                    if(tablero[i, j] == 1)
                    {
                        //poner reina
                        boxes[i, j].Image = reinaImg;
                    }
                    else{
                        //Quitar reina
                        boxes[i, j].Image = null;
                    }
                    //System.Console.Write("", tablero[i, j], "@");
                }
                //Console.WriteLine("");
            }
            //Console.WriteLine("");
            
        }

        static void mostrarAjedrez2(int[,] tablero)
        {
            //MOSTRAR TABLERO
            //Console.WriteLine("MOSTRANDO TABLERO");
            for (int i = 0; i < GlobalData.n; i++)
            {
                for (int j = 0; j < GlobalData.n; j++)
                {
                    if (tablero[i, j] == 1)
                    {
                        //poner reina
                        boxes[i, j].Image = reinaImg;
                    }
                    else
                    {
                        //Quitar reina
                        boxes[i, j].Image = null;
                    }
                }
            };

        }

        static async void elmein()
        {
            bool flag = true;

            for (int i = 0; i < GlobalData.n; i++)
            {
                GlobalData.reinas[i] = -1;
            }

            await colocarReina(0, flag);

            mostrarAjedrez();
            MessageBox.Show("¡10 SOLUCIONES ENCONTRADAS!", "Finalizado");
        }

        static async Task<bool> placeQueen(int[,] tablero, int _n)
        {
            if (_n >= GlobalData.n)
            {
                MessageBox.Show("Solucion encontrada");
                return true;
            }

            bool ramaErronea = true;

            for (int i = 0; i < GlobalData.n; i++)
            {
                if(esPosible2(tablero, i, _n))
                {
                    tablero[i, _n] = 1;

                    mostrarAjedrez2(tablero);
                    await Task.Delay(100);

                    if (await placeQueen(tablero, _n + 1))
                    {
                        ramaErronea = false;
                        //return true;
                    }

                    tablero[i, _n] = 0;
                }

                /*for (int j = 0; j < GlobalData.n; j++)
                {
                    if(tablero[i,j] == 0) //esta esta celda vacia?
                    {
                        if(esPosible2(tablero, i,j)) //ninguna reina amenaza esta celda?
                        {
                            tablero[i, j] = 1;

                            mostrarAjedrez2(tablero);

                            await Task.Delay(100);

                            await placeQueen(tablero, _n + 1); 
                        }
                    }
                }*/
            }

            //Si llega aquí es una rama erronea
            //MessageBox.Show("Rama erronea");
            if(!ramaErronea)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Rama mala");
                return false;
            }
        }

        static async void backtrack()
        {
            int[,] tablero = new int[GlobalData.n, GlobalData.n];

            for (int i = 0; i < GlobalData.n; i++)
            {
                for (int j = 0; j < GlobalData.n; j++)
                {
                    tablero[i, j] = 0;
                }
            }
            
            mostrarAjedrez2(tablero);

            await placeQueen(tablero, 0);
        }
    }
}
