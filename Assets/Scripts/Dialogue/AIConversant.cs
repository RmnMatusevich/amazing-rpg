using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] Dialogue dialogue = null;
        [SerializeField] string conversantName;

        CursorType IRaycastable.GetCursorType()
        {
            return CursorType.Dialogue;
        }

        bool IRaycastable.HandleRaycast(PlayerController callingController)
        {
            if (dialogue == null) return false;

            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
            }

            return true;
        }

        public string GetName()
        {
            return conversantName;
        }
    }
}