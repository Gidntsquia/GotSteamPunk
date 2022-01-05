using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SillyQuipsManager : MonoBehaviour
{
    public List<sound> sillyQuips;
    public audioPlayer audioPlayer;
    public bool shouldPlayQuips = false;
    public bool isPlayingQuip = false;
    [Range (0f,1f)]
    public float chanceToPlayQuip = 0.001f;
    public float secondsEnabled = 0f;
    
    void Awake()
    {
        foreach(sound sound in sillyQuips)
        {
            audioPlayer.setSoundValues(sound);
        }
        shouldPlayQuips = false;
        this.enabled = false;
    }

    // Fixed update plays once every 0.2 seconds.
    // Through trial an error, I've found that chanceToPlayQuip = 0.02 plays a quip about every second. 0.001 seems like the sweet spot.
    void FixedUpdate()
    {
        if(!isPlayingQuip && !audioPlayer.isPlayingSound)
        {
            // The quip manager will only track its time enabled if a quip isnt playing.
            secondsEnabled += Time.fixedDeltaTime;
            if(shouldPlayQuips)
            {
                float randomValue = Random.value;
                if(randomValue < chanceToPlayQuip)
                {
                    playRandomQuip();
                }
                
            }
        }
       
        
        
    }

   
    // This will play a random quip in the sound list. It will prevent itself from playing two quips at once.
    // TODO make it where these quips can be interrupted by other voice lines.
    public void playRandomQuip()
    {
        // This will find a random value from [0, length of silly quips)
        int randomIndex = Random.Range(0, sillyQuips.Count);
        audioPlayer.spawnCaptain(sillyQuips[randomIndex]);
        isPlayingQuip = true;
        StartCoroutine(trackWhenQuipEnds(sillyQuips[randomIndex].clip.length));
        secondsEnabled = 0f;
    }

    


    // This method sets "isPlayingQuip" to false after the clip is finished playing.
    private IEnumerator trackWhenQuipEnds(float clipLengthSeconds)
    {
        yield return new WaitForSeconds(clipLengthSeconds);
        isPlayingQuip = false;
        
    }
}
