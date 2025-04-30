using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Template.Collisions
{


    public interface IPPCollidable
    {


        public Vector2 CollisionPosition { get; }


        public List<RectangleData> Rectangles { get; set; }


        public bool DoHybridCheck(Rectangle other)
        {

            foreach (RectangleData rect in Rectangles)
                if (rect.Rectangle.Intersects(other))
                    return true;

            return false;

        }


        public bool PPCollisionCheck(IPPCollidable other)
        {

            foreach (RectangleData rect1 in Rectangles)
                foreach (RectangleData rect2 in other.Rectangles)
                    if (rect1.Rectangle.Intersects(rect2.Rectangle))
                        return true;

            return false;

        }


        public void UpdateRectangles(IPPCollidable collidable, int width, int height)
        {
            foreach (RectangleData rectangle in Rectangles)
                rectangle.UpdatePosition(this, width, height);
        }


        public List<RectangleData> CreateRectangles(Texture2D sprite)
        {

            List<RectangleData> rectangleList = new List<RectangleData>();
            List<Color[]> lines = new List<Color[]>();

            for (int i = 0; i < sprite.Height; i++)
            {

                Color[] colors = new Color[sprite.Width];
                sprite.GetData(0, new Rectangle(0, i,
                    sprite.Width, 1), colors, 0,
                    sprite.Width);
                lines.Add(colors);

            }

            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {

                    if (lines[y][x].A != 0)
                        if (x == 0 ||
                            x == lines[y].Length ||
                            x > 0 && lines[y][x - 1].A == 0 ||
                            x < lines[y].Length - 1 && lines[y][x + 1].A == 0 ||
                            y == 0 || y > 0 && lines[y - 1][x].A == 0 ||
                            y < lines.Count - 1 && lines[y + 1][x].A == 0)
                        {

                            RectangleData rd = new RectangleData(x, y);

                            rectangleList.Add(rd);

                        }

                }
            }

            return rectangleList;

        }



    }

    public class RectangleData
    {

        private Rectangle rectangle;
        private int x;
        private int y;


        public Rectangle Rectangle { get => rectangle; set => rectangle = value; }


        public int X { get => x; set => x = value; }


        public int Y { get => y; set => y = value; }


        public RectangleData(int x, int y)
        {

            rectangle = new Rectangle();
            this.x = x;
            this.y = y;

        }


        public void UpdatePosition(IPPCollidable collidable, int width, int height)
        {

            Rectangle = new Rectangle((int)collidable.CollisionPosition.X + X - width / 2, (int)collidable.CollisionPosition.Y + Y - height / 2, 1, 1);

        }

    }

}
