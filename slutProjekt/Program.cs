using Raylib_cs;
using System.Numerics;

Raylib.InitWindow(800, 600, "joebiden");
Raylib.SetTargetFPS(60);

List<Rectangle> walls = new();

walls.Add(new Rectangle(0,800,20,20));
walls.Add(new Rectangle(600,800,20,20));
walls.Add(new Rectangle(600,0,20,20));
walls.Add(new Rectangle(600,400,20,20));



while (!Raylib.WindowShouldClose()){
    Raylib.BeginDrawing();

    
 





    Raylib.EndDrawing();
}