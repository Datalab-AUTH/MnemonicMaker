using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChestHandler : MonoBehaviour
{

    public void setImage(Sprite sprite)
    {
        transform.Find("RawImage").GetComponent<RawImage>().texture = sprite.texture;
    }

}
