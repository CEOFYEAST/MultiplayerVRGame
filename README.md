* for oculus users
 	- you may need to disable integrated graphics in order to use oculus link properly 
	- you need the quest pc app to play 

Steps to Run 
	- install unity 2021.3.7f1
		- include Android Build Support module with OpenJDK and Android SDK & NDK Tools
	- open project using unity install above 
	- open correct scene in project via Assets > Show in Explorer > single player club fair project unity

Start Gameloop During Play 
	- open XR Origin > Camera Offset > Main Camera in inspector 
	- expand Effect During Runtime component 
	- set Start Game Loop variable to 1
	- set Start Game Loop back to 0 once the game loop has begun
		- the game loop may not start immediately (a script checks the variable every once in a while to see if its been changed)
		- you really don't need to set it back to 0 immediately. You just need to do it before the game loop is over
	- you can change the length of the time the player has to shoot 
		- expand the Game Loop component under Main Camera 
		- change the Round Length variable to the desired time 

