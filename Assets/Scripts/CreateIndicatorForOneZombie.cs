using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class CreateIndicatorForOneZombie : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject indicatorPrefab;
    public GameObject holderIndicator;
    void Start()
    {
        GameObject prefabIndi =  Instantiate(indicatorPrefab, holderIndicator.transform);
        prefabIndi.SetActive(false);                                                                                                                                                                                                        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
