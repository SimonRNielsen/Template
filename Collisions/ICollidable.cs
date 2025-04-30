using Microsoft.Xna.Framework;
using System;

namespace Template.Collisions
{


    public interface ICollidable
    {


        public Enum Type { get; }


        public Rectangle CollisionBox { get; }


        public bool CheckCollision(ICollidable other)
        {

            if (CollisionBox.Intersects(other.CollisionBox))
                return true;
            else
                return false;

        }


        public void OnCollision(ICollidable other);


    }

}
