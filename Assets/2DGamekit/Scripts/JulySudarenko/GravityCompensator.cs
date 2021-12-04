using UnityEngine;

namespace Gamekit2D
{
    public class GravityCompensator
    {
        private Rigidbody2D _rigidbody2D;

        public GravityCompensator(Rigidbody2D rigidbody2D)
        {
            _rigidbody2D = rigidbody2D;
        }

        public void CheckAndCompensateGravity()
        {
            if (Physics2D.gravity.y > 0)
            {
                _rigidbody2D.constraints |= RigidbodyConstraints2D.FreezePositionY;
            }
            else
            {
                _rigidbody2D.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            }
        }
    }
}
