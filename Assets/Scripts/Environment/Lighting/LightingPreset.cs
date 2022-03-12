using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Environment.Lighting
{

    [System.Serializable]
    [CreateAssetMenu(fileName = "Lighting Preset", menuName = "Lighting/Lighting preset", order = 1)]
    public class LightingPreset : ScriptableObject
    {
        public Gradient ambientColor;
        public Gradient directionalColor;
        public Gradient fogColor;
    }
}