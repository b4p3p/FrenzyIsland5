using UnityEngine;
using System.Collections;
using System;

public class SelectableItem : MonoBehaviour {

    public GameObject Highlight;
    private IslandBehaviour island;
    
	// Use this for initialization
	void Start () {
        island = Terrain.activeTerrain.GetComponent<IslandBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {

        // Highlight
        if (Highlight.activeInHierarchy)
        {
            Highlight.transform.Rotate(Vector3.up, 2);
        }

	}

    void OnMouseEnter()
    {
        if (GuiController.IsMenuOpened) return;
        Highlight.SetActive(true);
    }
    void OnMouseExit()
    {
        Highlight.SetActive(false);
    }

    void OnMouseDown()
    {
        

    }

    void OnClick()
    {
        
    }

    void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.name.Equals("Terrain"))
            {
                transform.position = hit.point;
                island.SetFlatTerrainAround(this.gameObject);
            }
        }
    }

    bool updated;

    
}
