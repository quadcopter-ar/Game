# BilliardsGame
Billiards game in Unity 
This is the documentation for a multiplayer 3D Pool/Billiards game. <br />
The game currently has provisions for two players that can be moved by W, A, S, D, and arrow buttons, respectively. The  player objects represent the quadcopter. The user operating as the player moves towards a cue ball and provides it with a forward thrust. This thrust in turn makes the spherical cue ball move forward at a particular angle in the 3D space until it loses momentum and comes to rest. The ball is capable of bouncing off flat surfaces.
The arena is a cubical structure with six faces(solid walls) and eight vertices(corners). Each vertex is designated as a pocket. Just like in a conventional billiards/pool game, a player scores if he/she is successfully able to deliver the cue ball to any of the eight pockets. Thus if a cue ball(after being struck by the forward thrust of a player) hits a pocket, the player who was able to send the ball to the pocket is rewarded with points and the ball is brought to rest at the mean position(the center of the cube).
The Unity version we are currently using is 2018.2.17f1. <br />
**Game Scene and objects**<br />
The entire game takes place within a cubic arena and the gaming objects are placed within this arena: Gravity is disabled in order to permit realistic motion of the ball in 3D space. The ball follows Newtonian principles of motion and comes to rest(after decelerating) in the absence of any external force.
Box-shaped cubic arena:-
<br />
***P1 and P2***<br />
These objects indicate the spawn position of both players. They are at each side of the cubic arena facing each other as opponents. They are controlled by the players and their collision with the ball drives the game.<br />
***Walls or surfaces***<br />
There are 6 walls to form a bounding box for gaming area. Box colliders are enabled and the ball can bounce on any of the walls during its motion in 3D space. With every collision, the ball keeps losing momentum and eventually comes to rest (in a realistic manner)<br />
***Spherical Cue ball / Bouncer***<br />
-present at the center of the arena by default. The players(P1 and P2) through their movements strike this ball and make it move along a certain trajectory.<br />
***Pockets***<br />
These are the vertices of the cube that play a special role in the game. The pockets act as goals or destinations for the ball. If a player’s thrust is successfully able to deliver the ball to any of the 8 pockets , the player whose thrust caused this is awarded with points and his score is incremented.<br />
**Gameplay:**<br />
The ball is allowed to bounce around any number of times during its motion in 3D space. Acceleration is provided to the ball entirely through the force of a particular player’s thrust. The ball is automatically brought back to its central mean position if it has been successfully delivered to a pocket(and the scoring user has been awarded with points).
In order to ensure that the player objects themselves do not bounce back on being hit by the ball, they are converted to stationary kinematic objects until that turn ends.  <br />
## Scripts
The two scripts governing the Player movement and the movement of the ball object are as follows:<br />
***NewBallScript.cs***<br />
This script governs the behavior of the ball throughout the game. The ball starts moving at every turn only on being struck by a player and obeys the Newtonian laws of motion in its behavior. Every collision causes it to lose some momentum. This continues until the ball comes to rest on its own or if it gets successfully delivered to a pocket. In the second case it is brought back to the mean position for the next turn(so that the second player can take an aim and strike).  
Collision detection is an important part of this script since it is the first collision with the player object that sets the ball into motion. The next turn cannot begin as long as this motion persists.<br />
***PlayerMoveScript.cs***<br />
This script governs the effect of the player’s collision with the ball. The game begins as a player object (P1 or P2) hits the ball with a forward thrust . In order to prevent the player from bouncing back in the opposite direction, the player is made into a kinematic entity for the rest of the turn. Even if the ball (during its course of motion with multiple collisions) bumps into the player during a particular turn it does not cause the player to get displaced.  The players alternatively get a chance to strike the ball . Each time, a player has successfully delivered the ball to a pocket, an alert message is displayed on the console and the score corresponding to the player is incremented.

