using UnityEngine;

namespace DDP {
    public class BeamEmitter : MonoBehaviour {
        [SerializeField] LightColor initialColor = LightColor.White;
        [SerializeField] float maxRange = 40f;

        public LightColor Color => initialColor;
        public float MaxRange => maxRange;
        public Vector3 Origin => transform.position;
        public Vector3 Direction => transform.forward;

        void OnEnable()  { BeamResolver.Register(this); }
        void OnDisable() { BeamResolver.Unregister(this); }

        void OnDrawGizmos() {
            Gizmos.color = new Color(1f, 0.7f, 0.3f);
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 0.5f);
        }
    }
}
