using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task5Script : MonoBehaviour
{
    // NOTE: Make sure the tasks are enabled in the scene editor, or else this script will not play correctly.
    // TODO add voicelines
    public GameObject steamObstacles;
    void OnEnable()
    {
        steamObstacles.SetActive(true);
    }


    void OnDisable()
    {
        steamObstacles.SetActive(false);
    }
}
