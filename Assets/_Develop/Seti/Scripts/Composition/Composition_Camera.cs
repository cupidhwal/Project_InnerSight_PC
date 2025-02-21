using System.Collections;
using UnityEngine;

namespace Seti
{
    [CreateAssetMenu(fileName = "New Camera Action", menuName = "Scenario/Composition/Camera")]
    public class Composition_Camera : CompositionObject
    {
        // 연출
        [Header("Variables")]
        [SerializeField]
        float durationExcute = 1f;
        [SerializeField]
        float durationStay = 2f;
        [SerializeField]
        float durationComeback = 1f;

        [Header("Sensitivity")]
        [SerializeField]
        private float sharpnessExcute = 10f;
        [SerializeField]
        private float sharpnessComeback = 10f;

        public override void Execute(GameObject obj)
        {
            StoryManager.Instance.CorStopper();
            StoryManager.Instance.CorExcutor(CameraCor(obj.transform, durationExcute, durationStay, durationComeback));
        }

        // 반복기
        #region Coroutines
        // 카메라 연출 : target까지 excuteDuration 안에 도달했다가 stayDuration 동안 머물고 comebackDuration 안에 돌아오는 연출
        IEnumerator CameraCor(Transform target, float excuteDuration, float stayDuration = 1f, float comebackDuration = 1f)
        {
            // 플레이어 타게팅 해제
            StoryManager.Instance.Cinemachine.Target.TrackingTarget = null;

            // 타겟 지점으로 카메라 이동
            float elapsed = 0f;
            while (elapsed < excuteDuration)
            {
                elapsed += Time.deltaTime;
                StoryManager.Instance.Cinemachine.transform.position = Vector3.Lerp(StoryManager.Instance.Cinemachine.transform.position,
                                                                                    target.position,
                                                                                    sharpnessExcute * Time.deltaTime);

                yield return null;
            }

            // 타겟 지점에서 stayDuration만큼 대기
            yield return new WaitForSeconds(stayDuration);

            // 기존 지점으로 카메라 이동
            elapsed = 0f;
            while (elapsed < comebackDuration)
            {
                elapsed += Time.deltaTime;
                StoryManager.Instance.Cinemachine.transform.position = Vector3.Lerp(StoryManager.Instance.Cinemachine.transform.position,
                                                                                    StoryManager.Instance.Player.transform.position,
                                                                                    sharpnessComeback * Time.deltaTime);

                yield return null;
            }

            // 플레이어 타게팅 재설정
            StoryManager.Instance.Cinemachine.Target.TrackingTarget = StoryManager.Instance.Player.transform;

            yield break;
        }
        #endregion
    }
}