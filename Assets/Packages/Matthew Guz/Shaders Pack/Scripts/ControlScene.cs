using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MatthewAssets
{


    public class ControlScene : MonoBehaviour
    {
        public Text exampleName;

        private GameObject shadersExamples; // Reference to Shaders_Examples object
        private List<Example> examples = new List<Example>(); // List of found examples
        private int currentIndex = -1;

        private void Start()
        {
            // Find the Shaders_Examples object in the scene
            shadersExamples = GameObject.Find("Shaders_Examples");

            if (shadersExamples != null)
            {
                // Iterate through all children of Shaders_Examples and add them as examples
                foreach (Transform child in shadersExamples.transform)
                {
                    Example example = new Example
                    {
                        objects = new GameObject[] { child.gameObject },
                        name = child.name
                    };
                    examples.Add(example);
                }
            }
            else
            {
                Debug.LogError("Shaders_Examples object not found in the scene.");
                return;
            }

            // Ensure there are examples before calling Next
            if (examples.Count > 0)
                Next();
            else
                Debug.LogError("No examples found under Shaders_Examples.");
        }

        public void Update()
        {
            if (examples.Count == 0) return; // Ensure there are examples before proceeding

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Previous();
            if (Input.GetKeyDown(KeyCode.RightArrow))
                Next();
        }

        public void Next()
        {
            if (currentIndex + 1 >= examples.Count)
                return;

            if (currentIndex != -1)
                ToggleExampleObjects(currentIndex, false);

            currentIndex++;
            ToggleExampleObjects(currentIndex, true);

            // Automatically set the name of the active example to the exampleName text
            if (exampleName != null)
                exampleName.text = examples[currentIndex].objects[0].name;
            else
                Debug.LogError("exampleName Text component is not assigned in the Inspector.");
        }

        public void Previous()
        {
            if (currentIndex - 1 < 0)
                return;

            ToggleExampleObjects(currentIndex, false);
            currentIndex--;
            ToggleExampleObjects(currentIndex, true);

            // Automatically set the name of the active example to the exampleName text
            if (exampleName != null)
                exampleName.text = examples[currentIndex].objects[0].name;
            else
                Debug.LogError("exampleName Text component is not assigned in the Inspector.");
        }

        private void ToggleExampleObjects(int index, bool isActive)
        {
            if (index < 0 || index >= examples.Count) return; // Check that index is valid

            foreach (var obj in examples[index].objects)
            {
                if (obj != null)
                    obj.SetActive(isActive);
            }
        }
    }

    [System.Serializable]
    public class Example
    {
        public GameObject[] objects;
        public string name;
    }
}
