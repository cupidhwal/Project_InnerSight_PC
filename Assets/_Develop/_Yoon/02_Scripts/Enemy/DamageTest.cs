using UnityEngine;

public class DamageTest : MonoBehaviour
{
    public EnemyHealth enemyHealth; // 적의 EnemyHealth 스크립트 연결

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10f); // 데미지 10씩 가함
            }
        }
    }
}
