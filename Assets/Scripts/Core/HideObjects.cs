using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class HideObjects : MonoBehaviour
    {
        Dictionary<string, GameObject> hidedObjectHits = null;
        GameObject player = null;

        private void Awake()
        {
            hidedObjectHits = new Dictionary<string, GameObject>();
            player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            bool hasHit = false;

            foreach (RaycastHit hit in hits)
            {
                if (hidedObjectHits.ContainsKey(hit.transform.gameObject.name))
                {
                    hasHit = true;
                    continue;
                }

                if (hit.transform.gameObject.tag == "HideObject")
                {
                    hidedObjectHits[hit.transform.gameObject.name] = hit.transform.gameObject;
                    HideObject(hit.transform.gameObject);

                    hasHit = true;
                }
            }

            if (!hasHit && hidedObjectHits.Count > 0)
            {
                List<string> keysToRemove = new List<string>();

                foreach (KeyValuePair<string, GameObject> entry in hidedObjectHits)
                {
                    if (entry.Value == null) continue;

                    ShowObject(entry.Value);
                    keysToRemove.Add(entry.Value.name);
                }

                foreach (var keyToRemove in keysToRemove)
                {
                    hidedObjectHits.Remove(keyToRemove);
                }
            }
        }

        private void HideObject(GameObject objectToHide)
        {
            Transform[] childs = objectToHide.GetComponentsInChildren<Transform>();
            foreach (Transform child in childs)
            {
                MeshRenderer childsMeshRenderer = child.GetComponent<MeshRenderer>();

                if (childsMeshRenderer == null) continue;
                TransparentMaterial(childsMeshRenderer.material);
            }
            MeshRenderer meshRenderer = objectToHide.GetComponent<MeshRenderer>();
            if (meshRenderer == null) return;
            TransparentMaterial(meshRenderer.material);
        }

        private void TransparentMaterial(Material material)
        {
            material.color = new Color(1.0f, 1.0f, 1.0f, 0f);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }

        private void ShowObject(GameObject objectToShow)
        {
            Transform[] childs = objectToShow.GetComponentsInChildren<Transform>();
            foreach (Transform child in childs)
            {
                MeshRenderer childsMeshRenderer = child.GetComponent<MeshRenderer>();

                if (childsMeshRenderer == null) continue;
                OpaqueMaterial(childsMeshRenderer.material);
            }
            MeshRenderer meshRenderer = objectToShow.GetComponent<MeshRenderer>();
            if (meshRenderer == null) return;
            OpaqueMaterial(meshRenderer.material);
        }

        private void OpaqueMaterial(Material material)
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;
        }
    }
}