# Hundir la Flota
El juego de Hundir la Flota en C#

![avatar](https://i.imgur.com/1fqHKhA.jpg)
![avatar](http://i.imgur.com/a69wP1C.jpg)

Update v0.3 - 02/02/13
----------------------------------
- Ordenado todo el código por POO
- Arreglado problema al dar J1 a J2 en pos de barco hundido(X) deberia perder turno
- Solucionado problema al cargar los barcos de forma manual de J1    
- Agregada notificacion de "cargando barcos..." para avisar del tiempo   
- Pequeños cambios en el diseño
- Permite agregar nombre al jugador 2 (máquina)
- Al colocar los barcos manualmente, se muestran en el mapa los barcos que vayas introduciendo
- Solucionado lo de colocar barco en pos ya coloc antes (colocación manual)
- Agregada la opción de activar efectos de sonido

Update v0.2 - 12/12/12
----------------------------------
- Mejorado el diseño de los mapas
- Asignación de identidades con carácteres. B = Barco. A = Agua. X = Hundido.- = Desconocido
- Marcado en el mapa donde se haya atacado en agua para no volver a dar en la misma posición
- Se ocultan los barcos del J2, sin embargo permanecen en la misma posición
- Optimizado el sistema de elección aleatoria
- Si atacas a una posición ya atacada anteriormente pierdes turno
- Creado un menú principal
- Se muestra el turno del usuario con su nombre introducido al iniciar
- Ahora puedes indicar en opciones con cuantos barcos quieres jugar en el mapa. Igual nº para J1 y J2.

Update v0.1 - 2/12/12
-------------------------------
 - Añadidas estadísticas de juego
 - Validación de la colocación y del ataque (mejorado)
 - Cuando destruyes un barco, puedes volver a atacar (como en el parchís)
 - Alternación de rondas entre J1 y J2 cada vez que das en agua...hasta destruir los 6 barcos
 - Evitada la colocación de más de un barco en la misma casilla (se eliminaba el barco anterior)
 - Ahora puedes elegir entre colocar los barcos automáticamente o manualmente.




