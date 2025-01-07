using System.Collections.Generic;
using UnityEngine;

namespace Seti
{
    // 수학 유틸리티
    public class MathUtility
    {
        #region 일반수학
        // 자연대수 지수함수(exponential function) 연산
        public static float CalDisExp(float x, float amplitude, float exponentialRate, float horizontalShift)
        {
            return (float)(amplitude
                * Mathf.Exp(exponentialRate * (x - horizontalShift)));
        }

        // 각도 기반 감쇠 함수 (r = 1, center = (1, 1)인 원)
        public static Vector2 CalAngExp(float deg)
        {
            // 원의 방정식을 이용해 각 점의 x, y 좌표 계산
            float rad = deg / Mathf.Rad2Deg;

            float x = 1 - Mathf.Cos(rad);
            float y = 1 - Mathf.Sin(rad);

            return new Vector2(x, y);
        }

        // Random Vector2
        public static Vector2 RanVec2(float x, float y)
        {
            float randomX = Random.Range(-x, x);
            float randomY = Random.Range(-y, y);

            return new Vector2(randomX, randomY);
        }

        // Vector3에서 1개의 변수를 몰라도 원하는 크기의 벡터를 반환
        public static Vector3 CalVec3(float mag, float x, float y)
        {
            // 크기를 3으로 유지하기 위한 Z값 계산
            float z = Mathf.Sqrt(mag * mag - x * x - y * y);

            // 새로운 위치 벡터
            return new Vector3(x, y, z);
        }

        // 입력 각도, 원 위의 점 하나를 반환하는 메서드
        public static Vector2 GetCirclePos(Vector2 center, float r, float rad)
        {
            // 원의 방정식을 이용해 각 점의 x, y 좌표 계산
            float x = center.x + r * Mathf.Cos(rad);
            float y = center.y + r * Mathf.Sin(rad);

            return new Vector2(x, y);
        }

        // 입력 각도, 타원 위의 점 하나를 반환하는 메서드
        public static Vector2 GetEllPos(Vector2 center, float a, float b, float rad)
        {
            // 타원의 파라메트릭 방정식을 이용해 각 점의 x, y 좌표 계산
            float x = center.x + a * Mathf.Cos(rad);
            float y = center.y + b * Mathf.Sin(rad);

            return new Vector2(x, y);
        }

        // 타원 위에 리스트 크기만큼 점을 배치하는 메서드
        public static List<Vector2> GetEllipsePositions(Vector2 center, float a, float b, int count)
        {
            List<Vector2> positions = new();

            // 각도를 리스트 크기만큼 등분하여 각 점의 위치를 계산
            for (int i = 0; i < count; i++)
            {
                float angle = 2 * Mathf.PI * i / count; // 0 ~ 2*PI 사이의 각도 계산

                positions.Add(GetEllPos(center, a, b, angle));
            }

            return positions;
        }
        #endregion

        #region 최단거리
        // 가장 가까운 GameObject를 찾는 메서드 (GameObject 리스트)
        public static GameObject MinDisObject(GameObject gameObject, List<GameObject> gameObjectList)
        {
            float closestDistance = float.MaxValue;
            GameObject closestObject = null;

            foreach (GameObject obj in gameObjectList)
            {
                if (obj == gameObject) continue;

                float distance = Vector3.Distance(gameObject.transform.position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = obj;
                }
            }

            return closestObject;
        }

        // 가장 가까운 Transform을 찾는 메서드 (GameObject 리스트에서 Transform을 추출)
        public static Transform MinDistance(Transform referenceTransform, List<GameObject> gameObjectList)
        {
            float closestDistance = float.MaxValue;
            Transform closestObject = null;

            foreach (GameObject obj in gameObjectList)
            {
                float distance = Vector3.Distance(referenceTransform.position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = obj.transform;
                }
            }

            return closestObject;
        }

        // 가장 가까운 GameObject를 찾는 메서드
        public static GameObject MinDistance(GameObject gameObject, IEnumerable<Transform> transforms)
        {
            float closestDistance = float.MaxValue;
            GameObject closestObject = null;

            foreach (Transform objTransform in transforms)
            {
                float distance = Vector3.Distance(gameObject.transform.position, objTransform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = objTransform.gameObject;
                }
            }

            return closestObject;
        }

        // 가장 가까운 Transform을 찾는 메서드
        public static Transform MinDistance(Transform referenceTransform, IEnumerable<Transform> transforms)
        {
            float closestDistance = float.MaxValue;
            Transform closestObject = null;

            foreach (Transform objTransform in transforms)
            {
                float distance = Vector3.Distance(referenceTransform.position, objTransform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = objTransform;
                }
            }

            return closestObject;
        }

        // 배열을 처리하는 오버로드 (배열을 List로 변환)
        public static GameObject MinDistance(GameObject gameObject, GameObject[] array)
        {
            return MinDistance(gameObject, ArrayToTransformList(array));
        }

        public static Transform MinDistance(Transform referenceTransform, GameObject[] array)
        {
            return MinDistance(referenceTransform, ArrayToTransformList(array));
        }

        // Dictionary에서 가장 가까운 GameObject 찾는 메서드 (Key 기준)
        public static GameObject MinDistance(GameObject gameObject, Dictionary<GameObject, GameObject> dictionary)
        {
            float closestDistance = float.MaxValue;
            GameObject closestObject = null;

            foreach (var pair in dictionary)
            {
                float distance = Vector3.Distance(gameObject.transform.position, pair.Key.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = pair.Key;
                }
            }

            return closestObject;
        }

        // 배열을 Transform 리스트로 변환하는 유틸리티 메서드
        private static List<Transform> ArrayToTransformList(GameObject[] array)
        {
            List<Transform> transformList = new();
            foreach (GameObject obj in array)
            {
                transformList.Add(obj.transform);
            }
            return transformList;
        }
        #endregion

        #region 랜덤배치
        // 원 1개를 이용하는 랜덤배치
        public static void ReArrObjects(float r, List<GameObject> objectList)
        {
            float angle = (2 * Mathf.PI) / objectList.Count; // 0 ~ 2*PI 사이의 각도 계산
            Vector2 center = new(0, 0);

            float randomAngleOffset = Random.Range(0, 2 * Mathf.PI);

            // 각도를 리스트 크기만큼 등분하여 각 점의 위치를 계산
            for (int i = 0; i < objectList.Count; i++)
            {
                RectTransform objRect = objectList[i].GetComponent<RectTransform>();

                objRect.anchoredPosition = GetCirclePos(center, r, angle * i + randomAngleOffset);
            }
        }

        // 중심점과 원 1개를 이용하는 랜덤배치
        public static void ReArrObjects(Vector2 center, float r, List<GameObject> objectList)
        {
            float angle = (2 * Mathf.PI) / objectList.Count; // 0 ~ 2*PI 사이의 각도 계산

            float randomAngleOffset = Random.Range(0, 2 * Mathf.PI);

            // 각도를 리스트 크기만큼 등분하여 각 점의 위치를 계산
            for (int i = 0; i < objectList.Count; i++)
            {
                RectTransform objRect = objectList[i].GetComponent<RectTransform>();

                objRect.anchoredPosition = GetCirclePos(center, r, angle * i + randomAngleOffset);
            }
        }

        // 작은 원과 큰 타원을 이용하는 랜덤배치
        public static void ReArrObjects(float r, float a, float b, List<GameObject> objectList)
        {
            float angle = (2 * Mathf.PI) / objectList.Count; // 0 ~ 2*PI 사이의 각도 계산
            Vector2 center = new(0, 0);

            float randomAngleOffset = Random.Range(Mathf.PI, 3 * Mathf.PI);

            // 각도를 리스트 크기만큼 등분하여 각 점의 위치를 계산
            for (int i = 0; i < objectList.Count; i++)
            {
                RectTransform objRect = objectList[i].GetComponent<RectTransform>();

                // 0번 인덱스는 반드시 가운데에 배치
                if (i == 0) objRect.anchoredPosition = Vector2.zero;

                else
                {
                    // 짝수(count % 2 == 0) 인덱스
                    if (i % 2 == 0)
                        objRect.anchoredPosition = GetCirclePos(center, r, (angle * i) + randomAngleOffset);

                    // 홀수(count % 2 == 1) 인덱스
                    else
                        objRect.anchoredPosition = GetEllPos(center, 2 * a, 2 * b, (angle * i) + randomAngleOffset);
                }
            }
        }

        // 원 두 개를 이용하는 랜덤배치
        public static void ReArrObjects(float r1, float r2, List<GameObject> objectList)
        {
            float angle = (2 * Mathf.PI) / objectList.Count; // 0 ~ 2*PI 사이의 각도 계산
            Vector2 center = new(0, 0);

            float randomAngleOffset = Random.Range(0, 2 * Mathf.PI);

            // 각도를 리스트 크기만큼 등분하여 각 점의 위치를 계산
            for (int i = 0; i < objectList.Count; i++)
            {
                if (objectList.Count < 2) break;

                RectTransform objRect = objectList[i].GetComponent<RectTransform>();

                // 짝수(count % 2 == 0) 인덱스
                if (i % 2 == 0)
                    objRect.anchoredPosition = GetCirclePos(center, r1, angle * i + randomAngleOffset);

                // 홀수(count % 2 == 1) 인덱스
                else
                    objRect.anchoredPosition = GetCirclePos(center, 2 * r2, angle * i + randomAngleOffset);
            }
        }

        // 타원 두 개를 이용하는 랜덤배치
        public static void ReArrObjects(float a1, float b1, float a2, float b2, List<GameObject> objectList)
        {
            float angle = (2 * Mathf.PI) / objectList.Count; // 0 ~ 2*PI 사이의 각도 계산
            Vector2 center = new(0, 0);

            float randomAngleOffset = Random.Range(0, 2 * Mathf.PI);

            // 각도를 리스트 크기만큼 등분하여 각 점의 위치를 계산
            for (int i = 0; i < objectList.Count; i++)
            {
                if (objectList.Count < 2) break;

                RectTransform objRect = objectList[i].GetComponent<RectTransform>();

                // 짝수(count % 2 == 0) 인덱스
                if (i % 2 == 0)
                    objRect.anchoredPosition = GetEllPos(center, a1, b1, angle * i + randomAngleOffset);

                // 홀수(count % 2 == 1) 인덱스
                else
                    objRect.anchoredPosition = GetEllPos(center, a2, b2, angle * i + randomAngleOffset);
            }
        }

        // GameObject 리스트를 받아서 원하는 간격으로 다시 균일하게 뿌리는 메서드
        /*public static void ReArrObjects(float between, List<GameObject> objectList)
        {
            foreach (GameObject obj in objectList)
            {
                if (objectList.Count < 2) break;

                GameObject closestObj = MinDisObject(obj, objectList);
                RectTransform objRect = obj.GetComponent<RectTransform>();
                RectTransform closestRect = closestObj.GetComponent<RectTransform>();

                objRect.anchoredPosition = RanVec2(300, 125);

                // 두 RectTransform 사이의 거리 계산
                float distance = Vector2.Distance(objRect.anchoredPosition, closestRect.anchoredPosition);

                // 최소 간격보다 가까우면 위치 재조정
                while (between > distance)
                {
                    closestObj = MinDisObject(obj, objectList);
                    objRect.anchoredPosition = RanVec2(300, 125);
                    distance = Vector2.Distance(objRect.anchoredPosition, closestRect.anchoredPosition);
                }
            }
        }*/

        // GameObject 리스트를 받아서 원하는 간격으로 다시 균일하게 뿌리는 메서드 오버로드
        public static void ReArrObjects(float between, GameObject thisObject, List<GameObject> objectList)
        {
            RectTransform thisRect = thisObject.GetComponent<RectTransform>();
            thisRect.anchoredPosition = RanVec2(300, 125);

            foreach (GameObject obj in objectList)
            {
                if (objectList.Count < 2) break;

                GameObject closestObj = MinDisObject(thisObject, objectList);
                RectTransform closestRect = closestObj.GetComponent<RectTransform>();

                // 두 RectTransform 사이의 거리 계산
                float distance = Vector2.Distance(thisRect.anchoredPosition, closestRect.anchoredPosition);

                // 최소 간격보다 가까우면 위치 재조정
                while (between > distance)
                {
                    closestObj = MinDisObject(obj, objectList);
                    thisRect.anchoredPosition = RanVec2(300, 125);
                    distance = Vector2.Distance(thisRect.anchoredPosition, closestRect.anchoredPosition);
                }
            }
        }
        #endregion

        #region Dummy
        /*
        public static void Revolution(float minX, float maxX, float minY, float maxY, Transform transform, Transform pivot, List<GameObject> existingSlots, float minAngle)
        {
            bool isValidPosition = false;

            while (!isValidPosition)
            {
                // 1. 임의의 회전 각도를 구함
                float rotationX = Random.Range(minX, maxX);
                float rotationY = Random.Range(minY, maxY);
                Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

                // 2. 플레이어 정면을 기준으로 회전 방향을 계산
                Vector3 direction = rotation * pivot.forward * 3f;  // 기준이 되는 플레이어의 forward 방향 * 3

                // 3. 회전된 방향으로 위치를 설정할 준비
                Vector3 newPosition = pivot.position + direction;

                // 4. 다른 슬롯들과 최소 각도를 비교
                isValidPosition = true;  // 일단 true로 설정하고 시작
                foreach (GameObject slot in existingSlots)
                {
                    Vector3 toSlot = slot.transform.position - pivot.position;
                    float angle = Vector3.Angle(direction, toSlot);

                    if (angle < minAngle)  // 설정한 최소 각도보다 작으면 무효
                    {
                        isValidPosition = false;
                        break;
                    }
                }

                // 만약 모든 슬롯과의 각도가 minAngle 이상이라면 위치 설정
                if (isValidPosition)
                {
                    transform.SetPositionAndRotation(newPosition, Quaternion.LookRotation(direction));
                }
            }
        }
         */

        /*public static void ReArrObjects(float between, Transform pivot, List<GameObject> objectList)
        {
            foreach (GameObject obj in objectList)
            {
                GameObject closestObj = MinDistance(obj, objectList);

                // 최소 각도만 계산 후 한 번에 적용
                float betweenAngle = Vector3.Angle(obj.transform.forward, closestObj.transform.forward);

                Revolution(-10f, 10f, -25f, 25f, obj.transform, pivot, objectList, between);

                // 조건 만족 시 회전 수행
                if (between > betweenAngle)
                {
                    Revolution(-10f, 10f, -25f, 25f, obj.transform, pivot, objectList, between);
                }
            }
        }*/

        /*public static void ReArrObjects(float between, Transform pivot, List<GameObject> objectList)
        {
            foreach (GameObject obj in objectList)
            {
                GameObject closestObj = MinDistance(obj, objectList);

                int iterations = 0;
                int maxIterations = 100;

                float betweenAngle = Vector3.Angle(obj.transform.forward, closestObj.transform.forward);

                Revolution(-10f, 10f, -25f, 25f, obj.transform, pivot, objectList, between);

                while (between > betweenAngle)
                {
                    if (iterations >= maxIterations) break;

                    Revolution(-10f, 10f, -25f, 25f, obj.transform, pivot, objectList, between);
                    betweenAngle = Vector3.Angle(obj.transform.forward, closestObj.transform.forward); // 각도 다시 계산

                    iterations++;
                }
            }
        }*/

        /*public static void ReArrObjects(float minDistance, List<GameObject> objectList)
        {
            foreach (GameObject obj in objectList)
            {
                RectTransform objRect = obj.GetComponent<RectTransform>();
                GameObject closestObj = MinDisObject(obj, objectList);

                if (closestObj != null)
                {
                    RectTransform closestRect = closestObj.GetComponent<RectTransform>();

                    // 두 RectTransform 사이의 거리 계산
                    float distance = Vector2.Distance(objRect.anchoredPosition, closestRect.anchoredPosition);

                    // 최소 간격보다 가까우면 위치 재조정
                    if (distance < minDistance)
                    {
                        // 새로운 위치를 계산하여 다시 배치
                        Vector2 direction = (objRect.anchoredPosition - closestRect.anchoredPosition).normalized;
                        objRect.anchoredPosition = closestRect.anchoredPosition + direction * minDistance;
                    }
                }
            }
        }*/
        #endregion
    }
}