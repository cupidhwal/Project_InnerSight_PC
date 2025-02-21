using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

namespace Seti
{
    /// <summary>
    /// 연출 담당 클래스
    /// </summary>
    public class Composition : MonoBehaviour
    {
        // 필드
        #region Variables
        // 기본
        private Player player;

        [Header("Variables")]
        [SerializeField]
        private CinemachineCamera cinemachine;

        // 연출
        [Header("Composition : Camera")]
        [SerializeField]
        private float excuteSharpness = 10f;
        [SerializeField]
        private float comebackSharpness = 10f;
        #endregion

        // 라이프 사이클
        private void Start()
        {
            player = cinemachine.Target.TrackingTarget.transform.GetComponent<Player>();
        }

        // 메서드
        public void Composition_Switch(GameObject target)
        {
            target.SetActive(!target.activeSelf);
        }

        public void Composition_Camera(Transform target, float excuteDuration, float stayDuration = 1f, float comebackDuration = 1f)
        {
            StopAllCoroutines();
            StartCoroutine(CameraCor(target, excuteDuration, stayDuration, comebackDuration));
        }

        // 반복기
        #region Coroutines
        // 카메라 연출 : target까지 excuteDuration 안에 도달했다가 stayDuration 동안 머물고 comebackDuration 안에 돌아오는 연출
        IEnumerator CameraCor(Transform target, float excuteDuration, float stayDuration = 1f, float comebackDuration = 1f)
        {
            // 플레이어 타게팅 해제
            cinemachine.Target.TrackingTarget = null;

            // 타겟 지점으로 카메라 이동
            float elapsed = 0f;
            while (elapsed < excuteDuration)
            {
                elapsed += Time.deltaTime;
                cinemachine.transform.position = Vector3.Lerp(cinemachine.transform.position, target.position, excuteSharpness * Time.deltaTime);

                yield return null;
            }

            // 타겟 지점에서 stayDuration만큼 대기
            yield return new WaitForSeconds(stayDuration);

            // 기존 지점으로 카메라 이동
            elapsed = 0f;
            while (elapsed < comebackDuration)
            {
                elapsed += Time.deltaTime;
                cinemachine.transform.position = Vector3.Lerp(cinemachine.transform.position, player.transform.position, comebackSharpness * Time.deltaTime);

                yield return null;
            }

            // 플레이어 타게팅 재설정
            cinemachine.Target.TrackingTarget = player.transform;

            yield break;
        }
        #endregion
    }
}