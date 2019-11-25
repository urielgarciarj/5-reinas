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
    public partial class Form1 : Form
    {
        static Image reinaImg;

        static PictureBox[,] boxes;

        public Form1()
        {
            InitializeComponent();

            btnPause.Hide();

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

        private void btnPause_Click(object sender, EventArgs e)
        {
            GlobalData.isRuning = !GlobalData.isRuning;

            btnPause.Text = GlobalData.isRuning ? "Pause" : "Continue";

            if(GlobalData.isRuning)
            {
                GlobalData.backtrackingPause?.SetResult(false);
            }
            else
            {
                GlobalData.backtrackingPause = new TaskCompletionSource<bool>();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Ajedrez_Load(object sender, EventArgs e)
        {

        }

        public static class GlobalData
        {
            public static int n = 5;
            public static int[] reinas = new int[n];
            public static bool isRuning = false;
            public static TaskCompletionSource<bool> backtrackingPause;
            public static BacktrackTree tree;
            public static string[] logLines = { };

            public static void Log(string _log)
            {
                string[] temp = logLines;
                logLines = new string[temp.Length + 1];

                for (int i = 0; i < temp.Length; i++)
                    logLines[i] = temp[i];

                logLines[temp.Length] = _log;

                string absolutePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"backtrack_log.txt");
                File.WriteAllText(absolutePath, String.Empty);
                File.WriteAllLines(absolutePath, logLines);
            }
        }

        static bool esPosible(int[,] tablero, int x, int y)
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

        int[] GetSolutionArray(int[,] board)
        {
            int[] result = new int[GlobalData.n];

            for(int i = 0; i < result.Length; i++)
            {
                for(int j = 0; i < result.Length; j++)
                {
                    if(board[i,j] == 1)
                    {
                        result[i] = j;
                        break;
                    }
                }
            }

            return result;
        }

        async Task<bool> placeQueen(int[,] tablero, int _n)
        {
            if(!GlobalData.isRuning)
            {
                await GlobalData.backtrackingPause.Task;
            }

            if (_n >= GlobalData.n)
            {
                //Soución encontrada
                GlobalData.tree.InsertSolution(GetSolutionArray(tablero));
                GlobalData.Log("Solution found");

                GlobalData.isRuning = false;
                btnPause.Invoke(new MethodInvoker(delegate { btnPause.Text = GlobalData.isRuning ? "Pause" : "Continue"; }));
                GlobalData.backtrackingPause = new TaskCompletionSource<bool>();
                await GlobalData.backtrackingPause.Task;

                return true;
            }

            bool ramaErronea = true;

            for (int i = 0; i < GlobalData.n; i++)
            {
                if(esPosible(tablero, i, _n))
                {
                    GlobalData.Log("Position (" + i + ", " + _n + ") is avilable");
                    tablero[i, _n] = 1;

                    mostrarAjedrez2(tablero);
                    await Task.Delay(50);

                    if (await placeQueen(tablero, _n + 1))
                    {
                        ramaErronea = false;
                    }

                    tablero[i, _n] = 0;
                }
                else
                {
                    GlobalData.Log("Position (" + i + ", " + _n + ") is unavilable");
                    tablero[i, _n] = 1;

                    mostrarAjedrez2(tablero);

                    pbFailure.Invoke(new MethodInvoker(delegate { pbFailure.Show(); }));
                    await Task.Delay(50);
                    pbFailure.Invoke(new MethodInvoker(delegate { pbFailure.Hide(); }));

                    tablero[i, _n] = 0;
                }
            }
            
            if(!ramaErronea)
            {
                return true;
            }
            else
            {
                GlobalData.Log("Got to the end of a branch without finding a solution");
                pbFailure.Invoke(new MethodInvoker(delegate { pbFailure.Show(); }));
                await Task.Delay(50);
                pbFailure.Invoke(new MethodInvoker(delegate { pbFailure.Hide(); }));

                return false;
            }
        }

        async void backtrack()
        {
            btnPause.Invoke(new MethodInvoker(delegate { btnPause.Show(); }));

            int[,] tablero = new int[GlobalData.n, GlobalData.n];

            for (int i = 0; i < GlobalData.n; i++)
            {
                for (int j = 0; j < GlobalData.n; j++)
                {
                    tablero[i, j] = 0;
                }
            }
            GlobalData.Log("Board initialized: width = "+ tablero.GetLength(0) + ", height = " + tablero.GetLength(1));

            GlobalData.tree = new BacktrackTree(GlobalData.n);
            GlobalData.Log("Tree Initialized: width = "+ GlobalData.tree.Depth + ", depth = " + GlobalData.tree.Depth);

            mostrarAjedrez2(tablero);

            GlobalData.isRuning = true;

            await placeQueen(tablero, 0);

            //writeBacktrackLog();

            MessageBox.Show("Soluciones encontradas");

            btnPause.Invoke(new MethodInvoker(delegate { btnPause.Hide(); }));
        }

        void writeBacktrackLog()
        {
            string absolutePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"backtrack_log.txt");
            File.WriteAllText(absolutePath, String.Empty);
            File.WriteAllLines(absolutePath, GlobalData.logLines);
        }
    }
}
