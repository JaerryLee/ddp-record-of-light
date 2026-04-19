using System;
using UnityEngine;

namespace DDP {
    public class PrismInventory : MonoBehaviour {
        public static PrismInventory Instance { get; private set; }

        [Serializable]
        public struct Entry {
            public PrismType type;
            public PrismInstance prefab;
        }

        [SerializeField] Entry[] prefabs;

        void Awake() {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        public PrismInstance Spawn(PrismType type) {
            foreach (var e in prefabs) {
                if (e.type == type && e.prefab != null)
                    return Instantiate(e.prefab);
            }
            return null;
        }
    }
}
