using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentAudioManager : MonoBehaviour
{

    public AudioClipList AudioListRef;

    private AudioSource myAudioSource;
    public List<AudioClip> EquipmentSounds { get; private set; }

    public bool CanTransitionToLoop { get; set; }

    private int currentAudioIndex = -1;

    private bool isInTimer = false;

    private EquipmentAction currentAction=null;

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        if (!AudioListRef) { return; }
        EquipmentSounds = AudioListRef.VaccumSounds;
    }

    public void PlayActiveSound(EquipmentAction action)
    {

        if (!currentAction || action != currentAction) 
        {
            currentAction = action;
            CanTransitionToLoop = false;
        }

        if (CanTransitionToLoop == true)
        {
            PlayActiveSound(action.LoopSoundIndex, true, false);
        }
        else
        {
            PlayActiveSound(action.StartupSoundIndex, false, true);
        }
    }

    public void PlayDeactiveSound(EquipmentAction action)
    {
        CanTransitionToLoop = false;
        PlayActiveSound(action.EndSoundIndex, false, false);
    }

    private void PlayActiveSound(int audioIndex, bool looping, bool shouldTransition)
    {
        if (!myAudioSource) { return; }
        if (!AudioListRef) { return; }

        var playing = myAudioSource.isPlaying;
        if (playing && audioIndex == currentAudioIndex) { return; }

        if (EquipmentSounds.Count <= 0) { return; }
        if (audioIndex < 0 || audioIndex > EquipmentSounds.Count)
        {
            CanTransitionToLoop = true;
            StopAllCoroutines();
            myAudioSource.Stop();
            return;
        }

        var pickedSound = EquipmentSounds[audioIndex];
        if (!pickedSound) { return; }

        if (playing && myAudioSource.loop) { myAudioSource.Stop(); }

        currentAudioIndex = audioIndex;
        myAudioSource.loop = looping;
        myAudioSource.clip = pickedSound;
        myAudioSource.Play();

        if (shouldTransition)
        {
            if (isInTimer) 
            { 
                StopAllCoroutines();
                CanTransitionToLoop = false;
            }
            StartCoroutine(TimeTilTransition(pickedSound.length));
        }
    }

    private IEnumerator TimeTilTransition(float seconds)
    {
        isInTimer = true;
        yield return new WaitForSeconds(seconds / 3f);
        CanTransitionToLoop = true;
        isInTimer = false;
    }
}
