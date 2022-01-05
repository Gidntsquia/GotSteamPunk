using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TaskWrapper
{
    // This is used to allow us to put a list inside a list in the Unity UI.
    [SerializeField]
    public List<VoiceLineTypeWrapper> task;
}
