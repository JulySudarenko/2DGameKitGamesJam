using UnityEngine;

namespace Gamekit2D
{
    internal class Gravitation
    {
        public float GravityForce { get; protected set; }

        public void SetGravity(Vector2 vector, float force)
        {
            Physics2D.gravity = vector;
            GravityForce = force;
        }
    }
}
