using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.GUI.Menu
{
    public class MenuCouple : IMenu
    {
        public MenuCouple() : base()
        {

        }

        public override void Open()
        {
            ViewCombineCharacter controllerMenu = InstanceMenu.GetComponent<ViewCombineCharacter>();
            controllerMenu.LoadCharacters();
        }        

        public override GameObject Instantiate()
        {
            return base.Instantiate("GUI/MatingCall/BackgroundCharacters");
        }
    }
}
