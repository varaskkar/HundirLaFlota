using System;
using System.IO;
using System.Threading;	

namespace Hundir_la_Flota{
	public class ColocacionDeBarcos{
		
		Program ppal = new Program();
		System.Media.SoundPlayer player = new System.Media.SoundPlayer();
		
		private bool validar = false, sonido = false;
		private int[,] tableroJ1 = new int[6,8], tableroJ2 = new int[6,8], posJ2 = new int[6,8];
		private int fila = 0, columna = 0, colocarBarco = 1;	// Fil + Colum = Agua (0)  || Barco (1)  || Hundido (2)
		private int barcosIniciales = 6, barcosJ1 = 0, barcosJ2 = 0;
		private string nombreJ1 = null;

		public int[,] getTableroJ1(){             
			return tableroJ1;
		} 
		
		public int[,] getTableroJ2(){
			return tableroJ2;
		} 
		
		public int[,] getPosJ2(){
			return posJ2;
		}
		
		public int getBarcosJ1(){
			return barcosJ1;
		} 
		
		public int getBarcosJ2(){
			return barcosJ2;
		}
		
		public void setBarcosIniciales(int barcosIniciales){
			this.barcosIniciales = barcosIniciales;
		}
		
		public void setNombreJ1(string nombreJ1){
			this.nombreJ1 = nombreJ1;
		}
		
		public void setSonido(bool sonido){
			this.sonido = sonido;
		}
			
		public void colocacionBarcosAutomatJ1(){
			menu();
			Console.Write("\n\n Cargando barcos");
			for(int i=0;i<barcosIniciales;i++){
				Random h = new Random(); 
				int hfila = h.Next(1,6), hcolumna = h.Next(1,8);
				fila=hfila; columna=hcolumna;
					if(tableroJ1[fila-1,columna-1] != colocarBarco){
						tableroJ1[fila-1,columna-1] = colocarBarco;
						barcosJ1++;
						Console.Write(".");
					}else{
						i--;
					}							
			}	
			Console.Write("Listo!");
			Thread.Sleep(1500);
		}
	
		public void colocacionBarcosManualJ1(){
		    for(int i=0;i<barcosIniciales;i++){
    			do{
					menu2();
					Console.SetCursorPosition(30, 11);Console.Write("Barquito velero "+(i+1));	
					Console.SetCursorPosition(30, 12);Console.Write("-----------------");
					Console.SetCursorPosition(30, 14);Console.Write("Fila: ");					
					validar = int.TryParse(Console.ReadLine(), out fila); 
				}while(fila<1 || fila>6);
							
				do{
					menu2();
					Console.SetCursorPosition(30, 11);Console.Write("Barquito velero "+(i+1));	
					Console.SetCursorPosition(30, 12);Console.Write("-----------------");
					Console.SetCursorPosition(30, 14);Console.Write("Fila: "+fila);	
					Console.SetCursorPosition(30, 15);Console.Write("Columna: ");							
					validar = int.TryParse(Console.ReadLine(), out columna); 
				}while(columna<1 || columna>10);

				if(tableroJ1[fila-1,columna-1] != (char)'B'){
					tableroJ1[fila-1,columna-1] = colocarBarco;  
					barcosJ1++;
					if(sonido == true){
						player.SoundLocation = ".\\sonido\\ColocarBarco.wav";
						player.Play();
					}
					menu2();
					Console.SetCursorPosition(30, 11);Console.Write("Barquito velero "+(i+1));	
					Console.SetCursorPosition(30, 12);Console.Write("-----------------");
					Console.SetCursorPosition(30, 14);Console.Write("Fila: "+fila);	
					Console.SetCursorPosition(30, 15);Console.Write("Columna: "+columna);
					Console.SetCursorPosition(30, 17);Console.Write("Barco "+(i+1)+" colocado correctamente!");
					Thread.Sleep(1250);
					
    			}else if(tableroJ1[fila-1,columna-1] == (char)'B'){ 	
					if(sonido == true){
						player.SoundLocation = ".\\sonido\\PosicionYaDada.wav";
						player.Play();
					}					
					menu2();
					Console.SetCursorPosition(30, 11);Console.Write("Barquito velero "+(i+1)+"\n\n");	
					Console.SetCursorPosition(30, 12);Console.Write("-----------------");
					Console.SetCursorPosition(30, 14);Console.Write("Fila: "+fila+"\n");	
					Console.SetCursorPosition(30, 15);Console.Write("Columna: "+columna);
					Console.SetCursorPosition(30, 17);Console.Write("Ya has colocado un barco en esta posición.");
					Thread.Sleep(1250);
					i--;
    			}
			} 	// Fin for		
		} // Fin método
		
		public void colocacionBarcosAutomatJ2(){
			for(int i=0;i<barcosIniciales;i++){
				Random r = new Random(); 
				int rfila = r.Next(1,6), rcolumna = r.Next(1,8);
				fila=rfila; columna=rcolumna;
					if(tableroJ2[fila-1,columna-1] != colocarBarco){
						tableroJ2[fila-1,columna-1] = colocarBarco;
						barcosJ2++;
					}else{
						i--;
					}							
			}
		}
		
		public void menu(){
			Console.Clear();
			Console.Write("  		_      _\n");
			Console.Write("  |_| | | |\\ | | \\  | |_|\n");
			Console.Write("  | | |_| | \\| |__| | | \\\n");
			Console.Write("       _     __	      _	  ___   __\n");
			Console.Write("  |   |_|   |__ |    | |   |   |__|\n");
			Console.Write("  |__ | |   |	|__  |_|   |   |  |\t\t  _|_\n");
			Console.Write("\t\t\t\t\t	___|___\n");
			Console.Write("\t\t\t\t\t	\\______/\n");
			Console.Write("__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\\n");
			Console.Write("\n   A B C D E F G H\t\t\n");
			Console.Write(" 1 - - - - - - - -\n");
			Console.Write(" 2 - - - - - - - -\n");
			Console.Write(" 3 - - - - - - - -\n");
			Console.Write(" 4 - - - - - - - -\n");
			Console.Write(" 5 - - - - - - - -\n");
			Console.Write(" 6 - - - - - - - -\n\n\n");
		}
		
		public void menu2(){
			Console.Clear();
			Console.Write("  		_      _\n");
			Console.Write("  |_| | | |\\ | | \\  | |_|\n");
			Console.Write("  | | |_| | \\| |__| | | \\\n");
			Console.Write("       _     __	      _	  ___   __\n");
			Console.Write("  |   |_|   |__ |    | |   |   |__|\n");
			Console.Write("  |__ | |   |	|__  |_|   |   |  |\t\t  _|_\n");
			Console.Write("\t\t\t\t\t	___|___\n");
			Console.Write("\t\t\t\t\t	\\______/\n");
			Console.Write("__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\__/\\");	
			Console.Write("\n\n");
			Console.Write("  Mapa de "+nombreJ1+"\n");
			Console.Write("  ---------------\n\n");
				for(int j=0;j<tableroJ1.GetLength(0);j++){			
					for(int k=0;k<tableroJ1.GetLength(1);k++){
						if(tableroJ1[j,k] == 1){	
							tableroJ1[j,k] = (char)'B';
						}
						if(tableroJ1[j,k] == 0){	
							tableroJ1[j,k] = (char)'-';
						}
						Console.Write("  "+(char)tableroJ1[j,k]);
					}
					Console.Write("\n");
				}		
		}
		
		
		
		
	} // Fin clase
} // Fin namespace
