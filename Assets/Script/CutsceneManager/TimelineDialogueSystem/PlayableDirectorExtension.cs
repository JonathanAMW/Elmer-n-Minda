using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public static class PlayableDirectorExtension
{
    public static void Freeze(this PlayableDirector director)
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    public static void Unfreeze(this PlayableDirector director)
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }
}