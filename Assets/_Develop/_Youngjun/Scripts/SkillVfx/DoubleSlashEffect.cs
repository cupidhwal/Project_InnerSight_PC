using System.Collections;
using UnityEngine;

namespace Noah
{
    public class DoubleSlashEffect : MonoBehaviour
    {
        public GameObject secondAttack;

        void TakeSecondAttack()
        {
            secondAttack.SetActive(true);

            secondAttack.GetComponent<ParticleSystem>().Play();
        }
        
    }

}
