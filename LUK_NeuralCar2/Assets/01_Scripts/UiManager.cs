using UnityEngine;

public class UiManager : MonoBehaviour
{
    //UI parts
    [SerializeField] GameObject toolsMenu = default;
    [SerializeField] GameObject netMenu = default;
    [SerializeField] GameObject minimapMenu = default;
    [SerializeField] GameObject mapCamera = default;
    [SerializeField] GameObject graphMenu = default;

    //Private bools
    bool isToolsMenuOpen;
    bool isNetMenuOpen;
    bool isMapMenuOpen;
    bool isGraphMenuOpen;

    public void OpenCloseToolsMenu()
    {
        isToolsMenuOpen = !isToolsMenuOpen;
        toolsMenu.SetActive(isToolsMenuOpen);
    }

    public void OpenCloseNetMenu()
    {
        isNetMenuOpen = !isNetMenuOpen;
        netMenu.SetActive(isNetMenuOpen);
    }

    public void OpenMapMenu()
    {
        isMapMenuOpen = !isMapMenuOpen;
        minimapMenu.SetActive(isMapMenuOpen);
        mapCamera.SetActive(isMapMenuOpen);
    }

    public void OpenGraphMenu()
    {
        isGraphMenuOpen = !isGraphMenuOpen;
        graphMenu.SetActive(isGraphMenuOpen);
    }
}