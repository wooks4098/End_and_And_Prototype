using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureSceneUI : MonoBehaviour
{
    [SerializeField] CreaturePlayer playerA;
    [SerializeField] CreaturePlayer playerB;

    public Text scoreTextA;
    public Text scoreTextB;

    public Button buttonA;
    public Button buttonB;


    public void UpScoreA()
    {
        playerA.score++;

        scoreTextA.text = "Score: " + playerA.score.ToString();
    }

    public void UpScoreB()
    {
        playerB.score++;

        scoreTextB.text = "Score: " + playerB.score.ToString();
    }
}
