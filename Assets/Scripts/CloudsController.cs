using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsController : MonoBehaviour
{

    public float Speed = 1;
    public GameObject sprite;
    public List<Sprite> images = new List<Sprite>();
    public int RandomSize = 10;
    public int RandomHit = 1;

    private float heightCamera;
    private float widthCamera;
    
    private Camera cam;
    private bool moving;

    private void Awake()
    {
        cam = Camera.main;
        heightCamera = 2f * cam.orthographicSize;
        widthCamera = heightCamera * cam.aspect;
        moving = false;
        resetPos();
    }

    private void Start()
    {
        cam = Camera.main;
        heightCamera = 2f * cam.orthographicSize;
        widthCamera = heightCamera * cam.aspect;
        moving = false;
        resetPos();
    }

    void Update()
    {
        var spriteToMove = sprite;
        if (!moving)
        {
            int rand = UnityEngine.Random.Range(0, RandomSize);
            if (rand >= 0 && rand <= RandomHit)
            {
                heightCamera = 2f * cam.orthographicSize;
                widthCamera = heightCamera * cam.aspect;
                spriteToMove.GetComponent<SpriteRenderer>().sprite = images[UnityEngine.Random.Range(0, images.Count)];
                float randY = UnityEngine.Random.Range(0 - spriteToMove.GetComponent<SpriteRenderer>().size.y / 2, heightCamera + spriteToMove.GetComponent<SpriteRenderer>().size.y / 2);
                float X = 3f + widthCamera/2;
                StartCoroutine(MoveOverSpeed(spriteToMove, new Vector3(X, randY, 1), Speed));
                moving = true;
            }
        }
    }

    public IEnumerator MoveOverSpeed(GameObject objectToMove, Vector3 end, float speed)
    {
        // speed should be 1 unit per second
        //objectToMove.transform.localPosition != end
        while (objectToMove.transform.localPosition.x <= widthCamera/2 + 2.5f)
        {
            //Debug.Log("While enum Call");
            objectToMove.transform.localPosition = Vector3.MoveTowards(objectToMove.transform.localPosition, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        moving = false;
        resetPos();
    }
    
    private void resetPos()
    {
        var spriteToMove = sprite;
        heightCamera = 2f * cam.orthographicSize;
        widthCamera = heightCamera * cam.aspect;
        float randY = UnityEngine.Random.Range(-1.5f,1.5f);
        float X = 0 - (widthCamera / 2 + 3f);
        spriteToMove.transform.localPosition = new Vector3(X, randY, 1);
    }
}

