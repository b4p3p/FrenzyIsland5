using UnityEngine;
using System.Collections;
using terraformerIsland.DiamondAlghorithm;
using Engine.ComputerGraphics.Component;

public class cmdNewIslandClick : MonoBehaviour {

    void OnClick()
    {
        GameObject terrain = GameObject.Find("Terrain");
        IslandBehaviour diamondSquare = terrain.GetComponent<IslandBehaviour>();
        diamondSquare.NewIsland();
    }
}
