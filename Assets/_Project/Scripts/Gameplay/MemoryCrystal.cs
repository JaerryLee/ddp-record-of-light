using UnityEngine;
using UnityEngine.Events;

namespace DDP {
    public class MemoryCrystal : MonoBehaviour {
        [SerializeField] string crystalId = "C1";
        [SerializeField] LightColor requiredColor = LightColor.White;
        [SerializeField, Min(0f)] float holdSeconds = 1f;
        [SerializeField] UnityEvent onActivated;

        public string Id => crystalId;
        public bool Activated { get; private set; }

        float holdTimer;
        LightColor receivedThisFrame;
        int   lastFrameReceived;

        public void ReceiveBeam(LightColor color) {
            receivedThisFrame = color;
            lastFrameReceived = Time.frameCount;
        }

        void Update() {
            if (Activated) return;

            bool match = lastFrameReceived == Time.frameCount
                      && receivedThisFrame.Satisfies(requiredColor);

            if (match) {
                holdTimer += Time.deltaTime;
                if (holdTimer >= holdSeconds) Activate();
            } else {
                holdTimer = 0f;
            }
        }

        void Activate() {
            Activated = true;
            onActivated?.Invoke();
            VignettePlayer.RequestPlay(crystalId);
        }

        void OnDrawGizmos() {
            Gizmos.color = Activated ? Color.cyan : new Color(0.5f, 0.8f, 1f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }
    }
}
