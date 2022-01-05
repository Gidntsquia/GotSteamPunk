using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ClumpWrapper
{
    // This is used to allow us to put a list inside a list in the Unity UI.
    [SerializeField]
    public List<int> clump;
}