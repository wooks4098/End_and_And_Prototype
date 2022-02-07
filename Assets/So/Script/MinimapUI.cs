using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapUI : MonoBehaviour
{
    public Button btn;
    public GameObject minimap;

    // Start is called before the first frame update
    void Start()
    {
        minimap.SetActive(false);
        btn.onClick.AddListener(showminimap);
    }

    void showminimap()
    {
        minimap.SetActive(true);
        btn.onClick.AddListener(hideminimap);
    }

    void hideminimap()
    {
        minimap.SetActive(false);
        btn.onClick.AddListener(showminimap);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
