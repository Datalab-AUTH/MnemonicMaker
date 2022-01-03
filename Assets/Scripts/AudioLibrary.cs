using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLibrary : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioLibrary;
    [SerializeField]
    private AudioClip[] ambientAudioLibrary;
    [SerializeField]
    private AudioClip[] sfxLibrary;

    private Dictionary<string, int> audioDict;
    private Dictionary<string, int> ambientAudioDict;
    private Dictionary<string, int> sfxDict;

    private AudioSource sourceMusic;
    private AudioSource sourceSFX;
    private AudioSource sourceAmbient;

    private string defaultSong = "Overworld";

    private void Start()
    {
        audioDict = new Dictionary<string, int>();
        ambientAudioDict = new Dictionary<string, int>();
        sfxDict = new Dictionary<string, int>();

        audioDict.Add("Overworld", 0);
        audioDict.Add("Dungeon", 1);

        ambientAudioDict.Add("Dolphin", 0);

        sfxDict.Add("Footsteps", 0);

        sourceSFX = GameObject.Find("SoundEffects").GetComponent<AudioSource>();
        sourceMusic = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        sourceAmbient = GameObject.Find("AmbientSound").GetComponent<AudioSource>();

        defaultSong = "Overworld";
    }

    private int getAudio(string audioKey)
    {
        int tempAudioIndex;
        //WEBGL adds an invisible-unprintable character at the end of the requested string
        //So trimming the end fixed this error.
        if (!audioDict.TryGetValue(audioKey.Trim(), out tempAudioIndex))
        {
            Debug.LogError("AUDIO LOOKUP: The Provided audio: `" + audioKey.Trim() + "` does not exist!");
            return 0; //default overworld sound
        }
        return tempAudioIndex;
    }

    private int getAmbientAudio(string audioKey)
    {
        int tempAudioIndex;
        //WEBGL adds an invisible-unprintable character at the end of the requested string
        //So trimming the end fixed this error.
        if (!ambientAudioDict.TryGetValue(audioKey.Trim(), out tempAudioIndex))
        {
            Debug.LogError("AUDIO AMBIENT LOOKUP: The Provided audio: `" + audioKey.Trim() + "` does not exist!");
            return 0; //default overworld sound
        }
        return tempAudioIndex;
    }

    private int getSFX(string audioKey)
    {
        int tempAudioIndex;
        //WEBGL adds an invisible-unprintable character at the end of the requested string
        //So trimming the end fixed this error.
        if (!sfxDict.TryGetValue(audioKey.Trim(), out tempAudioIndex))
        {
            Debug.LogError("SFX LOOKUP: The Provided audio: `" + audioKey.Trim() + "` does not exist!");
            return 0; //default overworld sound
        }
        return tempAudioIndex;
    }

    public void changeSound(int nodeID)
    {
        string song = "";
        if(nodeID == 0 || nodeID == 1 || (nodeID >= 7 && nodeID <= 17) ||
            nodeID == 29 || nodeID == 30 || (nodeID >= 36 && nodeID <= 46))
        {
            song = "Overworld";
        }
        else
        {
            song = "Dungeon";
        }
        if (!defaultSong.Equals(song))
        {
            sourceMusic.clip = audioLibrary[getAudio(song)];
            sourceMusic.Play();
            defaultSong = song;
        }
        
    }

    public void playAmbientSound(string keyword)
    {
        sourceAmbient.clip = ambientAudioLibrary[getAmbientAudio(keyword)];
        sourceAmbient.Play();
    }

    public void stopAmbientSound()
    {
        sourceAmbient.Stop();
    }

    public void playSFXSound(string keyword)
    {
        sourceSFX.clip = sfxLibrary[getSFX(keyword)];
        sourceSFX.Play();
    }

    public void playFootstep()
    {
        //sourceFootsteps.clip = audioLibrary[getAudio("Footsteps")];
        sourceSFX.PlayOneShot(sfxLibrary[getSFX("Footsteps")]);
    }

    public void stopFootstep()
    {
        sourceSFX.Stop();
    }

    public void changeVolume(int audioSourceID, float volume)
    {
        if(audioSourceID == 0)
        {
            sourceMusic.volume = volume;
        }else if(audioSourceID ==1)
        {
            sourceAmbient.volume = volume;
        }
        else if(audioSourceID == 2)
        {
            sourceSFX.volume = volume;
        }
    }
}
