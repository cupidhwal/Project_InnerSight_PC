using UnityEngine;

namespace VFXTools
{
	public class BulletController : MonoBehaviour
	{
		public Transform rotationCenter;   
		public float rotationSpeed = 100f; 
		public float movementSpeed = 10f;  

		private void Update()
		{
			Vector3 directionToCenter = rotationCenter.position - transform.position;
			Quaternion targetRotation = Quaternion.LookRotation(directionToCenter);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
			transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
		}
	}
}