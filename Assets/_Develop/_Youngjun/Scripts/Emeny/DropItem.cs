using Seti;
using UnityEngine;

namespace Noah
{
    public class DropItem : MonoBehaviour
    {
        public GameObject gold;

        Damagable e_Damagable;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            e_Damagable = GetComponent<Damagable>();
            e_Damagable.OnDeath += EnemyDropItem;
        }

        void EnemyDropItem()
        {
            Invoke("SpwanItem", 2f);
        }

        void SpwanItem()
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);

            Instantiate(gold, pos, gold.transform.rotation, transform.parent);
        }
    }
}