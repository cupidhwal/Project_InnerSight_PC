using Seti;
using UnityEngine;

namespace Noah
{
    public class FireObjectDamage : MonoBehaviour
    {
        [SerializeField] private float ctime = 0.5f;
        [SerializeField] private float attackDur = 0.5f;
        [SerializeField] private float damage = 5f;

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Damagable ec = other.GetComponent<Damagable>();

                if (ec != null)
                {
                    ctime += Time.deltaTime;

                    while (ctime > attackDur)
                    {
                        // 데미지 데이터 가공 후 데미지 주기
                        Damagable.DamageMessage data = new();
                        data.amount = damage;

                        ec.TakeDamage(data);

                        ctime = 0;
                    }
                }
            }         
        }
    }
}