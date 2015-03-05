using UnityEngine;
using System.Collections;
using Script.GUI.MatingCall;
using Assets.Script;
using Assets.Script.GUI.Menu;
using Assets.Script.GUI;

public class GuiController : MonoBehaviour {

    public static bool IsMenuOpened
    {
        get { 
            return _isMenuOpened; 
        }
        set
        {
            _isMenuOpened = value;
            SimpleCamera.IsActive = !value ;
        }
    }
    private static bool _isMenuOpened = false;

    public static SimpleCamera SimpleCamera 
    { 
        get {
            if (_simpleCamera != null) return _simpleCamera;
            _simpleCamera = GameObject.FindGameObjectWithTag(Tags.MainCamera).GetComponent<SimpleCamera>();
            return _simpleCamera;
        }
    }
    private static SimpleCamera _simpleCamera; 

    private static IMenu activeMenu = null;

    internal static void Open( IMenu menu )
    {
        if (activeMenu != null ) return;
        if (menu == null) return;

        activeMenu = menu;

        GameObject instanceMenu = menu.Instantiate();
        menu.Open();
        
        IsMenuOpened = true;
    }

    internal static void CloseActiveWindow()
    {
        if (activeMenu == null) return;

        activeMenu.Close();
        activeMenu = null;

        IsMenuOpened = false;
    }
}
