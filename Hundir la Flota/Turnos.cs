using System;
using System.IO;
using System.Threading;	

namespace Hundir_la_Flota{
	public class Turnos{
		
		Program ppal = new Program();
		ColocacionDeBarcos coloc = new ColocacionDeBarcos();
		System.Media.SoundPlayer player = new System.Media.SoundPlayer();
		
		private bool validar = false, repetirTirada = true, sonido = false;
		private int[,] tableroJ1 = new int[6,8], tableroJ2 = new int[6,8], posJ2 = new int[6,8];
		private int fila = 0, columna = 0;	
		private int barcosJ1 = 0, barcosJ2 = 0, hundidoJ1 = 0, hundidoJ2 = 0, rondasJ1 = 0, rondasJ2 = 0;	
		private string nombreJ1 = null, nombreJ2 = "Máquina";
		
		public void setNombreJ1(string nombreJ1){
			this.nombreJ1 = nombreJ1;
		}
		
		public void setNombreJ2(string nombreJ2){
			this.nombreJ2 = nombreJ2;
		}
		
		public void setTablero1(int[,] tableroJ1){
			this.tableroJ1 = tableroJ1;
		}
		
		public void setTablero2(int[,] tableroJ2){
			this.tableroJ2 = tableroJ2;
		}
		
		public void setPosJ2(int[,] posJ2){
			this.posJ2 = posJ2;
		}
		
		public void setBarcosJ1(int barcosJ1){
			this.barcosJ1 = barcosJ1;
		}
		
		public void setBarcosJ2(int barcosJ2){
			this.barcosJ2 = barcosJ2;
		}
		
		public void setSonido(bool sonido){
			this.sonido = sonido;
		}
		
		public bool getRepetirTirada(){
			return repetirTirada;
		}
		
		public int getFila(){
			return fila;
		}
		
		public int getColumna(){
			return columna;
		}
		
		public int getBarcosJ1(){
			return barcosJ1;
		}
		
		public int getBarcosJ2(){
			return barcosJ2;
		}
		
		public void introducirFila(){
			do{	
				Console.Clear();
				Console.Write("\n Mapa J1 - "+nombreJ1);
				barraDeNombreJ1();
					for(int i=0;i<tableroJ1.GetLength(0);i++){			// Muestra los barcos J1
						for(int j=0;j<tableroJ1.GetLength(1);j++){   
							if(tableroJ1[i,j]==1){
								tableroJ1[i,j]=(char)'B';
							}
							if(tableroJ1[i,j]==0){	
								tableroJ1[i,j]=(char)'-';
							}
							Console.Write("  "+(char)tableroJ1[i,j]);
						}
						Console.Write("\n");
					}				
				Console.Write("\n\n Mapa J2 - "+nombreJ2);
				barraDeNombreJ2();	
					for(int i=0;i<tableroJ2.GetLength(0);i++){			// Muestra los barcos J2
						for(int j=0;j<tableroJ2.GetLength(1);j++){
							posJ2[i,j]=0;							
							if(tableroJ2[i,j]==1){						   // Si hay un barco
								posJ2[i,j]=tableroJ2[i,j];				   // Guardo su posición
								tableroJ2[i,j]=(char)'-';				   // y muestro agua por pantalla
							}
							if(tableroJ2[i,j]==0){	
								tableroJ2[i,j]=(char)'-';
							}
							Console.Write("  "+(char)tableroJ2[i,j]);
						}
						Console.Write("\n");
					}						
					//------
					for(int i=0;i<tableroJ2.GetLength(0);i++){
						for(int j=0;j<tableroJ2.GetLength(1);j++){
							if(posJ2[i,j]==1){
								tableroJ2[i,j]=posJ2[i,j];				// Revivo los barcos J2 (antes los borré del mapa)
							}					
						}
					}
				Console.SetCursorPosition(32, 1);Console.Write("Turno J1 - "+nombreJ1);
				Console.SetCursorPosition(32, 2);barraNombreTurnoJ1();
				Console.SetCursorPosition(32, 4);Console.Write("Barcos: "+barcosJ1);
				Console.SetCursorPosition(32, 5);Console.Write("Has hundido: "+hundidoJ1);
				Console.SetCursorPosition(32, 6);Console.Write("Te han hundido: "+hundidoJ2);
				Console.SetCursorPosition(32, 7);Console.Write("Rondas: "+rondasJ1);		
				Console.SetCursorPosition(32, 9);Console.Write("Atacar J1  ==> Fila: ");				
				validar = int.TryParse(Console.ReadLine(), out fila); 
			}while(fila<1 || fila>6);
		}
		
		public void introducirColumna(){
			do{	
				Console.Clear();
				Console.Write("\n Mapa J1 - "+nombreJ1);
				barraDeNombreJ1();
					for(int i=0;i<tableroJ1.GetLength(0);i++){			
						for(int j=0;j<tableroJ1.GetLength(1);j++){
							if(tableroJ1[i,j]==1){	
								tableroJ1[i,j]=(char)'B';
							}
							if(tableroJ1[i,j]==0){	
								tableroJ1[i,j]=(char)'-';
							}
							Console.Write("  "+(char)tableroJ1[i,j]);
						}
						Console.Write("\n");
					}				
				Console.Write("\n\n Mapa J2 - "+nombreJ2);
				barraDeNombreJ2();				
					for(int i=0;i<tableroJ2.GetLength(0);i++){			
						for(int j=0;j<tableroJ2.GetLength(1);j++){
							if(tableroJ2[i,j]==1){						   // Si hay un barco
								posJ2[i,j]=tableroJ2[i,j];				   // Guardo su posición
								tableroJ2[i,j]=(char)'-';				   // y muestro agua por pantalla
							}
							if(tableroJ2[i,j]==0){	
								tableroJ2[i,j]=(char)'-';
							}
							Console.Write("  "+(char)tableroJ2[i,j]);
						}
						Console.Write("\n");
					}						
					//------
					for(int i=0;i<tableroJ2.GetLength(0);i++){
						for(int j=0;j<tableroJ2.GetLength(1);j++){
							if(posJ2[i,j]==1){
								tableroJ2[i,j]=posJ2[i,j];				
							}					
						}
					}	
				Console.SetCursorPosition(32, 1);Console.Write("Turno J1 - "+nombreJ1);
				Console.SetCursorPosition(32, 2);barraNombreTurnoJ1();	
				Console.SetCursorPosition(32, 4);Console.Write("Barcos: "+barcosJ1);
				Console.SetCursorPosition(32, 5);Console.Write("Has hundido: "+hundidoJ1);
				Console.SetCursorPosition(32, 6);Console.Write("Te han hundido: "+hundidoJ2);
				Console.SetCursorPosition(32, 7);Console.Write("Rondas: "+rondasJ1);		
				Console.SetCursorPosition(32, 9);Console.Write("Atacar J1  ==> Fila: "+fila);				
				Console.SetCursorPosition(32, 10);Console.Write("           ==> Columna: ");				
				validar = int.TryParse(Console.ReadLine(), out columna); 
	
			}while(columna<1 || columna>8);
			Console.Write("\n\n");
		}
		
		public void realizarComprobacionesJ1(){			
			repetirTirada = true;
			if(tableroJ2[fila-1,columna-1] == 1){								// Realiza el ataque a la posición indicada
				Console.SetCursorPosition(32, 12);Console.Write("Barco Hundido. Vuelves a atacar J1.\n");
				tableroJ2[fila-1,columna-1]=(char)'X';
					if(sonido == true){
						player.SoundLocation = ".\\sonido\\Explosion.wav";
						player.Play();
					}
				hundidoJ1++;
				barcosJ2--;
			}else if(tableroJ2[fila-1,columna-1] == (char)'A'){
					if(sonido == true){
						player.SoundLocation = ".\\sonido\\PosicionYaDada.wav";
						player.Play();
					}
				Console.SetCursorPosition(32, 12);Console.Write("Ya has atacado esta posición. Pasas turno.");
			}else if(tableroJ2[fila-1,columna-1] == (char)'X'){
					if(sonido == true){
						player.SoundLocation = ".\\sonido\\PosicionYaDada.wav";
						player.Play();
					}
				Console.SetCursorPosition(32, 12);Console.Write("Ya has hundido este barco. Pasas turno.");
				repetirTirada = false;
			}else{
				if(sonido == true){
					player.SoundLocation = ".\\sonido\\NoHayBarco.wav";
					player.Play();
				}
				Console.Clear();
				tableroJ2[fila-1,columna-1] = (char)'A';
				Console.Write("\n Mapa J1 - "+nombreJ1);
				barraDeNombreJ1();
					for(int i=0;i<tableroJ1.GetLength(0);i++){			
						for(int j=0;j<tableroJ1.GetLength(1);j++){
							Console.Write("  "+(char)tableroJ1[i,j]);
						}
						Console.Write("\n");
					}			
				Console.Write("\n\n Mapa J2 - "+nombreJ2);
				barraDeNombreJ2();
					for(int i=0;i<tableroJ2.GetLength(0);i++){			
						for(int j=0;j<tableroJ2.GetLength(1);j++){
							if(tableroJ2[i,j]==1){						   // Si hay un barco
								posJ2[i,j]=tableroJ2[i,j];				   // Guardo su posición
								tableroJ2[i,j]=(char)'-';				   // y muestro agua por pantalla
							}
							if(tableroJ2[i,j]==0){	
								tableroJ2[i,j]=(char)'-';
							}
							Console.Write("  "+(char)tableroJ2[i,j]);
						}
						Console.Write("\n");
					}												
						//------
					for(int i=0;i<tableroJ2.GetLength(0);i++){
						for(int j=0;j<tableroJ2.GetLength(1);j++){
							if(posJ2[i,j]==1){
								tableroJ2[i,j]=posJ2[i,j];				
							}					
						}
					}					
				Console.SetCursorPosition(32, 1);Console.Write("Turno J1 - "+nombreJ1);
				Console.SetCursorPosition(32, 2);barraNombreTurnoJ1();
				Console.SetCursorPosition(32, 4);Console.Write("Barcos: "+barcosJ1);
				Console.SetCursorPosition(32, 5);Console.Write("Has hundido: "+hundidoJ1);
				Console.SetCursorPosition(32, 6);Console.Write("Te han hundido: "+hundidoJ2);
				Console.SetCursorPosition(32, 7);Console.Write("Rondas: "+rondasJ1);		
				Console.SetCursorPosition(32, 9);Console.Write("Atacar J1  ==> Fila: "+fila);				
				Console.SetCursorPosition(32, 10);Console.Write("           ==> Columna: "+columna);	
				Console.SetCursorPosition(32, 12);Console.Write("Has tocado agua. Pasas turno a J2.\n");	
			}				
			rondasJ1++;
			Thread.Sleep(2000);
		}
		
		public void ataqueAlAzarDeJ2(){
			Console.Clear();
			Random r = new Random(); 								// Elije Fila y Columna AL AZAR EN CADA RONDA
			int rfila7 = r.Next(1,6), rcolumna7 = r.Next(1,8);	
			Console.Clear();
			Console.Write("\n Mapa J1 - "+nombreJ1);
			barraDeNombreJ1();		
				for(int i=0;i<tableroJ1.GetLength(0);i++){			
					for(int j=0;j<tableroJ1.GetLength(1);j++){
						if(tableroJ1[i,j]==1){	
							tableroJ1[i,j]=(char)'B';
						}
						if(tableroJ1[i,j]==0){	
							tableroJ1[i,j]=(char)'-';
						}
						Console.Write("  "+(char)tableroJ1[i,j]);
					}
					Console.Write("\n");
				}		
			Console.Write("\n\n Mapa J2 - "+nombreJ2);
			barraDeNombreJ2();				
				for(int i=0;i<tableroJ2.GetLength(0);i++){			// Muestra los barcos J2
					for(int j=0;j<tableroJ2.GetLength(1);j++){
						posJ2[i,j]=0;
						if(tableroJ2[i,j]==1){						   // Si hay un barco
							posJ2[i,j]=tableroJ2[i,j];				   // Guardo su posición
							tableroJ2[i,j]=(char)'-';				   // y muestro agua por pantalla
						}
						if(tableroJ2[i,j]==0){	
							tableroJ2[i,j]=(char)'-';
						}
						Console.Write("  "+(char)tableroJ2[i,j]);
					}		
					Console.Write("\n");
				}						
			//------
				for(int i=0;i<tableroJ2.GetLength(0);i++){
					for(int j=0;j<tableroJ2.GetLength(1);j++){
						if(posJ2[i,j]==1){
							tableroJ2[i,j]=posJ2[i,j];				// Revivo los barcos J2 (antes los borré del mapa)
						}					
					}
				}
			Console.SetCursorPosition(32, 1);Console.Write("Turno J2 - "+nombreJ2);
			Console.SetCursorPosition(32, 2);barraNombreTurnoJ2();
			Console.SetCursorPosition(32, 4);Console.Write("Barcos: "+barcosJ2);
			Console.SetCursorPosition(32, 5);Console.Write("Has hundido: "+hundidoJ2);
			Console.SetCursorPosition(32, 6);Console.Write("Te han hundido: "+hundidoJ1);
			Console.SetCursorPosition(32, 7);Console.Write("Rondas: "+rondasJ2);
			Console.SetCursorPosition(32, 9);Console.Write("Atacar J2  ==> Fila: ");				// Atacar J2 a J1
			Thread.Sleep(1500);	
			Console.Write(rfila7);
			fila=rfila7;
			Console.SetCursorPosition(32, 10);Console.Write("           ==> Columna: ");
			Thread.Sleep(1500);
			Console.Write(rcolumna7);
			columna=rcolumna7;
			Thread.Sleep(1000);
			Console.Write("\n\n");	
		}
		
		public void realizarComprobacionesJ2(){
			repetirTirada = true;
			if(tableroJ1[fila-1,columna-1] == (char)'B'){
				Console.SetCursorPosition(32, 12);Console.Write("Barco Hundido. Vuelves a atacar jugador 2.\n");
				tableroJ1[fila-1,columna-1] = (char)'X';	
					if(sonido == true){
						player.SoundLocation = ".\\sonido\\Explosion.wav";
						player.Play();
					}				
				hundidoJ2++;
				barcosJ1--;					
			}else if(tableroJ1[fila-1,columna-1] == (char)'A'){
					if(sonido == true){
						player.SoundLocation = ".\\sonido\\PosicionYaDada.wav";
						player.Play();
					}
				Console.SetCursorPosition(32, 12);Console.Write("Ya has atacado esta posición. Pasas turno.");
			}else if(tableroJ1[fila-1,columna-1] == (char)'X'){
					if(sonido == true){
						player.SoundLocation = ".\\sonido\\PosicionYaDada.wav";
						player.Play();
					}
				Console.SetCursorPosition(32, 12);Console.Write("Ya has hundido este barco. Pasas turno.");
				repetirTirada = false;
			}else{
				if(sonido == true){
					player.SoundLocation = ".\\sonido\\NoHayBarco.wav";
					player.Play();
				}
				Console.SetCursorPosition(32, 12);Console.Write("Has tocado agua. Pasas turno a J1.\n");
				tableroJ1[fila-1,columna-1]=(char)'A';
			}
			rondasJ2++;
			Thread.Sleep(2000);
		}
		
		public void barraDeNombreJ1(){
			if(nombreJ1.Length == 10)		Console.Write("\n --------------------\n\n");
			else if(nombreJ1.Length == 9)	Console.Write("\n -------------------\n\n");
			else if(nombreJ1.Length == 8)	Console.Write("\n ------------------\n\n");
			else if(nombreJ1.Length == 7)	Console.Write("\n -----------------\n\n");	
			else if(nombreJ1.Length == 6)	Console.Write("\n ----------------\n\n");	
			else if(nombreJ1.Length == 5)	Console.Write("\n ---------------\n\n");	
			else if(nombreJ1.Length == 4)	Console.Write("\n --------------\n\n");	
			else if(nombreJ1.Length == 3)	Console.Write("\n -------------\n\n");	
			else if(nombreJ1.Length == 2)	Console.Write("\n ------------\n\n");	
			else if(nombreJ1.Length == 1)	Console.Write("\n -----------\n\n");	
		}
		public void barraDeNombreJ2(){
			if(nombreJ2.Length == 10)		Console.Write("\n --------------------\n\n");
			else if(nombreJ2.Length == 9)	Console.Write("\n -------------------\n\n");
			else if(nombreJ2.Length == 8)	Console.Write("\n ------------------\n\n");
			else if(nombreJ2.Length == 7)	Console.Write("\n -----------------\n\n");	
			else if(nombreJ2.Length == 6)	Console.Write("\n ----------------\n\n");	
			else if(nombreJ2.Length == 5)	Console.Write("\n ---------------\n\n");	
			else if(nombreJ2.Length == 4)	Console.Write("\n --------------\n\n");	
			else if(nombreJ2.Length == 3)	Console.Write("\n -------------\n\n");	
			else if(nombreJ2.Length == 2)	Console.Write("\n ------------\n\n");	
			else if(nombreJ2.Length == 1)	Console.Write("\n -----------\n\n");	
		}
		public void barraNombreTurnoJ1(){
			if(nombreJ1.Length == 10)		Console.Write("---------------------");
			else if(nombreJ1.Length == 9)	Console.Write("--------------------");
			else if(nombreJ1.Length == 8)	Console.Write("-------------------");
			else if(nombreJ1.Length == 7)	Console.Write("------------------");
			else if(nombreJ1.Length == 6)	Console.Write("-----------------");	
			else if(nombreJ1.Length == 5)	Console.Write("----------------");	
			else if(nombreJ1.Length == 4)	Console.Write("---------------");	
			else if(nombreJ1.Length == 3)	Console.Write("--------------");
			else if(nombreJ1.Length == 2)	Console.Write("-------------");	
			else if(nombreJ1.Length == 1)	Console.Write("------------");	
		}
		public void barraNombreTurnoJ2(){
			if(nombreJ2.Length == 10)		Console.Write("---------------------");
			else if(nombreJ2.Length == 9)	Console.Write("--------------------");
			else if(nombreJ2.Length == 8)	Console.Write("-------------------");
			else if(nombreJ2.Length == 7)	Console.Write("------------------");
			else if(nombreJ2.Length == 6)	Console.Write("-----------------");	
			else if(nombreJ2.Length == 5)	Console.Write("----------------");	
			else if(nombreJ2.Length == 4)	Console.Write("---------------");	
			else if(nombreJ2.Length == 3)	Console.Write("--------------");
			else if(nombreJ2.Length == 2)	Console.Write("-------------");	
			else if(nombreJ2.Length == 1)	Console.Write("------------");	
		}
		
		
		
	}
}
