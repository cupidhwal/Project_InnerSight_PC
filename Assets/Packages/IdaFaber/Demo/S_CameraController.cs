using UnityEngine;

namespace CameraControlSystem
{
    public class S_CameraController : MonoBehaviour
    {
        public GameObject character;
        public Transform cameraHolder;
        public float zoomSpeed = 0.5f;
        public float rotationSpeed = 3f;
        public float smoothTime = 0.3F;
        private float currentZoom = 0f;
        private float zoomMin = 0.5f;
        private float zoomMax = 3f;
        private Vector3 zoomVelocity = Vector3.zero;
        private Vector3 positionVelocity = Vector3.zero;

        private Vector3 initialRelativePosition;
        private float lastRotateHorizontal;

        private void Start()
        {
            Cursor.visible = true;
            currentZoom = cameraHolder.localPosition.z;
        }

        private void Update()
        {
            HandleZoom();
            HandleRotation();
            HandleCursorVisibility();
        }

        void HandleZoom()
        {
            float scroll = -Input.GetAxis("Mouse ScrollWheel");
            currentZoom += scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, zoomMin, zoomMax);

            float targetZ = Mathf.SmoothDamp(cameraHolder.localPosition.z, currentZoom, ref zoomVelocity.z, smoothTime);
            float zoomFactorInverse = 1 - ((currentZoom - zoomMin) / (zoomMax - zoomMin));
            float headFocusY = 1.63f;
            float defaultCameraY = 1.128f;
            float targetY = Mathf.Lerp(defaultCameraY, headFocusY, zoomFactorInverse);

            targetY = Mathf.SmoothDamp(cameraHolder.localPosition.y, targetY, ref positionVelocity.y, smoothTime);

            cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, targetY, targetZ);
        }

        void HandleRotation()
        {
            if (Input.GetMouseButton(0))
            {
                lastRotateHorizontal = -Input.GetAxis("Mouse X") * rotationSpeed;
                Quaternion rotation = Quaternion.Euler(0f, lastRotateHorizontal, 0f);
                character.transform.rotation *= rotation;
            }
            else if (!Mathf.Approximately(lastRotateHorizontal, 0))
            {
                Quaternion rotation = Quaternion.Euler(0f, lastRotateHorizontal, 0f);
                character.transform.rotation *= rotation;
                lastRotateHorizontal = Mathf.Lerp(lastRotateHorizontal, 0, Time.deltaTime * rotationSpeed * 3f);
            }
        }

        void HandleCursorVisibility()
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
