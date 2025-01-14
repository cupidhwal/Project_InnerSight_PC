using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameUI_KeyUI : MonoBehaviour
{
    public List<Transform> buttons = new List<Transform>();

    private Button defaultBtn;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            buttons.Add(transform.GetChild(i));
        }

        index = 0;

        defaultBtn = buttons[0].GetChild(0).GetComponent<Button>();

        EventSystem.current.SetSelectedGameObject(defaultBtn.gameObject);
    }

    private void OnEnable()
    {
        //index = 0;

        //defaultBtn = buttons[0].GetChild(0).GetComponent<Button>();

        //EventSystem.current.SetSelectedGameObject(defaultBtn.gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        SelectBtn();
    }

    void SelectBtn()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            index++;

            Debug.Log(index);
            if (index >= buttons.Count)
            {
                index = 0;
                Debug.Log("as" + index);
                EventSystem.current.SetSelectedGameObject(buttons[index].GetChild(0).gameObject);
            }
            else 
            {
  
                EventSystem.current.SetSelectedGameObject(buttons[index].GetChild(0).gameObject);
            }

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            index--;

            Debug.Log(index);

            if (index < 0)
            {           
                index = buttons.Count - 1;


                EventSystem.current.SetSelectedGameObject(buttons[index].GetChild(0).gameObject);
            }
            else
            {

                EventSystem.current.SetSelectedGameObject(buttons[index].GetChild(0).gameObject);
            }
        }
    }
}
