using UnityEngine;
using System.Collections;

namespace RPG.Core
{
    public class InvisibleObject : MonoBehaviour
    {
        void Start()
        {
            Material material = GetComponent<MeshRenderer>().material;
            material.color = new Color(1.0f, 1.0f, 1.0f, 0.1f);

            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }
    }
}