using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public class ReadOnlyAttribute : PropertyAttribute
    {

    }

    public Gate entryNode;
    public Gate exitNode;

    private ChunkController controller;

    [System.Serializable]
    public class Gate
    {
        
        [ReadOnly] public Transform doormat;
        [SerializeField]
        public GameObject gate;

        [ReadOnly] public string world;
        [ReadOnly] public string dungeon;
        [ReadOnly] public int chunkID ;
        [ReadOnly] public int nodeID;
        [ReadOnly] public int gateID;

    }

    private void Start()
    {
        if (entryNode.gate.GetComponent<WarpLocation>().gateID == -1)
        {
            entryNode.gate.GetComponent<WarpLocation>().gateID = GameObject.Find("ScriptLoader").GetComponent<Globals>().bip39.portalCounter++;
            entryNode.gate.GetComponent<WarpLocation>().portal = gameObject.GetComponent<Portal>();
            entryNode.gateID = entryNode.gate.GetComponent<WarpLocation>().gateID;
        }
        if (exitNode.gate.GetComponent<WarpLocation>().gateID == -1)
        {
            exitNode.gate.GetComponent<WarpLocation>().gateID = GameObject.Find("ScriptLoader").GetComponent<Globals>().bip39.portalCounter++;
            exitNode.gate.GetComponent<WarpLocation>().portal = gameObject.GetComponent<Portal>();
            exitNode.gateID = exitNode.gate.GetComponent<WarpLocation>().gateID;
        }

        entryNode.doormat = entryNode.gate.GetComponent<WarpLocation>().doormat;
        exitNode.doormat = exitNode.gate.GetComponent<WarpLocation>().doormat;

        controller = GameObject.Find("ScriptLoader").GetComponent<ChunkController>();
        controller.locateGate(entryNode);
        controller.locateGate(exitNode);
    }

    void OnDrawGizmosSelected()
    {
        if (entryNode.gate != null && exitNode.gate != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.cyan;
            
            Gizmos.DrawLine(entryNode.gate.transform.position, exitNode.gate.transform.position);
        }
    }

    public void warp(int gateID)
    {
        CinemachineFader cvFader = GameObject.Find("ScriptLoader").GetComponent<CinemachineFader>();
        cvFader.fadeCamera();
        if (entryNode.gateID != gateID)
        {
            //entry node
            StartCoroutine(DelayAction(1f, entryNode));
        }
        else
        {
            //exit node
            StartCoroutine(DelayAction(1f, exitNode));
        }
    }

    IEnumerator DelayAction(float delayTime, Gate exit)
    {

        ChunkController controller = GameObject.Find("ScriptLoader").GetComponent<ChunkController>();
        AudioLibrary audioController = GameObject.Find("Audio").GetComponent<AudioLibrary>();
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        
        //Do the action after the delay time has finished.

        //Play area sound 
        audioController.changeSound(exit.nodeID);
        //Warp play position
        GameObject.Find("Character").transform.position = exit.doormat.position;
        controller.warp(exit.world, exit.dungeon);
        GameObject.Find("Character").GetComponent<SpriteController>().nodeID = exit.nodeID;
        //Change camera 
        GameObject.Find("ScriptLoader").GetComponent<CinemachineSwitch>().triggerCameraSwitch(exit.nodeID);
        if (!GameObject.Find("ScriptLoader").GetComponent<Globals>().bip39.normal)
            GameObject.Find("ScriptLoader").GetComponent<Globals>().pathfinder.GetComponent<Pathfinding>().pathfind();

        GameObject.Find("ScriptLoader").GetComponent<EffectManager>().changeEffects();
    }

}
