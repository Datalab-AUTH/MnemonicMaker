using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSwitch : MonoBehaviour
{

    [SerializeField]
    CameraConnection[] cameraConnections;

    [System.Serializable]
    public class CameraConnection
    {
        [SerializeField]
        public CinemachineVirtualCamera camera;
        [SerializeField]
        public int nodeID;
    }

    public void triggerCameraSwitch(int nodeID)
    {
        CinemachineVirtualCamera oldCamera = GameObject.Find("ScriptLoader").GetComponent<Globals>().activeCV;
        foreach (CameraConnection cc in cameraConnections)
        {
            if (cc.nodeID == nodeID)
            {
                cc.camera.Priority = 99;
                oldCamera.Priority = 1;
                GameObject.Find("ScriptLoader").GetComponent<Globals>().activeCV = cc.camera;
            }
        }
    }
}
