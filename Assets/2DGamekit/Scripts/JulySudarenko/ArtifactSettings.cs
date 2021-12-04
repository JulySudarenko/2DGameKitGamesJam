using UnityEngine;

namespace Gamekit2D
{
    public class ArtifactSettings : MonoBehaviour
    {
        public Damager JumpArtefactDamager;
        public Collider2D CellingCollider;
        public Vector2 NormalGravity = new Vector2(0.0f,-9.81f);
        public Vector2 ArtefactGravity = new Vector2(0.0f, 9.81f);
        public float ArtefactForceGravity = 100.0f;
        public float Charge = 50.0f;
        public float ImprovedCharge = 100.0f;
    }
}
