using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIndicator : MonoBehaviour
{
    [Range(5,30)]
    [SerializeField] float destroyTimer = 20.0f;
    // Start is called before the first frame update
    private void Start()
    {
        Invoke("Register", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Register()
    {
        /*if (IndicatorSys.CheckIfObjectInSight(this.transform))
        {
            IndicatorSys.CreateIndicator(this.transform);
        }*/
        //Destroy(this.transform.gameObject, destroyTimer);
    }
}
