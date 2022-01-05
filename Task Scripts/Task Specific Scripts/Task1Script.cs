using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1Script : MonoBehaviour
{
    // NOTE: Make sure the tasks are enabled in the scene editor, or else this script will not play correctly.
    // TODO add voicelines
    public GameObject filledFloorAsset;
    public GameObject holeInFloorAsset;

    void OnEnable()
    {
        enableHoleInFloor();
    }


    void OnDisable()
    {
        enableFilledFloor();
    }

    // This enables the filled floor asset and disables the hole in the floor asset since only one can be active at a time.
    private void enableFilledFloor()
    {
        filledFloorAsset.SetActive(true);
        holeInFloorAsset.SetActive(false);
    }


    // This enables the hole in the floor asset and disables the filled floor asset since only one can be active at a time.
    private void enableHoleInFloor()
    {
        filledFloorAsset.SetActive(false);
        holeInFloorAsset.SetActive(true);

    } 
}
