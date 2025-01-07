//using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noah
{
    public class RandomSkill : MonoBehaviour
    {
        private Transform effect_obj;

        private Vector3 startPos;

        private float amplitude = 0.2f;  // ������Ʈ�� �̵��� �ִ� �Ÿ�
        private float frequency = 2f;    // ������ �ӵ�

        private void Start()
        {
            startPos = transform.position;

            if (transform.GetChild(0) != null)
            {
                effect_obj = transform.GetChild(0);
            }
        }

        private void Update()
        {
            MoveObject();
        }

        void MoveObject()
        {
            // �ð��� ���� y ��ġ�� sin �Լ��� ����
            float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

            // ���ο� ��ġ�� �̵�
            transform.position = new Vector3(startPos.x, newY, startPos.z);
        }

        void DestroyObject()
        {
            effect_obj.gameObject.GetComponent<ParticleSystem>().Stop();
            //transform.gameObject.GetComponent<SpriteRenderer>().material.DOFade(0, 1f).OnComplete(() =>
            Destroy(gameObject);
        }



        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                transform.GetComponent<Collider>().enabled = false;

                UIManager.Instance.skillSelectUI.SetActive(true);
                InGameUI_Skill.instance.GetRandomSkill();
                Time.timeScale = 0f;

                DestroyObject();
            }
        }

    }
}