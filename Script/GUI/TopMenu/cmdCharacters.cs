using UnityEngine;
using System.Collections;
using System.Linq;
using Engine.Characters.Entity;
using System.Collections.Generic;
using Assets.Script;
using Script.GUI.MatingCall;
using Assets.Script.GUI.Menu;

namespace Assets.Script.GUI
{
    public class cmdCharacters : MonoBehaviour
    {
        void OnClick()
        {
            MenuCouple menuCouple = new MenuCouple();
            GuiController.Open(menuCouple);
        }

    }
}
