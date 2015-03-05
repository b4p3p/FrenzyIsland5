using UnityEngine;
using System.Collections;
using Assets.Script.GUI.Menu;

public class cmdBuilding : MonoBehaviour {

    void OnClick()
    {
        MenuBuilding menuBuilding = new MenuBuilding();
        GuiController.Open(menuBuilding);
    }
}
