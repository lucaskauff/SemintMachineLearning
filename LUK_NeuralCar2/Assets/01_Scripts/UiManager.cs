using UnityEngine;

public class UiManager : MonoBehaviour
{
    //UI parts
    [SerializeField] GameObject toolsMenu = default;
    [SerializeField] GameObject netMenu = default;
    [SerializeField] GameObject minimapMenu = default;
    [SerializeField] GameObject mapCamera = default;

    //Private bools
    bool isToolsMenuOpen;
    bool isNetMenuOpen;
    bool isMapMenuOpen;

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
}