using Microsoft.Xna.Framework;

namespace Template
{
    internal interface ICollidable<T>
    {
        
        public abstract bool CheckCollision(T collider, T other);
        
        public abstract void OnCollision(T collider, T other);

    }
}
