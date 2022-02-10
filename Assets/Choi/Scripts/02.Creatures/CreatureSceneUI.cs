using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureSceneUI : MonoBehaviour
{
    [SerializeField] CreatureAggro aggro;

    public Text scoreTextA;
    public Text scoreTextB;

    public Button buttonA;
    public Button buttonB;


    public void UpScoreA()
    {
        aggro.Player1Point += 1;

        scoreTextA.text = "Score: " + aggro.Player1Point;
    }

    public void UpScoreB()
    {
        aggro.Player2Point += 1;

        scoreTextB.text = "Score: " + aggro.Player2Point;
    }
}
