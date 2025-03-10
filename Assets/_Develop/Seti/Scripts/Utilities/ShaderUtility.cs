using UnityEngine;

namespace Seti
{
    public static class ShaderUtility
    {
        // Material이 갖는 모든 속성의 이름을 출력하는 유틸리티
        public static void ShowProperties(Material material)
        {
            Shader shader = material.shader;
            int propertyCount = shader.GetPropertyCount();

            for (int i = 0; i < propertyCount; i++)
            {
                string propertyName = shader.GetPropertyName(i);
                Debug.Log($"Shader Property: {propertyName}");
            }
        }
    }
}