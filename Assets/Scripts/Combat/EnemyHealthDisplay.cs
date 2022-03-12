using RPG.Combat;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        private void Start()
        {
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (fighter.GetTarget() == null)
            {
                GetComponent<Text>().text = "Not available";

                return;
            }
            Health health = fighter.GetTarget();

            GetComponent<Text>().text = string.Format("{0:0.0}% {1:0}/{2:0}", health.GetPercentage(), health.GetHelthPoints(), health.GetMaxHealthPoints());
        }
    }
}