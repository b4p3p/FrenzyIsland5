using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.GUI.Menu
{
    public class MenuBuilding : IMenu
    {
        public override void Open()
        {
            ViewBuildingMenu controller = InstanceMenu.GetComponent<ViewBuildingMenu>();
        }

        public override GameObject Instantiate()
        {
            return base.Instantiate("GUI/BuildingMenu/BackgroundBuilding");
        }
    }
}
