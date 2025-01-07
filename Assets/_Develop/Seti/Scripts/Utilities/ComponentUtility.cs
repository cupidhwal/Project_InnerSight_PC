using UnityEngine;

namespace Seti
{
    // MonoBehaviour에 없는 편의용 메서드 모음
    public static class ComponentUtility
    {
        // 부모 체인에서 T 타입의 컴포넌트를 안전하게 가져오는 메서드 (스태틱 메서드)
        public static bool TryGetComponentInParent<T>(Transform transform, out T component) where T : Component
        {
            component = transform.GetComponentInParent<T>();
            return component != null;
        }

        // 자식 체인에서 T 타입의 컴포넌트를 안전하게 가져오는 메서드 (스태틱 메서드)
        public static bool TryGetComponentInChildren<T>(Transform transform, out T component) where T : Component
        {
            component = transform.GetComponentInChildren<T>();
            return component != null;
        }

        // 자신을 포함하여 모든 계층을 순회하며 컴포넌트를 찾는 메서드 (스태틱 메서드)
        public static bool TryGetComponentAll<T>(Transform transform, out T component) where T : Component
        {
            component = transform.GetComponent<T>() ??
                        transform.GetComponentInChildren<T>() ??
                        transform.GetComponentInParent<T>();
            return component != null;
        }
    }
}