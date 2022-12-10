using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Example : MonoBehaviour
{
    private PlayableDirector _director;

    private void Awake()
    {
        _director = GetComponent<PlayableDirector>();
        _director.Freeze();
        
    }

    void FixedUpdate()
    {
        if(Input.GetMouseButton(0))
        {
            _director.Unfreeze();
        }else
            _director.Freeze();
    }
}
