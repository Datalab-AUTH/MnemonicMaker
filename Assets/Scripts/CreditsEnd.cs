using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsEnd : MonoBehaviour
{
    public void end()
    {
        GameObject.Find("ScriptLoader").GetComponent<Globals>().mainMenuScript.stopTheCredits();
    }
}
