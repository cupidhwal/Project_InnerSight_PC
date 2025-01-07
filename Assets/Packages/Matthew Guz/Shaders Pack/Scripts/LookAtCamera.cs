using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatthewAssets
{


    public class LookAtCamera : MonoBehaviour
    {
        Camera _cam;
        void Start()
        {
            _cam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            transform.forward = _cam.transform.position - transform.position;
        }
    }
}