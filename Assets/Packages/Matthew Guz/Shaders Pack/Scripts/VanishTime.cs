using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatthewAssets
{


    [RequireComponent(typeof(MeshRenderer))]
    public class DiffusseTime : MonoBehaviour
    {
        private MeshRenderer meshRenderer;

        public float speed = .5f;

        private void Start()
        {
            meshRenderer = this.GetComponent<MeshRenderer>();
        }

        private float t = 0.0f;
        private void Update()
        {
            Material[] mats = meshRenderer.materials;

            mats[0].SetFloat("_Cutoff", Mathf.Sin(t * speed));
            t += Time.deltaTime;

            meshRenderer.materials = mats;
        }
    }
}
