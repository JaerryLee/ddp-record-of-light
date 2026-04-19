using System.Collections.Generic;
using UnityEngine;

namespace DDP {
    // One resolver per scene. Computes all beam paths in LateUpdate so prism
    // placement from PlayerController (Update) is visible on the same frame.
    // Docs: 07 · Technical Design §5.
    [DefaultExecutionOrder(100)]
    public class BeamResolver : MonoBehaviour {
        public const int MAX_REFLECTIONS = 3;

        static readonly List<BeamEmitter> emitters = new();
        static bool dirty = true;

        public static void Register(BeamEmitter e)   { if (!emitters.Contains(e)) emitters.Add(e); dirty = true; }
        public static void Unregister(BeamEmitter e) { emitters.Remove(e); dirty = true; }
        public static void MarkDirty()               { dirty = true; }

        [SerializeField] Material beamMaterial;
        [SerializeField] LayerMask beamMask = ~0;
        [SerializeField] float beamWidth = 0.04f;

        readonly Dictionary<BeamEmitter, LineRenderer> lines = new();

        void LateUpdate() {
            EnsureRenderers();
            foreach (var e in emitters) if (e != null) Trace(e);
            dirty = false;
        }

        void EnsureRenderers() {
            foreach (var e in emitters) {
                if (e == null) continue;
                if (!lines.ContainsKey(e) || lines[e] == null) {
                    var go = new GameObject($"Beam_{e.name}");
                    go.transform.SetParent(transform, false);
                    var lr = go.AddComponent<LineRenderer>();
                    lr.material = beamMaterial;
                    lr.startWidth = beamWidth;
                    lr.endWidth = beamWidth;
                    lr.useWorldSpace = true;
                    lr.numCapVertices = 4;
                    lines[e] = lr;
                }
            }
        }

        void Trace(BeamEmitter emitter) {
            var lr = lines[emitter];
            var path = new List<Vector3> { emitter.Origin };

            Vector3 origin = emitter.Origin;
            Vector3 dir = emitter.Direction;
            LightColor color = emitter.Color;
            float remaining = emitter.MaxRange;

            for (int hits = 0; hits <= MAX_REFLECTIONS && remaining > 0f; hits++) {
                if (!Physics.Raycast(origin, dir, out var hit, remaining, beamMask, QueryTriggerInteraction.Collide)) {
                    path.Add(origin + dir * remaining);
                    break;
                }
                path.Add(hit.point);
                remaining -= hit.distance;

                var crystal = hit.collider.GetComponentInParent<MemoryCrystal>();
                if (crystal != null) { crystal.ReceiveBeam(color); break; }

                var prism = hit.collider.GetComponentInParent<PrismInstance>();
                if (prism != null && hits < MAX_REFLECTIONS) {
                    origin = hit.point + dir * 0.001f;
                    ReflectThroughPrism(prism, hit.normal, ref dir, ref color);
                    if (color == LightColor.None) break;
                    continue;
                }
                break; // static world hit
            }

            lr.positionCount = path.Count;
            lr.SetPositions(path.ToArray());
            var rc = color.ToRenderColor();
            lr.startColor = rc; lr.endColor = rc;
        }

        static void ReflectThroughPrism(PrismInstance p, Vector3 normal, ref Vector3 dir, ref LightColor color) {
            switch (p.Type) {
                case PrismType.Mirror:
                case PrismType.MirrorPlus:
                    dir = Vector3.Reflect(dir, normal);
                    break;
                case PrismType.FilterR: color &= LightColor.Red;   break;
                case PrismType.FilterG: color &= LightColor.Green; break;
                case PrismType.FilterB: color &= LightColor.Blue;  break;
                case PrismType.Splitter:
                    // Sprint 2: spawn 3 sub-beams (R/G/B). Sprint 1 stub reflects only.
                    dir = Vector3.Reflect(dir, normal);
                    break;
            }
        }
    }
}
