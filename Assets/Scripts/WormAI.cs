using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormAI : MonoBehaviour
{
    public GameObject entity;
    public GameObject worm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setParent()
    {
        //Debug.Log("Animation end");
        Vector3 vecWormNew = new Vector3();
        vecWormNew.y = (float)-0.1 + worm.transform.position.y;
        vecWormNew.z = 0;
        vecWormNew.x = (float)-0.14 + worm.transform.position.x;
        worm.transform.position = vecWormNew;
        /*
        if (delayedStop)
        {
            delayedStop = false;
            animator.SetBool("TriggerJump", false);
            animator.SetBool("TriggerJump2", false);
        }*/
    }

    public void setParent2()
    {
        //Debug.Log("Animation end");
        Vector3 vecWormNew = new Vector3();
        vecWormNew.y = (float)-0.1 + worm.transform.position.y;
        vecWormNew.z = 0;
        vecWormNew.x = (float)0.14 + worm.transform.position.x;
        worm.transform.position = vecWormNew;
        /*
        if (delayedStop)
        {
            delayedStop = false;
            animator.SetBool("TriggerJump", false);
            animator.SetBool("TriggerJump2", false);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
