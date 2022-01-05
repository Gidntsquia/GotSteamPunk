using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class audioPlayer : MonoBehaviour
{
    public sound[] soundArr;
    public bool isPlayingSound = false;
    private GameObject captain;
    private Animator animeCap;

    void OnEnable()
    {
        /*
        foreach(sound s in soundArr)
        {
            setSoundValues(s);
        }
        */
        
    }


    void Awake()
    {
        captain = GameObject.Find("Captain");
        animeCap = captain.GetComponent<Animator>();
        foreach(sound s in soundArr)
        {
            setSoundValues(s);
            
        }
    }
    public void spawnCaptain(string name, string background){
        isPlayingSound = true;
        changeVolume("titleMusic",0.05f);
        sound s = Array.Find(soundArr, sound => sound.name == name);
        if (s == null) {
            Debug.Log("sound "+ name +" is not in the array"); 
            return; 
        }
        float clipLength = s.source.clip.length;
        s.source.enabled = true;
        
        animeCap.SetBool("talking",true);
        captain.transform.position = new Vector3(
            Camera.main.transform.position.x-5,
            Camera.main.transform.position.y,
            Camera.main.transform.position.z+11);
        
        StartCoroutine(voiceEnd(s, clipLength));

    }

    // This will play the inputted sound if no other sound is currently playing.
    public void spawnCaptain(sound s)
    {
        if(!isPlayingSound)
        {
            isPlayingSound = true;
            changeVolume("titleMusic",0.05f);
            float clipLength = s.source.clip.length;
            s.source.enabled = true;
            
            animeCap.SetBool("talking",true);
            captain.transform.position = new Vector3(
                Camera.main.transform.position.x-5,
                Camera.main.transform.position.y,
                Camera.main.transform.position.z+11);
            
            StartCoroutine(voiceEnd(s, clipLength));
        }
    }

    // This will play the inputted sound regardless of if a sound is currently playing.
    public void spawnCaptainInterrupt(sound s)
    {
        StopAllCoroutines();
        stopAllSoundsExceptMusic();
        isPlayingSound = true;
        changeVolume("titleMusic",0.05f);
        float clipLength = s.source.clip.length;
        s.source.enabled = true;
        
        animeCap.SetBool("talking",true);
        /*
        captain.transform.position = new Vector3(
            Camera.main.transform.position.x-5,
            Camera.main.transform.position.y,
            Camera.main.transform.position.z+11);
        */
        StartCoroutine(voiceEnd(s, clipLength));
    }

    // This will play the inputted sound regardless of if a sound is currently playing.
    public void spawnCaptainInterrupt(String name)
    {
        StopAllCoroutines();
        stopAllSoundsExceptMusic();
        isPlayingSound = true;
        changeVolume("titleMusic",0.05f);
        sound s = Array.Find(soundArr, sound => sound.name == name);
        if (s == null) {
            Debug.Log("sound "+ name +" is not in the array"); 
            return; 
        }
        float clipLength = s.source.clip.length;
        s.source.enabled = true;
        
        animeCap.SetBool("talking",true);
        /*
        captain.transform.position = new Vector3(
            Camera.main.transform.position.x-5,
            Camera.main.transform.position.y,
            Camera.main.transform.position.z+11);
        */
        StartCoroutine(voiceEnd(s, clipLength));
    }

    private IEnumerator voiceEnd(sound theSound,float length){
        yield return new WaitForSeconds(length);
        changeVolume("titleMusic",0.5f);
        animeCap.SetBool("talking",false);
        captain.transform.position= new Vector3(8,-33,-69);
        theSound.source.enabled = false;
        isPlayingSound = false;
    }

    public void PlaySound(string name)
    {
        sound s = Array.Find(soundArr, sound => sound.name == name);
        if (s == null) {
            Debug.Log("sound "+ name +" is not in the array"); 
            return; 
        }
        s.source.enabled = true;
    }

    public IEnumerator PlaySoundThenStop(sound sound)
    {
        sound.source.enabled = true;
        yield return new WaitForSeconds(sound.clip.length);
        stopSound(sound);
    }

    public IEnumerator PlaySoundThenStop(string name)
    {
        sound sound = Array.Find(soundArr, sound => sound.name == name);
        sound.source.enabled = true;
        yield return new WaitForSeconds(sound.clip.length);
        stopSound(sound);
    }

    public void PlayMusic()
    {
        // TODO Check which scene it is and play music accordingly
        PlaySound("titleMusic");
    }


    public void changeVolume(string name,float newVolume){
        sound s = Array.Find(soundArr, sound => sound.name == name);
        if (s == null) {
            Debug.Log("sound "+ name +" is not in the array"); 
            return; 
        }
        s.source.volume = newVolume;
    }
        public void lowerVolumeContinuous(string name)
    {
        sound s = Array.Find(soundArr, sound => sound.name == name);
        if (s == null) {
            Debug.Log("sound "+ name +" is not in the array"); 
            return; 
        }
        for(int i = 0; i< 20; i++){
            StartCoroutine(volumeDown(s.source,.4f));
        }
    }
    private IEnumerator volumeDown(AudioSource source,float delay){
        if(source.volume > 0.05f){source.volume -= .05f;}
        yield return new WaitForSeconds(delay);
    }


    public void stopSound(string name)
    {
        sound s = Array.Find(soundArr, sound => sound.name == name);
        if (s == null) {
            Debug.Log("sound "+ name +" is not in the array"); 
            return; 
        }
        s.source.enabled = false;
    }

    private void stopSound(sound s)
    {
        s.source.enabled = false;
    }

    public void stopAllSounds()
    {
        List<AudioSource> sounds = new List<AudioSource>(GetComponents<AudioSource>());
        foreach(AudioSource sound in sounds)
        {
            sound.enabled = false;
        }
    }

    public void stopAllSoundsExceptMusic()
    {
        // All sounds in the audioPlayer soundArr are sound effects or music.
        List<string> soundEffectNames = new List<string>();
        foreach(sound s in soundArr)
        {
            soundEffectNames.Add(s.clip.name);
        }

        List<AudioSource> sounds = new List<AudioSource>(GetComponents<AudioSource>());
        foreach(AudioSource sound in sounds)
        {
            
            // This will deactivate all audio clips except for the music one and other sound effects.
            if(!soundEffectNames.Contains(sound.clip.name))
            {
                sound.enabled = false;
            }
            
        }

        
    }

    public void setSoundValues(sound sound)
    {
        sound.source = gameObject.AddComponent<AudioSource>();
        sound.source.enabled = false;
        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
        sound.source.loop = sound.loop;
    }

}
