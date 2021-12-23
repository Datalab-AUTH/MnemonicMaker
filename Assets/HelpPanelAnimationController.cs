using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPanelAnimationController : MonoBehaviour
{
    private Animator animator;
    private bool visible;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && !visible)
        {
            animator.Play("HelpPanelShow");
            visible = true;
        }
        else if (Input.GetKeyDown(KeyCode.H) && visible)
        {
            animator.Play("HelpPanelHide");
            visible = false;
        }
    }
}
