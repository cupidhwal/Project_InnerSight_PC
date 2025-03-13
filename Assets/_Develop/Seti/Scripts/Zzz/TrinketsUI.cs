using UnityEngine;

namespace Seti
{
    public class TrinketsUI : MonoBehaviour
    {
        Condition_Player condition;

        private void Awake()
        {
            condition = InitializeManager.Instance.Player.Condition as Condition_Player;
        }

        private void OnEnable()
        {
            condition.PlayerSetActive(false);
        }

        private void OnDisable()
        {
            condition.PlayerSetActive(true);
        }
    }
}