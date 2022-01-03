using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public bool minimapPanelActive;
    public Camera MinimapCamera;
    public Animator animator;

    private void Start()
    {
        minimapPanelActive = false;
    }

    public void minimapPopup()
    {
        if (minimapPanelActive)
        {
            animator.Play("MinimapHide");
            minimapPanelActive = false;
        }
        else if (!minimapPanelActive)
        {
            MinimapCamera.transform.gameObject.SetActive(true);
            gameObject.SetActive(true);
            animator.Play("MinimapShow");
            minimapPanelActive = true;
        }
    }
}
