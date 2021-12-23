using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider _slider;
    private MainMenu script;
    // Start is called before the first frame update
    void Start()
    {
        script = GameObject.Find("ScriptLoader").GetComponent<Globals>().mainMenuScript;
        _slider.onValueChanged.AddListener((v) =>
        {
            if(_slider.name == "MusicSlider")
                script.setMusicVolume(v);
            else if (_slider.name == "AmbientSlider")
                script.setAmbientVolume(v);
            else if (_slider.name == "SFXSlider")
                script.setSFXVolume(v);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
