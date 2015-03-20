using System;
using System.IO;
using System.Threading;	

	// TODO generar un archivo de texto con los resultados de la partida...rondas
	// TODO problema al ganar la partida											
	// TODO poner "guardar para siempre" configuracion de colores que elija el usuario		No Puedo :/	
	// TODO crear un 'acerca de' con algo de información
	// TODO que el usuario elija el tamaño del mapa
	// TODO colocar un método para validar el nombre de usuario

namespace Hundir_la_Flota{
	public class Program{    
	
		public static void Main(string[] args){
		
		ColocacionDeBarcos colocBarcos = new ColocacionDeBarcos();
		Turnos turnos = new Turnos();
		Cliente cliente = new Cliente();
		Servidor servidor = new Servidor();
		System.Media.SoundPlayer player = new System.Media.SoundPlayer();
		
		Console.Title = "Hundir la flota 0.4";
		bool validar = false, volverAtras = true, sonido = false;
		int opcion1 = 0, opcion2 = 0, opcion3 = 0, barcosIniciales = 6;	
		string nombreJ1 = null, nombreJ2 = null;	
		
	do{ // Validación del menu principal
		if(sonido == true){
			player.SoundLocation = ".\\sonido\\FondoDeUnaCueva.wav";
			player.Play();
		}
		colocBarcos.menu();
		Console.SetCursorPosition(24, 11);Console.Write("#############################");
		Console.SetCursorPosition(24, 12);Console.Write("#			    #");
		Console.SetCursorPosition(24, 13);Console.Write("# Selecciona una categoría: #");
		Console.SetCursorPosition(24, 14);Console.Write("#			    #");
		Console.SetCursorPosition(24, 15);Console.Write("# (1)  Comenzar            #");
		Console.SetCursorPosition(24, 16);Console.Write("# (2)  Opciones            #");
		Console.SetCursorPosition(24, 17);Console.Write("# (3)  Salir               #");
		Console.SetCursorPosition(24, 18);Console.Write("#		Versión 0.4 #");
		Console.SetCursorPosition(24, 19);Console.Write("#			    #");
		Console.SetCursorPosition(24, 20);Console.Write("#############################\n\n");		
		for(int i=0; i<=79; i++){
			Console.Write("_");
		}
		for(int i=0; i<=79; i++){
		   		Console.Write("=");
		   		Thread.Sleep(8);
		} 
		Console.Write(">>> ");
		validar = int.TryParse(Console.ReadLine(), out opcion1);
		
		switch(opcion1){
			case 1:
				do{					
					colocBarcos.menu();
					Console.SetCursorPosition(24, 11);Console.Write("Elije el modo de juego\n");
					Console.SetCursorPosition(24, 12);Console.Write("----------------------\n");
					Console.SetCursorPosition(24, 13);Console.Write("(1)  Contra la máquina");
					Console.SetCursorPosition(24, 14);Console.Write("(2)  Multijugador");
					Console.SetCursorPosition(24, 15);Console.Write("(3)  Online");
					Console.SetCursorPosition(24, 16);Console.Write("(4)  Volver al menú");
					Console.SetCursorPosition(24, 18);Console.Write(">>> ");
					validar = int.TryParse(Console.ReadLine(), out opcion2);			
					switch(opcion2){
						case 1:
							do{ 
								if(sonido == true){
									player.SoundLocation = ".\\sonido\\MenuOpciones.wav";
									player.Play();
								}
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
								validar = int.TryParse(Console.ReadLine(), out opcion3); 		
							}while(opcion3!=1 && opcion3!=2);
							switch(opcion3){
								case 1:
									colocBarcos.colocacionBarcosAutomatJ1();
						    		break;
						  	  	case 2:	
						    		colocBarcos.colocacionBarcosManualJ1();
						       	 	break;	
							} 
							colocBarcos.colocacionBarcosAutomatJ2();		
			
							turnos.setTablero1(colocBarcos.getTableroJ1());	
							turnos.setTablero2(colocBarcos.getTableroJ2()); 
							turnos.setPosJ2(colocBarcos.getPosJ2());
							turnos.setBarcosJ1(colocBarcos.getBarcosJ1());
							turnos.setBarcosJ2(colocBarcos.getBarcosJ2());
							
							cliente.setTablero1(colocBarcos.getTableroJ1()); 	// Pasamos el tablero C al S para que en esa
							servidor.setTablero1(colocBarcos.getTableroJ1());	// clase se lo pase a la otra por Sock. Luego
							
							//turnos.setTablero2(cliente.getTableroJ2()); 		// Lo ponemos en clase turnos como tableroJ2
							//turnos.setTablero2(servidor.getTableroJ2());
			
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
									
							}while(turnos.getBarcosJ2() != 0 || turnos.getBarcosJ1() != 0);		// Si todos los barcos enemigos están hundidos, se acabó la partida
								if(turnos.getBarcosJ1() == 0){
									Console.Write("Has perdido. La próxima vez será.");
								}else if(turnos.getBarcosJ2() == 0){
									Console.Write("Has ganado la partida. Enhorabuena!!");
								}
								Thread.Sleep(2500);
						break;
					case 2:		// Multijugador
						
						break;
					case 3:		// Online
						colocBarcos.menu();
						Console.SetCursorPosition(24, 11);Console.Write("¿Quién vas a ser?");
						Console.SetCursorPosition(24, 12);Console.Write("-----------------");
						Console.SetCursorPosition(24, 13);Console.Write("(1)  Cliente");
						Console.SetCursorPosition(24, 14);Console.Write("(2)  Servidor");
						Console.SetCursorPosition(24, 16);Console.Write(">>> ");
						validar = int.TryParse(Console.ReadLine(), out opcion3);
						switch(opcion3){
							case 1:
								cliente.cliente();
								break;
							case 2:
								servidor.servidor();
								break;
						}
						break;
					case 4:
						volverAtras = false;
						break;
					}
				}while(volverAtras == true);
				break;	// Fin case 1 - Comenzar a jugar
			case 2:
			  do{
				if(sonido == true){
					player.SoundLocation = ".\\sonido\\FondoDeUnaCueva.wav";
					player.Play();
				}
				colocBarcos.menu();
				Console.SetCursorPosition(24, 11);Console.Write("(1)  Elegir cantidad de barcos\n");
				Console.SetCursorPosition(24, 12);Console.Write("(2)  Poner nombre al jugador 2\n");
				Console.SetCursorPosition(24, 13);Console.Write("(3)  Activar efectos de sonido\n");
				Console.SetCursorPosition(24, 14);Console.Write("(4)  Volver al menú\n");
				Console.SetCursorPosition(24, 16);Console.Write(">>> ");
				validar = int.TryParse(Console.ReadLine(), out opcion2);
				switch(opcion2){
					case 1:
						do{
							colocBarcos.menu();
							Console.SetCursorPosition(24, 11);Console.Write("¿Con cuántos barcos quieres jugar? (Máx 30)");
							Console.SetCursorPosition(24, 13);Console.Write(">>> ");
							validar = int.TryParse(Console.ReadLine(), out barcosIniciales);
						}while(barcosIniciales<1 || barcosIniciales>30);
						colocBarcos.setBarcosIniciales(barcosIniciales);
						Console.SetCursorPosition(24, 15);Console.Write("Cambios guardados correctamente!");
						Thread.Sleep(1250);
						break;
					case 2:
						colocBarcos.menu();
						Console.SetCursorPosition(24, 11);Console.Write("Introduce el nombre de tu oponente ");
						Console.SetCursorPosition(24, 13);Console.Write(">>> ");
						nombreJ2 = Console.ReadLine();
							while(nombreJ2.Length>7){
								colocBarcos.menu();
								Console.SetCursorPosition(24, 11);Console.Write("Pon un nombre más corto");
								Console.SetCursorPosition(24, 13);Console.Write(">>> ");
								nombreJ2 = Console.ReadLine();
							}
						turnos.setNombreJ2(nombreJ2);
						Console.SetCursorPosition(24, 15);Console.Write("Nombre guardado correctamente!");
						Thread.Sleep(1250);
						break;
					case 3:
						colocBarcos.menu();
						Console.SetCursorPosition(24, 11);Console.Write("¿Qué desea hacer?");
						Console.SetCursorPosition(24, 12);Console.Write("-----------------");
						Console.SetCursorPosition(24, 13);Console.Write("(1)  Activar efectos de sonido ");
						Console.SetCursorPosition(24, 14);Console.Write("(2)  Desactivar efectos de sonido (defecto)");
						Console.SetCursorPosition(24, 16);Console.Write(">>> ");
						validar = int.TryParse(Console.ReadLine(), out opcion3);
							if(opcion3 == 1){
								DirectoryInfo DIR = new DirectoryInfo(".\\sonido"); 
								if (!DIR.Exists){ 
									Console.SetCursorPosition(24, 18);Console.Write("Error. No se encuentra la carpeta 'sonido'.");
									Thread.Sleep(1750);
								}else{
									sonido = true;
									Console.SetCursorPosition(24, 18);Console.Write("Sonido activado correctamente!");
									Thread.Sleep(1250);
								}
							}else if(opcion3 == 2){
								sonido = false;
								Console.SetCursorPosition(24, 18);Console.Write("Sonido desactivado correctamente!");
								Thread.Sleep(1250);
							}
						turnos.setSonido(sonido); 
						colocBarcos.setSonido(sonido);
						break;
					case 4:
						volverAtras = false;
						break;
				}
			  }while(volverAtras == true);
				break;
			case 3:
				break;
			case 4:
				Console.Write("1- Cliente\n2- Servidor");
				Console.Write("\n>>> ");
				validar = int.TryParse(Console.ReadLine(), out opcion3);
				switch(opcion3){
					case 1:
						cliente.cliente();
						break;
					case 2:
						servidor.servidor();
						break;
				}
				break;
		}
	}while(opcion1!=3);
				

			
			

			


		
		
		
		
		
		
		
		
		
		
		
		
		
		}
	}
}