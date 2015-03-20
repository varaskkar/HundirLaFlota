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

namespace Hundir_la_Flota{
	public class Servidor{
		
		// Servidor
        Socket sS = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    	IPEndPoint ipS = new IPEndPoint (IPAddress.Any, 1238);
    	// Cliente
   		Socket sC = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); 
        IPEndPoint ipC = new IPEndPoint (IPAddress.Parse ("127.0.0.1"), 6000); 
        // Serialización

        
		ColocacionDeBarcos colocBarcos = new ColocacionDeBarcos();
		Turnos turnos = new Turnos();
		System.Media.SoundPlayer player = new System.Media.SoundPlayer();
		
		bool validar = false, volverAtras = true, sonido = false;
		int opcion1 = 0, opcion2 = 0, opcion3 = 0, barcosIniciales = 6;	
		string nombreJ1 = null, nombreJ2 = null;
		
		int[,] tableroJ1 = new int[6,8], tableroJ2 = new int[6,8], posJ2 = new int[6,8];
		
		
		
		public Servidor(){  }
		
		public void setTablero1(int[,] tableroJ1){
			this.tableroJ1 = tableroJ1;
		}
		
		public int[,] getTableroJ2(){
			return tableroJ2;
		}
		
		public void servidor(){
		    
			Console.SetCursorPosition(24, 18);Console.WriteLine("Buscando un cliente disponible...");  
			
			try {
			   	sS.Bind(ipS);	
	    	   	sS.Listen(25);
			   	Socket accepted = sS.Accept();	// 1º Espera
			   	sC.Connect(ipC);	   			// 2º Confirma  	
			   	
			   	Console.SetCursorPosition(24, 18);Console.WriteLine("                                 ");  
			   	Console.SetCursorPosition(24, 18);Console.WriteLine("Conexión establecida con éxito!");  

			   	NetworkStream net = new NetworkStream(sC);


			   	
	           	Thread.Sleep(1000);
	           	Console.SetCursorPosition(24, 20);Console.Write("Esperando a que el cliente coloque sus barcos...");	
	           	
	           	string strData = null;
	           	while(strData == null){		 
	           		byte[] buffer = new Byte[255];
               		int rec = accepted.Receive(buffer,0,buffer.Length,0);
               		Array.Resize(ref buffer, rec); 
               		strData = Encoding.ASCII.GetString(buffer);
	    		}       
	   			Console.SetCursorPosition(24, 22);Console.Write("Ahora coloca tus barcos.");
	           	Thread.Sleep(1000);

				do{ // Validación del menú secundario
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
				
				string text = "fin";	// Barcos colocados, Comienza la partida!
				byte[] data = Encoding.ASCII.GetBytes(text);
				sC.Send(data); 
				
				byte[] buffer2 = new Byte[255];						// Guardo su nombre
           		int rec2 = accepted.Receive(buffer2,0,buffer2.Length,0);
           		Array.Resize(ref buffer2, rec2); 
           		nombreJ2 = Encoding.ASCII.GetString(buffer2);
				turnos.setNombreJ2(nombreJ2);
				
				byte[] data2 = Encoding.ASCII.GetBytes(nombreJ1);	// Envío el nombre
				sC.Send(data2);        		
              		
           		BinaryFormatter bf = new BinaryFormatter();			// Guardo su tablero
           		tableroJ2 = (int[,])bf.Deserialize(net);           		           		
				
//				if(int i=0; i<tableroJ1.Length;i++){
//					if(int j=0; tableroJ2<tableroJ2.Length;j++){
//						if(tableroJ1 == 1 || tableroJ1 == (char)'B'){
//							byte[] data3 = Encoding.ASCII.GetBytes(tableroJ1[i,j]);	// Envío el nombre
//							sC.Send(data3); 
//						}
//					}
//				}
				
				do{									// Cambia el turno de J1 a J2 y viceversa	
					do{								// Vuelve a tirar J1 si hunde un barco
						turnos.introducirFila();
						turnos.introducirColumna();
						turnos.realizarComprobacionesJ1();
					}while(colocBarcos.getTableroJ2()[turnos.getFila()-1,turnos.getColumna()-1] == (char)'X' && turnos.getRepetirTirada() == true);

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
	           Console.WriteLine ("\nConexión perdida...");
	           Console.WriteLine(ex.Message);
	           Console.ReadKey();
	        }
		}
		
		
		
		
		
		
		
	}
}
