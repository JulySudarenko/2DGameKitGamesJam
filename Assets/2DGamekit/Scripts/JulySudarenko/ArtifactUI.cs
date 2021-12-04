using UnityEngine;
using UnityEngine.UI;

namespace Gamekit2D
{
    public class ArtifactUI : MonoBehaviour
    {
        public Image ArtifactIconPrefab;
        public Image ChargeIconPrefab;
        private readonly Text _gamePointsLabel;

        public void ArtifactActivate(float maxCharge, float charge)
        {
            ChargeIconPrefab.fillAmount = charge / maxCharge;
        }

        public void ActivateDeactivate(bool value)
        {
            if (value)
            {
                ArtifactIconPrefab.color = Color.white;
                ChargeIconPrefab.color = Color.white;
            }
            else
            {
                ArtifactIconPrefab.color = Color.black;
                ChargeIconPrefab.color = Color.black;
            }
        }
    }
}
