using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;

        private void Awake()
        {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            GetComponent<Text>().text = string.Format("{0:0.0}% {1:0}/{2:0}", health.GetPercentage(), health.GetHelthPoints(), health.GetMaxHealthPoints());
        }
    }
}