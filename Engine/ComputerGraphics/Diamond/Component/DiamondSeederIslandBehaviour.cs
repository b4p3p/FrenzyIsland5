using UnityEngine;
using System.Collections;
using terraformerIsland.DiamondAlghorithm;

public class DiamondSeederIslandBehaviour : MonoBehaviour 
{
    public int TileSize = 64;
    
    private DiamondSeeder diamondSeeder = null;

	// Use this for initialization
	void Start () {
        diamondSeeder = new DiamondSeeder(TileSize);
	}

    public DiamondSeeder GetDiamonSeeder()
    {
        return diamondSeeder;
    }
	
}
