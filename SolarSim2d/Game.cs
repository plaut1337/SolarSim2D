using Raylib_cs;
using System.Numerics;

namespace SolarSim2d
{
    public class Game
    {
        Raylib_cs.Color backgroundColor;
        Planet star = new Planet(new Vector2(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2), new Vector2(0, 0), new Vector2(0, 0));
        Planet planet = new Planet(new Vector2(300, 300), Vector2.Zero, Vector2.Zero);
        List<Planet> planetList = new List<Planet>();
        List<Raylib_cs.Color> colorList = new List<Raylib_cs.Color>() { Color.ORANGE, Color.RED, Color.GREEN, Color.BLUE, Color.YELLOW, Color.PURPLE, Color.PINK };
        int currentColorIndex = 0, currentMass = 2;
        Raylib_cs.Color currentColor = Color.ORANGE;

        public Game(Raylib_cs.Color color)
        {
            backgroundColor = color;
        }

        public int CurrentMass
        {
            get { return currentMass; }
        }

        public Color CurrentColor
        {
            get { return currentColor; }
        }

        public void AddPlanet()
        {
            Planet planet = new Planet();

            planet.position = Raylib.GetMousePosition();
            planet.mass = currentMass;
            planet.color = currentColor;

            planetList.Add(planet);
        }

        public void DrawGame()
        {
            Raylib.ClearBackground(backgroundColor);

            Raylib.DrawText("Mass: " + currentMass.ToString(), 5, 5, 24, Color.GREEN);
            Raylib.DrawText("Color:", 5, 35, 24, Color.GREEN);
            Raylib.DrawCircle(5 + Raylib.MeasureText("Color:  ", 24), 46, 10, currentColor);

            Raylib.DrawText("- +", Raylib.MeasureText("Mass: 10", 24) + 30, 5, 24, Color.GREEN);
            Raylib.DrawText("<  >", Raylib.MeasureText("Mass: 10", 24) + 31, 35, 24, Color.GREEN);

            Raylib.DrawText("Back", Raylib.GetScreenWidth() - 5 - Raylib.MeasureText("Back", 24), 5, 24, Color.GREEN);

            Raylib.DrawCircle((int)star.position.X, (int)star.position.Y, 15, Color.YELLOW);

            for (int i = 0; i < planetList.Count; i++)
            {
                Raylib.DrawCircle((int)planetList[i].position.X, (int)planetList[i].position.Y, planetList[i].mass * 3f, planetList[i].color);

                planetList[i].AttractTo(star);

                planetList[i].UpdatePlanet();
            }

            if (Raylib.CheckCollisionCircles(Raylib.GetMousePosition(), 1, new Vector2(Raylib.MeasureText("Mass: 10", 24) + 35, 16), 9) 
            && currentMass > 1 && Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)) currentMass--;
            else if (Raylib.CheckCollisionCircles(Raylib.GetMousePosition(), 1, new Vector2(Raylib.MeasureText("Mass: 10", 24) + 57, 16), 9) 
            && currentMass < 5 && Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)) currentMass++;
            else if (Raylib.CheckCollisionCircles(Raylib.GetMousePosition(), 1, new Vector2(Raylib.MeasureText("Mass: 10", 24) + 36, 46), 9) 
            && Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                if (currentColorIndex != 0) currentColorIndex--;
                else currentColorIndex = 6;

                currentColor = colorList[currentColorIndex];
            }
            else if (Raylib.CheckCollisionCircles(Raylib.GetMousePosition(), 1, new Vector2(Raylib.MeasureText("Mass: 10", 24) + 62, 46), 9)
                && Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
            {
                if (currentColorIndex != 6) currentColorIndex++;
                else currentColorIndex = 0;

                currentColor = colorList[currentColorIndex];
            }

            if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT)
            && !Raylib.CheckCollisionCircles(Raylib.GetMousePosition(), 1, new Vector2(Raylib.MeasureText("Mass: 10", 24) + 62, 46), 9)
            && !Raylib.CheckCollisionCircles(Raylib.GetMousePosition(), 1, new Vector2(Raylib.MeasureText("Mass: 10", 24) + 36, 46), 9)
            && !Raylib.CheckCollisionCircles(Raylib.GetMousePosition(), 1, new Vector2(Raylib.MeasureText("Mass: 10", 24) + 57, 16), 9)
            && !Raylib.CheckCollisionCircles(Raylib.GetMousePosition(), 1, new Vector2(Raylib.MeasureText("Mass: 10", 24) + 35, 16), 9)) AddPlanet();
            else if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT)
            && !Raylib.CheckCollisionCircles(Raylib.GetMousePosition(), 1, new Vector2(Raylib.MeasureText("Mass: 10", 24) + 62, 46), 9)
            && !Raylib.CheckCollisionCircles(Raylib.GetMousePosition(), 1, new Vector2(Raylib.MeasureText("Mass: 10", 24) + 36, 46), 9)
            && !Raylib.CheckCollisionCircles(Raylib.GetMousePosition(), 1, new Vector2(Raylib.MeasureText("Mass: 10", 24) + 57, 16), 9)
            && !Raylib.CheckCollisionCircles(Raylib.GetMousePosition(), 1, new Vector2(Raylib.MeasureText("Mass: 10", 24) + 35, 16), 9)
            && planetList.Count > 0) planetList.RemoveAt(planetList.Count - 1);
            
        }
    }

    public class Planet
    {
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 force;
        public int mass = 1;
        bool trigger = false;
        public Raylib_cs.Color color;

        public Planet(Vector2 position, Vector2 velocity, Vector2 force)
        {
            this.position = position;
            this.velocity = velocity;
            this.force = force;
        }

        public Planet() { }

        public void AttractTo(Planet planet)
        {
            Vector2 direction = new Vector2();
            direction = planet.position - position;
            float distance = direction.Length();
            if(distance < 400) distance = 400;
            float forceMagnitude = mass * 100000 / (distance * distance * 3f) ;
            if (!trigger)
            {
                velocity = Rotate(direction, 90) * 0.05f;
                trigger = true;
            }
            direction = Vector2.Normalize(direction);
            force = direction * forceMagnitude;
        }

        public void UpdatePlanet()
        {
            velocity += force;
            position += velocity;
        }

        public static Vector2 Rotate(Vector2 vector, double degrees)
        {
            float cosine = (float)Math.Cos(degrees * Math.PI / 180);
            float sine = (float)Math.Sin(degrees * Math.PI / 180);

            return new Vector2(cosine * vector.X - sine * vector.Y, cosine * vector.X + sine * vector.Y);
        }
    }
}

  
