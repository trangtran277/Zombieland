using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private List<RectTransform> active = new List<RectTransform>();
    public RectTransform[] children;
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        //children = this.GetComponentsInChildren<RectTransform>();
        index = 0;
        for(int i = 0; i < 2; i++)
        {
            //children[i].gameObject.SetActive(true);
            active.Add(children[i]);
            index++;
        }
    }

    public void Next()
    {
        if(index >= children.Length - 1)
        {
            Skip();
        }
        else
        {
            foreach (RectTransform item in active)
            {
                Debug.Log("setting item to non-active" + this.name);
                item.gameObject.SetActive(false);
            }
            index++;
            children[index].gameObject.SetActive(true);
            active.Add(children[index]);
        }
    }

    public void Skip()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
