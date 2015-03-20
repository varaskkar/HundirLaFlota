using System.IO;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System;
using System.Net.Sockets;
using System.Net;

using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data.SqlTypes;
using System.Runtime.Serialization.Formatters.Binary;

// TODO 1º ataca J1 y luego J2, y viceversa(mientras uno no ataca espera)
// TODO pasar el array de los barcos de los 2 jugadores

namespace Hundir_la_Flota{
	public class Cliente{
		
		// Servidor - Sólo escucha
        Socket sS = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    	IPEndPoint ipS = new IPEndPoint (IPAddress.Any, 6000);
    	// Cliente - Sólo envía
   		Socket sC = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); 
        IPEndPoint ipC = new IPEndPoint (IPAddress.Parse ("127.0.0.1"), 1238); 
        // Serialización
//        NetworkStream net = new NetworkStream(ipC.AddressFamily, SocketType.Stream, ProtocolType.Tcp));
        

        
		
		ColocacionDeBarcos colocBarcos = new ColocacionDeBarcos();
		Turnos turnos = new Turnos();
		System.Media.SoundPlayer player = new System.Media.SoundPlayer();

	    bool validar = false, volverAtras = true, sonido = false;
		int opcion1 = 0, opcion2 = 0, opcion3 = 0, barcosIniciales = 6;	
		string nombreJ1 = null, nombreJ2 = null;
		
		private int[,] tableroJ1 = new int[6,8], tableroJ2 = new int[6,8], posJ2 = new int[6,8];
		
		public Cliente(){  }
		
		public void setTablero1(int[,] tableroJ1){
			this.tableroJ1 = tableroJ1;
		}
		
		public int[,] getTableroJ2(){
			return tableroJ2;
		}
		
		public void cliente(){  
			
	        Console.SetCursorPosition(24, 18);Console.WriteLine("Conectandose al servidor...");   
	
 	       	try {
	        	
    	        sC.Connect (ipC);	    	        
               	sS.Bind (ipS);
	    	   	sS.Listen (25);
			   	Socket accepted = sS.Accept();
			   	
			   	Console.SetCursorPosition(24, 18);Console.WriteLine("Conexión establecida con éxito!"); 
				NetworkStream net = new NetworkStream(sC);		

				
				
				
				
	           	Thread.Sleep(1000);

				do{ 
	                colocBarcos.menu();
					Console.SetCursorPosition(24, 11);Console.Write("Introduce tu nombre: ");
					nombreJ1 = Console.ReadLine();
						while(nombreJ1.Length < 1 || nombreJ1.Length > 10){
							while(nombreJ1.Length > 10){ 
								colocBarcos.menu();	
								Console.SetCursorPosition(24, 11);Console.Write("                         ");
								Console.SetCursorPosition(24, 11);Console.Write("Pon un nombre más corto: ");
								nombreJ1 = Console.ReadLine();
							}	
							while(nombreJ1.Length < 1){ 
								colocBarcos.menu();	
								Console.SetCursorPosition(24, 11);Console.Write("                         ");
								Console.SetCursorPosition(24, 11);Console.Write("Debes colocar un nombre: ");
								nombreJ1 = Console.ReadLine();
							}	
						}
					turnos.setNombreJ1(nombreJ1);
					colocBarcos.setNombreJ1(nombreJ1);
					Console.SetCursorPosition(24, 13);Console.Write("Coloca tus embarcaciones\n");
					Console.SetCursorPosition(24, 14);Console.Write("------------------------\n");
					Console.SetCursorPosition(24, 15);Console.Write("(1)  Automáticamente\n");
					Console.SetCursorPosition(24, 16);Console.Write("(2)  Manualmente\n");
					Console.SetCursorPosition(24, 18);Console.Write(">>> ");
					validar = int.TryParse(Console.ReadLine(), out opcion2); 		
				}while(opcion2!=1 && opcion2!=2);
				switch(opcion2){
					case 1:
						colocBarcos.colocacionBarcosAutomatJ1();
			    		break;
			  	  	case 2:	
			    		colocBarcos.colocacionBarcosManualJ1();
			       	 	break;	
				} 	
	
				turnos.setTablero1(colocBarcos.getTableroJ1());
				turnos.setTablero2(colocBarcos.getTableroJ2());
				turnos.setPosJ2(colocBarcos.getPosJ2());
				turnos.setBarcosJ1(colocBarcos.getBarcosJ1());
				turnos.setBarcosJ2(colocBarcos.getBarcosJ2());
					
				string text = "fin";	// Barcos colocados, le toca colocarlos al server
				byte[] data = Encoding.ASCII.GetBytes(text);
				sC.Send(data);    
				
				Thread.Sleep(1500);
				Console.SetCursorPosition(1, 22);Console.Write("Esperando a que el servidor coloque sus barcos...");
				
				string strData = null;
        		while(strData == null){	// Confirmamos que el server los ha colocado
	           		byte[] buffer = new Byte[255];
               		int rec = accepted.Receive(buffer,0,buffer.Length,0);
               		Array.Resize(ref buffer, rec); 
               		strData = Encoding.ASCII.GetString(buffer);
	    		}             

				byte[] data2 = Encoding.ASCII.GetBytes(nombreJ1);	// Envío el nombre
				sC.Send(data2); 
				
           		byte[] buffer2 = new Byte[255];						// Guardo su nombre
           		int rec2 = accepted.Receive(buffer2,0,buffer2.Length,0);
           		Array.Resize(ref buffer2, rec2); 
           		nombreJ2 = Encoding.ASCII.GetString(buffer2);
				turnos.setNombreJ2(nombreJ2);				
				
				BinaryFormatter bf = new BinaryFormatter();			// Envío el tablero
				bf.Serialize(net, tableroJ1);
			
				do{									// Cambia el turno de J1 a J2 y viceversa	
					do{								// Vuelve a tirar J1 si hunde un barco
						turnos.introducirFila();
						turnos.introducirColumna();
						turnos.realizarComprobacionesJ1();
					}while(colocBarcos.getTableroJ2()[turnos.getFila()-1,turnos.getColumna()-1] == (char)'X' && turnos.getRepetirTirada() == true);

//					string text = "tocaAlServer";	// Ya he atacado, le toca al server
//					byte[] data = Encoding.ASCII.GetBytes(text);
//					sC.Send(data); 
//					
//					byte[] buffer2 = new Byte[255];	// Espero que el server ataque 
//		       		int rec2 = accepted.Receive(buffer2,0,buffer2.Length,0);
//		       		Array.Resize(ref buffer2, rec2); 
//		       		nombreJ2 = Encoding.ASCII.GetString(buffer2);
//					turnos.setNombreJ2(nombreJ2);
					
					do{								// Vuelve a tirar J2 si hunde un barco
						turnos.ataqueAlAzarDeJ2();	
						turnos.realizarComprobacionesJ2();
					}while(colocBarcos.getTableroJ1()[turnos.getFila()-1,turnos.getColumna()-1] == (char)'X' && turnos.getRepetirTirada() == true);					
						
					if(turnos.getBarcosJ1() == 0){
						Console.Write("Has perdido la partida. La próxima vez será.");
						Console.ReadKey();
						break;
					}
						
				}while(turnos.getBarcosJ2() != 0);		// Si todos los barcos enemigos están hundidos, se acabó la partida
					Console.Write("Has ganado la partida. Enhorabuena!!");	
				
       	 	}catch(Exception ex) {
            	//Console.WriteLine(ex.Message);
            	if(!sC.Connected){
    	        	Console.SetCursorPosition(12, 21);Console.Write("Error, no se ha podido establecer la conexión con el servidor.");
    	        	Console.SetCursorPosition(12, 22);Console.Write("Asegúrese de que la IP es correcta y el servidor esté activo.");
    	        }
            	Console.ReadKey();
            }
		}
		
		
		
		
		
		
		
	}
}
