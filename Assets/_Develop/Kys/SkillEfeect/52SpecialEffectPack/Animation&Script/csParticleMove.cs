using UnityEngine;
using System.Collections;
using Noah;

public class csParticleMove : MonoBehaviour
{
    public float speed = 0.1f;

	void Update () 
    {
        transform.Translate(Vector3.forward * speed);
	}
}
