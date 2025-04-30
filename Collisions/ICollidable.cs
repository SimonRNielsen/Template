using Microsoft.Xna.Framework;

namespace Template.Collisions
{


    public interface ICollidable
    {


        public Rectangle CollisionBox { get; }


        public bool CheckCollision(ICollidable other)
        {

            if (CollisionBox.Intersects(other.CollisionBox))
                return true;
            else
                return false;

        }


        public void OnCollision();


    }

}
