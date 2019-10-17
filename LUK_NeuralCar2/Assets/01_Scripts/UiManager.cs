using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    bool isOpen;
    public GameObject toolsMenu;

    public void OpenCloseMenu()
    {
        if(isOpen == true)
        {
            toolsMenu.SetActive(false);
            isOpen = false;
        }
        else if(isOpen == false)
        {
            toolsMenu.SetActive(true);
            isOpen = true;
        }
    }


}
