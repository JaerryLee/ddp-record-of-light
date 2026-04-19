using UnityEngine;

namespace DDP {
    // Sprint 1 stub. Sprint 2: load AudioClip per crystalId, show hologram
    // prefab, drive subtitle timeline. Docs: 01 §5, 05 §5.
    public class VignettePlayer : MonoBehaviour {
        public static VignettePlayer Instance { get; private set; }

        void Awake() {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        public static void RequestPlay(string crystalId) {
            if (Instance != null) Instance.Play(crystalId);
            else Debug.Log($"[Vignette] {crystalId} (no VignettePlayer in scene)");
        }

        public void Play(string crystalId) {
            Debug.Log($"[Vignette] playing {crystalId}");
        }
    }
}
