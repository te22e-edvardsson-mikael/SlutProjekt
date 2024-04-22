
using System.Diagnostics.Contracts;
using Raylib_cs;
using System.Numerics;
using System;



Raylib.InitWindow(1800, 900, "vinterprojekt");
Raylib.SetTargetFPS(60);


//variabler för spelen
int enemyColorss = 0;
string scene = "start";
float timer = 0f;
bool tidslut = false; 
int liv = 2;
int score = 0;
float enemyVelocityY = 1f;
bool PowerUp = true;



//objekt i spelet
Rectangle rRect = new Rectangle(275, 260, 250, 100);
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
}

void newlevel(List<Rectangle> wallslist)
{
  walls.Add(new Rectangle(300, 100, 60, 20));
  walls.Add(new Rectangle(320, 0, 16, 200));
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
bool enemyIsInWall = false;
  

  for (int i = 0; i < wallList.Count; i++)
  {

    if (Raylib.CheckCollisionRecs(player, wallList[i]))
    {
      playerIsInAWall = true;
    }

     if (Raylib.CheckCollisionRecs(enemy, wallList[i]))
    {
      enemyIsInWall = true;
    }
  }
  return (playerIsInAWall, enemyIsInWall);
 }

 



/*--------------------------------------------------------------------------------------------------------------*/

while (!Raylib.WindowShouldClose())
{
  bool Igame1 = scene == "game";


//start scene
  if (scene == "start")
  {
    Raylib.BeginDrawing();

    Raylib.ClearBackground(Color.Black);
    Raylib.DrawRectangleRec(rRect, Color.Black);
    Raylib.DrawText("Press space to start", 290, 300, 20, Color.Red);

    if (Raylib.IsKeyPressed(KeyboardKey.Space))
    {
      Raylib.ClearBackground(Color.Black);


      scene = "game";


      playerRect.X = 21;
      playerRect.Y = 50;

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
    Raylib.DrawText("victory", 900, 450, 10, Color.Violet);
    score = 0;
    if (Raylib.IsKeyPressed(KeyboardKey.Space))
    {
      scene = "game2";

    }

  }


  if (scene == "game2")
  {


    Raylib.ClearBackground(Color.White);

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

  
  if (PowerUp){
    Raylib.DrawRectangleRec(PowerUpRect, Color.Gray);
  }

  timer += Raylib.GetFrameTime();

  if (timer >= 15f && !tidslut){
  tidslut = true; 
  scene = "start";
  }

  if (Raylib.IsKeyPressed(KeyboardKey.X))
  {
    lasershot.Add(new Rectangle(playerRect.X + playerRect.Width / 2 - 2, playerRect.Y, 4, 10));
  }

  for (int i = 0; i < lasershot.Count; i++){
    Rectangle laser = lasershot[i];
    laser.Y -= 10;
    lasershot[i] = laser;


    if (laser.Y < 0)
    {
      lasershot.RemoveAt(i);
    }

    else if (Raylib.CheckCollisionRecs(laser, enemyRect))
    {
      score++;
      enemyRect.X = random.Next(0, 1800);
      enemyRect.Y = random.Next(0, 800);
      lasershot.RemoveAt(i);
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






