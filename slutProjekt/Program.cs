
using System.Diagnostics.Contracts;
using Raylib_cs;
using System.Numerics;
using System;
using System.Collections.Generic;



Raylib.InitWindow(1800, 900, "vinterprojekt");
Raylib.SetTargetFPS(60);


//variabler för spelen
int enemyColorss = 0; //index för färgen på fienden
string scene = "start"; //aktuella scenen
float timer = 0f;//timer
bool tidslut = false; //när tiden tar slut
int liv = 2;//antal liv
int score = 0;//poäng
float enemyVelocityY = 2f;//fiendens hastighet
bool PowerUp = true;//om powerup går att använda
bool validspawnpoint = false;//kontrollera giltig spawnpoint




//objekt i spelet
Rectangle rRect = new Rectangle(275, 260, 250, 100);//start scenen
Rectangle playerRect = new Rectangle(1, 50, 100, 100);
Rectangle enemyRect = new Rectangle(1000, 100, 100, 100);
Rectangle PowerUpRect = new Rectangle(500, 400, 20, 20);


//färg för enemy
Color[] enemyColors = new Color[] { Color.Blue, Color.Purple, Color.Red, Color.Orange };




//listor för nivåer

List<Rectangle> walls = new();
List<Rectangle> walls2 = new();

void newlevel2(List<Rectangle> walls2list)
{

  walls2.Add(new Rectangle(300, 150, 60, 20));
  walls2.Add(new Rectangle(300, 150, 600, 20));
  walls2.Add(new Rectangle(0, 880, 1600, 20));
  walls2.Add(new Rectangle(0, 0, 1800, 20));
  walls2.Add(new Rectangle(0, 880, 1800, 20));
  walls2.Add(new Rectangle(1780, 0, 20, 900));
  walls2.Add(new Rectangle(0, 0, 20, 900));
  walls2.Add(new Rectangle(0, 400, 200, 20));
  walls2.Add(new Rectangle(400, 300, 100, 20));
  walls2.Add(new Rectangle(800, 600, 20, 300));
  walls2.Add(new Rectangle(100, 100, 20, 200));
  walls2.Add(new Rectangle(1200, 400, 200, 20));
  walls2.Add(new Rectangle(1400, 200, 20, 400));
  walls2.Add(new Rectangle(600, 200, 400, 20));
  walls2.Add(new Rectangle(700, 600, 100, 20));
  

}

void newlevel(List<Rectangle> wallslist)
{
  walls.Add(new Rectangle(300, 100, 60, 20));
  walls.Add(new Rectangle(320, 0, 20, 200));
  walls.Add(new Rectangle(300, 0, 32, 128));
  walls.Add(new Rectangle(300, 600, 100, 128));
  walls.Add(new Rectangle(1000, 0, 50, 128));
  walls.Add(new Rectangle(1000, 600, 50, 128));
  walls.Add(new Rectangle(800, 100, 50, 128));
  walls.Add(new Rectangle(1600, 70, 50, 128));
  walls.Add(new Rectangle(1400, 300, 50, 128));
  walls.Add(new Rectangle(0, 0, 1800, 20));
  walls.Add(new Rectangle(0, 880, 1800, 20));
  walls.Add(new Rectangle(1780, 0, 20, 900));
  walls.Add(new Rectangle(0, 0, 20, 900));
  walls.Add(new Rectangle(800, 500, 300, 20));
  walls.Add(new Rectangle(100, 700, 200, 20));
  walls.Add(new Rectangle(1400, 200, 20, 300));
  walls.Add(new Rectangle(500, 100, 20, 200));
  

}

newlevel(walls);
newlevel2(walls2);

//lista för laserskott
List<Rectangle> lasershot = new List<Rectangle>();

//slump generator
Random random = new Random();



//metod för kollisioner mellan spelare/fiende och väggar
static (bool, bool) CheckInWall(Rectangle player, Rectangle enemy, List<Rectangle> wallList)
{
bool playerIsInAWall = false;
bool IsenemyInWall = false;
  

  for (int i = 0; i < wallList.Count; i++)
  {
//är spelaren i väggen
    if (Raylib.CheckCollisionRecs(player, wallList[i]))
    {
      playerIsInAWall = true;
    }
//är enemy i väggen
     if (Raylib.CheckCollisionRecs(enemy, wallList[i]))
    {
      IsenemyInWall = true;
    }
  }
  return (playerIsInAWall, IsenemyInWall);
 }



 

 
//while-loop för att kolla så att enemyn inte respawnar innuti väggen
while (!validspawnpoint)
{
  int spawnX = random.Next(0, 1800);
  int spawnY = random.Next(0, 800);

  enemyRect.X = spawnX;
  enemyRect.Y = spawnY;

  


//kontrollerar kollision med alla väggar
 bool IsenemyInWall = walls.Concat(walls2).Any(wall => Raylib.CheckCollisionRecs(enemyRect, wall));

 // om fienden är inte i vägg är avslutas loopen och enemyn kan spawna
 if (!IsenemyInWall)
{
  validspawnpoint = true;
}
}


/*--------------------------------------------------------------------------------------------------------------*/

while (!Raylib.WindowShouldClose())
{
  //ser till så att powerup bara är aktiv ifall scenen är i game1
  bool Igame1 = scene == "game";
  
 


//start scene
  if (scene == "start")
  {
    walls2.Clear();
    newlevel(walls);
    Raylib.BeginDrawing();


    Raylib.ClearBackground(Color.Black);
    Raylib.DrawRectangleRec(rRect, Color.Black);
    Raylib.DrawText("Press space to start", 290, 300, 20, Color.Red);
    Raylib.DrawText("Press space after you're done with the first lvl to advance to lvl2", 290, 400, 20, Color.Red);
    Raylib.DrawText("LVL2: Look out for gray rectangles for boosts in lvl2 and you will have a laser activated, you will have 30sec to complete lvl2", 290, 500, 20, Color.Red);
    

//starta spelet
    if (Raylib.IsKeyPressed(KeyboardKey.Space))
    {
      Raylib.ClearBackground(Color.Black);


      scene = "game"; 


      playerRect.X = 21;
      playerRect.Y = 50;

      validspawnpoint = false;

    }
    
  }
//spel 1 scene
  else if (scene == "game")
  {

    

    Raylib.ClearBackground(Color.White);

    

    foreach (Rectangle wall in walls)
    {
      Raylib.DrawRectangleRec(wall, Color.DarkBlue);
    }

    //render poäng och hälsa
    {



      Raylib.DrawText($"points {score}", 50, 520, 40, Color.Gray);
      Raylib.DrawText($"Health {liv}", 250, 520, 40, Color.Gray);
      Raylib.DrawRectangleRec(playerRect, Color.Red);
      Raylib.DrawRectangleRec(enemyRect, enemyColors[enemyColorss]);
    }
  }


  //-------------------------------------------------------------------------------

  //logik för fiende
  enemyRect.Y += enemyVelocityY;

  if (enemyRect.Y <= 0 || enemyRect.Y + 100 >= 900)
  {
    enemyVelocityY = -enemyVelocityY;
  }







  //rörelse för player

  if (Raylib.IsKeyDown(KeyboardKey.D))
  {
    playerRect.X += 5;
  }
  if (Raylib.IsKeyDown(KeyboardKey.A))
  {
    playerRect.X -= 5;
  }

  if (Raylib.IsKeyDown(KeyboardKey.W))
  {
    playerRect.Y -= 5;
  }
  if (Raylib.IsKeyDown(KeyboardKey.S))
  {
    playerRect.Y += 5;
  }


//powerup, ökar hälsa om kollision
  if (!Igame1 && PowerUp && Raylib.CheckCollisionRecs(playerRect, PowerUpRect)){
    liv++;
    PowerUp = false;
  }


  //-------------------------------------------------------------------------


 //kollision mellan väggar och tar bort liv ifall det blir kollision
  (bool playerIsInAWall, bool enemyIsInWall) = CheckInWall(playerRect, enemyRect, walls.Concat(walls2).ToList());

  if (playerIsInAWall)
  {

    liv--;

    playerRect.X = 21;
    playerRect.Y = 50;


    if (liv < 0)
    {

      walls.Clear();

      scene = "start";
      liv = 2;

    }

  }

if(enemyIsInWall) {
  enemyVelocityY = -enemyVelocityY;
}




  //---------------------------------------------------------------------

  //kollar kollsion med fiende och tar hand om poängen och vart exakt fienden är, också säkerställer färgändringen
  if (Raylib.CheckCollisionRecs(playerRect, enemyRect))
  {

    enemyColorss = (enemyColorss + 1) % enemyColors.Length;
    score++;
    bool iveggen = true;


  
//spawnar nya fienden och ser till så att den inte spawnar i väggen
    while (iveggen)
    {
      enemyRect.X = random.Next(0, 1800);
      enemyRect.Y = random.Next(0, 800);
      iveggen = walls.Any(wall => Raylib.CheckCollisionRecs(enemyRect, wall));
      iveggen = walls2.Any(wall2 => Raylib.CheckCollisionRecs(enemyRect, wall2));

    }
  }






  if (scene == "victory")
  {

    Raylib.ClearBackground(Color.Black);
    Raylib.DrawText("if you've currently completed lvl1 press SPACE", 400, 350, 30, Color.Violet);
    Raylib.DrawText("Otherwise CONGRATZ you have completed the game", 400, 750, 50, Color.Violet);
    score = 0;
    if (Raylib.IsKeyPressed(KeyboardKey.Space))
    {
      scene = "game2";

    }

  }


  if (scene == "game2")
  {
    walls2.Clear(); 
    newlevel2(walls2); 

    Raylib.ClearBackground(Color.White);
    


//rita ut väggarna i game2
    foreach (Rectangle wall2 in walls2)
    {
      Raylib.DrawRectangleRec(wall2, Color.Brown);
    }

    Raylib.DrawText($"points {score}", 50, 520, 40, Color.Gray);
    Raylib.DrawText("Laser is activated, press 'X' to use", 50, 720, 40, Color.Gray);
    Raylib.DrawText($"Health {liv}", 250, 520, 40, Color.Gray);
    Raylib.DrawText($"Time {timer}", 450, 520, 40, Color.Green);
    Raylib.DrawRectangleRec(playerRect, Color.Red);
    Raylib.DrawRectangleRec(enemyRect, enemyColors[enemyColorss]);

  // om powerup är true 
  if (PowerUp){
    Raylib.DrawRectangleRec(PowerUpRect, Color.Gray);
  }
//för varje frame går en sek i timern
  timer += Raylib.GetFrameTime();
//om timern når 30sek blir tidslut true och scenen går till start
  if (timer >= 30f && !tidslut){
  tidslut = true; 
  scene = "start";
  }

  if (Raylib.IsKeyPressed(KeyboardKey.X))
  {
    //lasern dimensioner
    lasershot.Add(new Rectangle(playerRect.X + playerRect.Width / 2 - 2, playerRect.Y, 4, 10));
  }

  for (int i = 0; i < lasershot.Count; i++){
    Rectangle laser = lasershot[i];
    laser.Y -= 10;//uppdaterar lasern position
    lasershot[i] = laser;

//om lasern går ut ur skärmen tars den bort
    if (laser.Y < 0)
    {
      lasershot.RemoveAt(i);
    }
//annars kollas kollision med lasern och enemyn (ifall den kolliderar spawna ny) och tar bort lasern efter en kollision skett
    else if (Raylib.CheckCollisionRecs(laser, enemyRect))
    {
      score++;
      enemyRect.X = random.Next(0, 1800);
      enemyRect.Y = random.Next(0, 800);
      lasershot.RemoveAt(i);//tar bort laser
      i--;
    }
    else{
      Raylib.DrawRectangleRec(laser, Color.Violet);
    }

  }
  }

  if (score == 2)
  {
    scene = "victory";
    walls.Clear();
  }


  Raylib.EndDrawing();
}






