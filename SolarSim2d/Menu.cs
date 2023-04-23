using Raylib_cs;
using System.Numerics;

namespace SolarSim2d
{
    public class Menu
    {
        RayTimer timer = new RayTimer(0.5);
        
        bool pointerState = true;
        int pointerPosition = 0;
        int linePosition = 5;

        List<Title> titleList = new List<Title>();
        List<MenuItem> itemList = new List<MenuItem>();

        int numberOfItems;

        Raylib_cs.Color backgroundColor;

        public Menu(Raylib_cs.Color color)
        {
            backgroundColor = color;
        }

        public int CheckUpDownKeys()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN)) return 1;
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP)) return 2;
            else return 0;
        }

        public static bool CheckEnter()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER)) return true;
            else return false;
        }

        public int GetPointerPosition()
        {
            return pointerPosition;
        }

        public void AddTitle(string text, int level, Raylib_cs.Color color)
        {
            Title title = new Title(text, linePosition, level, color);

            if (title.level == 1) title.font = 36;
            else if (title.level == 2) title.font = 32;
            else if (title.level == 3) title.font = 28;

            titleList.Add(title);

            linePosition += 40;

            if (level < 1 || level > 3) Console.WriteLine("Invalid title index");
        }

        public void AddItem(string text, Raylib_cs.Color color)
        {
            if (itemList.Count == 0) linePosition += 40;
            
            MenuItem item = new MenuItem(text, linePosition, color);
            itemList.Add(item);

            linePosition += 30;
            numberOfItems++;
        }

        public void DrawMenu()
        {
            Raylib.ClearBackground(backgroundColor);

            if (titleList.Count + itemList.Count < 1) return;

            if(titleList.Count > 0)
            {
                for (int i = 0; i < titleList.Count; i++) Raylib.DrawText(titleList[i].text, 5, titleList[i].position, titleList[i].font, titleList[i].color);
            }

            if (itemList.Count > 0)
            {
                for (int i = 0; i < itemList.Count; i++) Raylib.DrawText(">" + itemList[i].text, 5, itemList[i].position, 28, itemList[i].color);
            }

            if(pointerState && itemList.Count > 0)
            {
                Raylib.DrawRectangle(Raylib.MeasureText(itemList[pointerPosition].text, 28) + 15, itemList[pointerPosition].position, 15, 25, itemList[pointerPosition].color);
            }

            if (CheckUpDownKeys() == 1 && pointerPosition < numberOfItems - 1) pointerPosition++;
            if (CheckUpDownKeys() == 2 && pointerPosition > 0) pointerPosition--;

            timer.UpdateTimer();
            if (!timer.CheckTimer()) 
            {
                if (pointerState) pointerState = false;
                else pointerState = true;
                timer.ResetTimer();
            }
        }
    }

    public class MenuItem
    {
        public string text = "Menu Item";
        public int position;
        public Raylib_cs.Color color;

        public MenuItem(){}
        public MenuItem(string text, int position, Raylib_cs.Color color)
        { 
            this.text = text;
            this.position = position;
            this.color = color;
        }
    }

    public class Title : MenuItem
    {
        public int level;
        public int font;

        public Title(string text, int position, int level, Raylib_cs.Color color) 
        {
            this.text = text;
            this.position = position;
            this.level = level;
            this.color = color;
        }
    }
}



