using UnityEngine;

namespace Gamekit2D
{
    internal class Gravitation : IGravity
    {
        public float GravityForce { get; private set; }

        public void SetGravity(Vector2 vector, float force)
        {
            Physics2D.gravity = vector;
            GravityForce = force;
        }
    }
}
