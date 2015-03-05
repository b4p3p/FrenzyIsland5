using UnityEngine;
using System.Collections;
using Engine.Characters.Entity;
using System.Collections.Generic;
using Assets.Script;
using Script.GUI.MatingCall;
using Assets.Engine.AI.GeneticAlgorithm;
using Engine.Characters.Entity.Characters;
using System.Linq;


public class ViewBuildingMenu : MonoBehaviour
{
    public GameObject ButtonQuit;

    internal ViewBuildingMenu() 
    {
 
    }

    void Start()
    {
        AddEvent();
    }

    private void ButtonQuit_OnClick()
    {
        GuiController.CloseActiveWindow();
    }

    private void Building_OnClick()
    {
        GuiController.CloseActiveWindow();

    }

    private void AddEvent()
    {
        EventDelegate.Add(ButtonQuit.GetComponent<UIButton>().onClick, ButtonQuit_OnClick);
        Transform table = transform.FindChild("BackBuilder/ScrollView/Table");
        foreach (Transform item in table.transform)
        {
            EventDelegate.Add( item.gameObject.GetComponent<UIButton>().onClick, Building_OnClick);
        }
    }
}
