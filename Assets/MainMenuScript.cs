using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public GameObject panelAbout; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OpenPanelAbout(){
        panelAbout.SetActive(true);
    }

    public void ClosePanelAbout(){
        panelAbout.SetActive(false);
    }
}
