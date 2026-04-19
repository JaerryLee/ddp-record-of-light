using UnityEngine;

namespace DDP {
    public enum PrismType { Mirror, Splitter, FilterR, FilterG, FilterB, MirrorPlus }

    public class PrismNode : MonoBehaviour, IInteractable {
        [SerializeField] PrismType allowedType = PrismType.Mirror;
        [SerializeField] Transform prismAnchor;

        public PrismType AllowedType => allowedType;
        public PrismInstance Current { get; private set; }
        public Transform PrismAnchor => prismAnchor ? prismAnchor : transform;

        public string PromptText => Current == null ? "프리즘 배치 [E]" : "프리즘 회수 [E]";
        public bool CanInteract => true;

        public void Interact() {
            if (Current == null) PlaceDefault();
            else Remove();
        }

        public void Place(PrismInstance instance) {
            if (Current != null || instance == null) return;
            Current = instance;
            instance.transform.SetParent(PrismAnchor, false);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            BeamResolver.MarkDirty();
        }

        public void Remove() {
            if (Current == null) return;
            Destroy(Current.gameObject);
            Current = null;
            BeamResolver.MarkDirty();
        }

        void PlaceDefault() {
            if (PrismInventory.Instance == null) return;
            var instance = PrismInventory.Instance.Spawn(allowedType);
            if (instance != null) Place(instance);
        }

        void OnDrawGizmos() {
            Gizmos.color = Current == null
                ? new Color(1f, 0.7f, 0.3f, 0.6f)
                : new Color(0.3f, 1f, 0.5f, 0.6f);
            Gizmos.DrawWireSphere(transform.position, 0.25f);
        }
    }
}
