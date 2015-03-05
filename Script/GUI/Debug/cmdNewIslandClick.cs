using UnityEngine;
using System.Collections;
using terraformerIsland.DiamondAlghorithm;

public class cmdNewIslandClick : MonoBehaviour {

    void OnClick()
    {
        GameObject terrain = GameObject.Find("Terrain");
        IslandBehaviour diamondSquare = terrain.GetComponent<IslandBehaviour>();
        diamondSquare.NewIsland();
    }
}
