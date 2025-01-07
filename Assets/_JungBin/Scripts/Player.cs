using UnityEngine;

namespace JungBin
{

    public class Player : MonoBehaviour
    {
        public static int Lives { get; private set; } = 1; // 기본 생명 수
        public bool IsAlive => Lives > 0;           // 생존 여부

        // 생명 추가 메서드
        public void AddLife(int amount)
        {
            Lives += amount;
            Debug.Log($"생명 추가됨: 현재 생명 수 {Lives}");
        }

        // 사망 메서드
        public void Die()
        {
            if (Lives > 0) // 생명이 남아 있을 때만 감소
            {
                Lives--; // 생명 감소
                //이곳에 사망 애니메이션 추가
                if (Lives == 0) // 감소 후 생명이 없으면 죽음 처리
                {
                    //게임 오버 UI 출력
                    Debug.Log("플레이어 죽음");
                }
                else
                {
                    //소생 애니메이션 추가
                    Debug.Log($"플레이어 소생 남은 목숨 수 : {Lives}");
                }
            }
            else
            {
                Debug.Log("플레이어는 이미 죽음 상태입니다.");
            }
        }
    }
}