using UnityEngine;

namespace Gamekit2D
{
    internal interface IGravity
    {
        float GravityForce { get; }
        void SetGravity(Vector2 vector, float force);
    }
}
