using UnityEngine;
using Action = System.Action;

namespace Gamekit2D
{
    public class ArtifactSettings : MonoBehaviour
    {
        public Action OnGetArtifact;
        public Action OnImprovedCharge;
        
        public Damager JumpArtifactDamager;
        public ArtifactUI ArtifactView;
        public Vector2 NormalGravity = new Vector2(0.0f, -9.81f);
        public Vector2 ArtifactGravity = new Vector2(0.0f, 9.81f);
        public float ArtifactForceGravity = -100.0f;
        public float Charge = 50.0f;
        public float ImprovedCharge = 50.0f;
        public float MaxCharge = 80.0f;

        public void GetArtifact()
        {
            OnGetArtifact();
        }
        
        public void SetImprovedCharge()
        {
            OnImprovedCharge?.Invoke();
        }
    }
}
