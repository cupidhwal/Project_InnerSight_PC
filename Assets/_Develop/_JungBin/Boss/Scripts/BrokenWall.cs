using UnityEngine;

namespace JungBin
{

    public class BrokenWall : MonoBehaviour
    {
        public GameObject brokenObj;
        public GameObject UnBrokenObj;

        public void RushToWall()
        {

            brokenObj.SetActive(true);
            UnBrokenObj.SetActive(false);

            Invoke("Destroy", 3f);
        }

        public void Destroy()
        {
            Destroy(brokenObj);
        }
    }
}