using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject[] TutorialImage;

    private void Start()
    {
        StartCoroutine(HideTutial());
    }

    IEnumerator HideTutial()
    {
        yield return new WaitForSeconds(7f);
        for (int i = 0; i < TutorialImage.Length; i++)
            TutorialImage[i].SetActive(false);
    }
}
