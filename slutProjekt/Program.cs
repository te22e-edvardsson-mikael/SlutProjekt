
using System.Diagnostics.Contracts;
using Raylib_cs;
using System.Numerics;



Raylib.InitWindow(1800, 900, "vinterprojekt");
Raylib.SetTargetFPS(60);


//variabler//
int enemyColorss = 0;


string scene = "start";






int liv = 2;

int score = 0;

float enemyVelocityY = 1f;







//listor//

List<Rectangle> walls = new();
List<Rectangle> walls2 = new();

void newlevel2(List<Rectangle> walls2list)
{

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








Rectangle rRect = new Rectangle(275, 260, 250, 100);
Rectangle playerRect = new Rectangle(1, 50, 100, 100);
Rectangle enemyRect = new Rectangle(1000, 100, 100, 100);



Color[] enemyColors = new Color[] { Color.Blue, Color.Purple, Color.Red, Color.Orange };


/*--------------------------------------------------------------------------------------------------------------*/

while (!Raylib.WindowShouldClose())
{



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

  else if (scene == "game")
  {
    Raylib.ClearBackground(Color.White);
    foreach (Rectangle wall in walls)
    {
      Raylib.DrawRectangleRec(wall, Color.DarkBlue);
    }

    //render
    {



      Raylib.DrawText($"points {score}", 50, 520, 40, Color.Gray);
      Raylib.DrawText($"Health {liv}", 250, 520, 40, Color.Gray);
      Raylib.DrawRectangleRec(playerRect, Color.Red);
      Raylib.DrawRectangleRec(enemyRect, enemyColors[enemyColorss]);




      //-------------------------------------------------------------------------------

      //logic
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


      //-------------------------------------------------------------------------

      //räkna ints

      bool isInAWall = false;

      for (int i = 0; i < walls.Count; i++)
      {

        if (Raylib.CheckCollisionRecs(playerRect, walls[i]))
        {
          isInAWall = true;
        }
        if (Raylib.CheckCollisionRecs(enemyRect, walls[i]))
        {
          enemyVelocityY = -enemyVelocityY;
        }
      }

      if (isInAWall == true)
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





      //---------------------------------------------------------------------

      //funktioner för enemyrect



      if (Raylib.CheckCollisionRecs(playerRect, enemyRect))
      {

        enemyColorss = (enemyColorss + 1) % enemyColors.Length;

        score++;

        Random random = new Random();

        bool iveggen = true;

        while (iveggen)
        {
          enemyRect.X = random.Next(0, 1800);
          enemyRect.Y = random.Next(0, 800);

          iveggen = walls.Any(wall => Raylib.CheckCollisionRecs(enemyRect, wall));
        }
      }
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



  }

  if (score == 5)
  {
    scene = "victory";
    walls.Clear();
  }
  Raylib.EndDrawing();
}
