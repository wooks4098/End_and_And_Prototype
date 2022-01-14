using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureHPTestUI : MonoBehaviour
{
    [SerializeField] CreaturePlayer goPlayer1;
    [SerializeField] CreaturePlayer goPlayer2;

    [SerializeField] Slider player1;
    [SerializeField] Slider player2;

    public void Update()
    {
        player1.value = goPlayer1.CalculatePlayerHP() / 100f;
        player2.value = goPlayer2.CalculatePlayerHP() / 100f;
    }
}
