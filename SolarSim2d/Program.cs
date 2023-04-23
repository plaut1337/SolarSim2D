using Raylib_cs;
using System.Numerics;

namespace SolarSim2d
{
    static class Program
    {
        public static void Main()
        {
            Raylib.InitWindow(1000, 750, "Solar System Simulator 2D");

            Raylib.SetTargetFPS(60);

            //Creating scenes states and a state manager
            Menu menu = new Menu(Color.BLACK);
            Menu help = new Menu(Color.BLACK);
            Game game = new Game(Color.BLACK);

            StateManager manager = new StateManager();

            manager.AddState(() => menu.DrawMenu(), 0);
            manager.AddState(() => help.DrawMenu(), 1);
            manager.AddState(() => game.DrawGame(), 2);

            //Creating a main menu
            menu.AddTitle("Solar System Simulator 2D [version 6.9.4.20]", 1, Color.GREEN);
            menu.AddTitle("(c) Roko Marsic. All rights reserved.", 3, Color.GREEN);

            menu.AddItem("Start", Color.GREEN);
            menu.AddItem("Help", Color.GREEN);
            menu.AddItem("Exit", Color.GREEN);

            //Creating a help menu
            help.AddTitle("Help", 1, Color.GREEN);
            help.AddTitle(" ", 3, Color.GREEN);
            help.AddTitle("Left mouse click to add a planet.", 3, Color.GREEN);
            help.AddTitle("Right mouse click to remove the last planet.", 3, Color.GREEN);
            help.AddTitle("Upper-left corner to adjust properties of the next planet.", 3, Color.GREEN);
            help.AddTitle(" ", 3, Color.GREEN);





            //Game loop
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                
                manager.StartManager();

                if (manager.CurrentState == 1) Raylib.DrawText("Back", Raylib.GetScreenWidth() - 5 - Raylib.MeasureText("Back", 24), 5, 24, Color.GREEN);

                Raylib.EndDrawing();

                if (Menu.CheckEnter() && menu.GetPointerPosition() == 0) manager.ChangeState(2);
                else if (Menu.CheckEnter() && menu.GetPointerPosition() == 1) manager.ChangeState(1);
                else if (Menu.CheckEnter() && menu.GetPointerPosition() == 2) return;
                else if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT) && Raylib.GetMouseX() > Raylib.GetScreenWidth() - Raylib.MeasureText("Back", 24) - 5
                && Raylib.GetMouseY() < 30) manager.ChangeState(0);
            }

            Raylib.CloseWindow();
        }
    }
}