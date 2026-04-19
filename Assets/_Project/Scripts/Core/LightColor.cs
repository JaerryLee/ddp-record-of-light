using System;
using UnityEngine;

namespace DDP {
    [Flags]
    public enum LightColor {
        None  = 0,
        Red   = 1 << 0,
        Green = 1 << 1,
        Blue  = 1 << 2,
        White = Red | Green | Blue,
    }

    public static class LightColorEx {
        public static Color ToRenderColor(this LightColor c) {
            bool r = (c & LightColor.Red)   != 0;
            bool g = (c & LightColor.Green) != 0;
            bool b = (c & LightColor.Blue)  != 0;
            return new Color(r ? 1f : 0f, g ? 1f : 0f, b ? 1f : 0f, 1f);
        }

        public static bool Satisfies(this LightColor incoming, LightColor required) {
            return (incoming & required) == required && (incoming & ~required) == 0;
        }
    }
}
