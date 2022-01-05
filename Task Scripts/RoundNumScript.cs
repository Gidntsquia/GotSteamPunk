using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundNumScript : MonoBehaviour
{
    public int roundNum = 0;
    public TaskGroupManager taskGroupManagerScript;
    public Text roundNumText;

    void OnEnable()
    {
        roundNum = taskGroupManagerScript.roundAdvancement + 1;
    }

    public void updateRoundNum()
    {
        roundNum = taskGroupManagerScript.roundAdvancement + 1;
        roundNumText.text = "Round: " + roundNum.ToString();

    }

    void Update()
    {
        if(Input.anyKey)
        {
            updateRoundNum();
        }
    }
}
