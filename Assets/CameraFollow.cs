using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTransform;
    public float smoothMove = 0.15f;
    public Camera camera;
    Vector3 zero = Vector3.zero;
    // Clamp the target position to be within the boundaries
    float cameraVertExtent;
    float cameraHorizExtent;
    private Globals script;

    void Start()
    {
        script = GameObject.Find("ScriptLoader").GetComponent<Globals>();
        cameraVertExtent = camera.orthographicSize;
        cameraHorizExtent = camera.orthographicSize * camera.aspect;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 characterPos = followTransform.position;
        characterPos.z = transform.position.z;


        //clamp
        Vector3 clampMovement = new Vector3();
        (int,int) offsets = script.chunkController.getCurrentWorldBounds();

        clampMovement.y = Mathf.Clamp(characterPos.y, 0f + cameraVertExtent + offsets.Item2, 81f + offsets.Item2 - cameraVertExtent);
        clampMovement.x = Mathf.Clamp(characterPos.x, 0f + cameraHorizExtent + offsets.Item1, 196f + offsets.Item1 - cameraHorizExtent);
        clampMovement.z = transform.position.z;


        //transform.position = new Vector3(followTransform.position.x, followTransform.position.y, this.transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, clampMovement, ref zero, smoothMove);
    }
}
