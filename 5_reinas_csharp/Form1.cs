using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _5_reinas_csharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    }
}


/*#include <iostream>
#include <stdlib.h>
#include <vector>

using namespace std;

//DECLARACIONES DE FUNCIONES
void colocarReina(int, bool&);
bool esPosible(int);
bool estaSolucionado();
void mostrarAjedrez();
const int n = 5;
int reinas[n];

vector<int[n][n]> soluciones;

bool esPosible(int i) {
	bool v = true;
	for (int t = 0; t < n; t++) {
		if (i != t && reinas[t] != -1 && reinas[i] != -1)
		{
			if (reinas[i] == reinas[t]) {
				return false;
			}
			if (abs(reinas[i] - reinas[t]) == abs(i - t)) {
				return false;
			}
		}
	}

	

	return v;
}

void colocarReina(int i, bool &solucion) {
	if (i >= n)
		return;

	reinas[i] = 0;

	int k = 0;
	do {
		reinas[i] = k;
		k++;
		solucion = false;

		//mostrarAjedrez();

		if (esPosible(i)) {
			cout << "Es posible colocarlo ahí" << endl;
			colocarReina(i + 1, solucion);
		}

	} while (!estaSolucionado() && (k < n));
}

bool estaSolucionado() {
	for (int i = 0; i < n; i++) {
		if (reinas[i] != -1) {
			if (!esPosible(i)) {
				return false;
			}
		}
		else{
			return false;
		}
	}
	return true;
}

void mostrarAjedrez() {
	int tablero[n][n];

	//Asignando valores del arreglo a 0
	for (int i = 0; i < n; i++) {
		for(int j = 0; j < n; j++) {
			tablero[i][j] = 0;
		}
	}

	cout << "Mostrando arreglo" << endl;
	for (int i = 0; i < n; i++) {
		cout << reinas[i] << "|";
	}

	cout << endl;

	//VALOR DEL ARREGLO AL TABLERO
	for (int i = 0; i < n; i++) {
		for (int j = 0; j < n; j++) {
			if(reinas[j] >= 0)
				tablero[reinas[j]][j] = 1;
		}
	}


	//MOSTRAR TABLERO
	cout << "MOSTRANDO TABLERO" << endl;
	for (int i = 0; i < n; i++) {
		for (int j = 0; j < n; j++) {
			cout << tablero[i][j] << "|";
		}
		cout << endl;
	}
	cout << endl;

}

int main()
{
	bool solucion;

	for(int i = 0; i < n; i++)
	{
		reinas[i] = -1;
	}

	colocarReina(0, solucion);

	mostrarAjedrez();

	return 0;
}
*/