using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestory : MonoBehaviour
{
    void Awake()
    {
       GameObject[] objs = GameObject.FindGameObjectsWithTag("DataTantangan");

       if (objs.Length > 1)
       {
           Destroy(this.gameObject);
       }

       DontDestroyOnLoad(this.gameObject);
    }
}
