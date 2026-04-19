using UnityEngine;

namespace DDP {
    public class PrismInstance : MonoBehaviour {
        [SerializeField] PrismType type = PrismType.Mirror;
        [SerializeField] LightColor filterPassColor = LightColor.Red;

        public PrismType Type => type;
        public LightColor FilterPassColor => filterPassColor;

        public void Rotate(float degrees) {
            transform.localRotation *= Quaternion.Euler(0f, degrees, 0f);
            BeamResolver.MarkDirty();
        }
    }
}
