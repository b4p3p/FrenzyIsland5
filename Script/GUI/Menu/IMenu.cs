using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.GUI.Menu
{
    public abstract class IMenu
    {
        public GameObject InstanceMenu { get; set; }

        public IMenu() 
        {
            InstanceMenu = null;
        }

        public abstract void Open();
        
        public void Close()
        {
            GameObject.Destroy(InstanceMenu);
        }

        public abstract GameObject Instantiate();
        
        public GameObject Instantiate(string nameResource)
        {
            GameObject prefab = Resources.Load<GameObject>( nameResource );

            UIAnchor anchor = GameObject.FindGameObjectWithTag( Tags.AnchorMenu ).GetComponent<UIAnchor>();
            InstanceMenu = GameObject.Instantiate(prefab) as GameObject;

            InstanceMenu.transform.parent = anchor.transform;
            InstanceMenu.transform.localPosition = new Vector3(0, -420, 0);
            InstanceMenu.transform.localScale = new Vector3(1, 1, 1);
            InstanceMenu.name = prefab.name;

            return InstanceMenu;
        }
    }

}
