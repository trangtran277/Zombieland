using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayZombie : MonoBehaviour
{
    // Start is called before the first frame update
    public static ArrayZombie instance;
    public float zombiesLength = 100;
    public void Compare(float n)
    {
        zombiesLength = zombiesLength > n ? n : zombiesLength;
    }
    public bool CheckDitection()
    {
        if (zombiesLength <= 10)
            return true;
        return false;
    }
    public bool CheckChase()
    {
        if (zombiesLength <= 10)
            return true;
        return false;
    }
}
