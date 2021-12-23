using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineFader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 0.5f;

    public void fadeCamera()
    {
        transition.SetTrigger("StartFade");
    }

    public void endFadeCamera()
    {
        transition.SetTrigger("EndFade");
    }

}
