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

        private float amplitude = 0.2f;  // 오브젝트가 이동할 최대 거리
        private float frequency = 2f;    // 진동의 속도

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
            // 시간에 따라 y 위치를 sin 함수로 변경
            float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

            // 새로운 위치로 이동
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