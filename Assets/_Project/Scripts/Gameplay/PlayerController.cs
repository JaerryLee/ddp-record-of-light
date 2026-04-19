using UnityEngine;
using UnityEngine.InputSystem;

namespace DDP {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour {
        [Header("Movement (docs/01 §3)")]
        [SerializeField] float walkSpeed = 3.5f;
        [SerializeField] float jumpHeight = 1.2f;
        [SerializeField] float gravity = -19.62f;

        [Header("Look")]
        [SerializeField] Transform cameraRoot;
        [SerializeField, Range(0.2f, 3f)] float mouseSensitivity = 1.0f;
        [SerializeField] float minPitch = -85f;
        [SerializeField] float maxPitch =  85f;

        [Header("Interaction")]
        [SerializeField] float interactRange = 3f;
        [SerializeField] LayerMask interactMask = ~0;

        CharacterController cc;
        Vector3 velocity;
        float pitch;

        public IInteractable CurrentTarget { get; private set; }
        public event System.Action<IInteractable> OnTargetChanged;

        void Awake() {
            cc = GetComponent<CharacterController>();
            if (!cameraRoot) cameraRoot = transform;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update() {
            ReadLook();
            ReadMove();
            ReadInteract();
        }

        void ReadLook() {
            var mouse = Mouse.current;
            if (mouse == null) return;
            Vector2 delta = mouse.delta.ReadValue() * mouseSensitivity * 0.1f;
            transform.Rotate(0f, delta.x, 0f);
            pitch = Mathf.Clamp(pitch - delta.y, minPitch, maxPitch);
            cameraRoot.localEulerAngles = new Vector3(pitch, 0f, 0f);
        }

        void ReadMove() {
            var kb = Keyboard.current;
            if (kb == null) return;

            float x = (kb.dKey.isPressed ? 1f : 0f) + (kb.aKey.isPressed ? -1f : 0f);
            float z = (kb.wKey.isPressed ? 1f : 0f) + (kb.sKey.isPressed ? -1f : 0f);
            Vector3 move = (transform.right * x + transform.forward * z).normalized * walkSpeed;

            if (cc.isGrounded) {
                velocity.y = -1f;
                if (kb.spaceKey.wasPressedThisFrame)
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            } else {
                velocity.y += gravity * Time.deltaTime;
            }

            cc.Move((move + Vector3.up * velocity.y) * Time.deltaTime);
        }

        void ReadInteract() {
            IInteractable found = null;
            if (Physics.Raycast(cameraRoot.position, cameraRoot.forward, out var hit, interactRange, interactMask)) {
                found = hit.collider.GetComponentInParent<IInteractable>();
            }
            if (found != CurrentTarget) {
                CurrentTarget = found;
                OnTargetChanged?.Invoke(CurrentTarget);
            }

            var kb = Keyboard.current;
            var mouse = Mouse.current;
            bool interactPressed = (kb != null && kb.eKey.wasPressedThisFrame)
                                || (mouse != null && mouse.leftButton.wasPressedThisFrame);
            if (interactPressed && CurrentTarget != null && CurrentTarget.CanInteract)
                CurrentTarget.Interact();
        }
    }
}
