using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestComplition : MonoBehaviour
    {
        [SerializeField] Quest quest;
        [SerializeField] string objective;

        public void CompleteObjective()
        {
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.CompleteObjective(quest, objective);
        }
    }
}