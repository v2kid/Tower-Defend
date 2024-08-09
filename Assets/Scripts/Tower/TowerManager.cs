using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    private List<GameObject> towerPoints;
    public  List<GameObject> towerPrefabs;
    public static Spot selectedSpot;


    void Start()
    {
        RegisterTowerPoints();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity);
            if (hit.collider && hit.collider.tag == "Map"){
                DeselectAllSpots();
            }
}
            // if (hit.collider != null)
            // {
            //     Debug.Log("Hit object: " + hit.collider.name);
            // }
            // else
            // {
            //     DeselectAllSpots();
            // }

        }
    




    void RegisterTowerPoints()
    {
        GameObject parentGameObject = GameObject.Find("CreateTowerPoint");
        if (parentGameObject != null)
        {
            towerPoints = new List<GameObject>();
            foreach (Transform child in parentGameObject.transform)
            {
                if (child.name.StartsWith("CreateTower"))
                {
                    towerPoints.Add(child.gameObject);
                    child.gameObject.AddComponent<Spot>(); // Add the Spot script to each child
                    Debug.Log("Registered tower point: " + child.gameObject.name);
                }
            }
        }
        else
        {
            Debug.LogError("Parent GameObject 'CreateTowerPoint' not found");
        }
    }

    public void SelectSpot(Spot spot)
    {
        if (selectedSpot != spot) // Ensure the spot is not already selected
        {
            selectedSpot = spot;
            Debug.Log("Selected spot: " + selectedSpot.name);
            selectedSpot.Select();
            // CreateTower(TowerType.Normal);
        }
    }

    void DeselectAllSpots()
    {
        Debug.Log("DeselectAllSpots called");

        if (towerPoints == null)
        {
            Debug.LogWarning("Tower points list is null.");
            return;
        }

        foreach (GameObject towerPoint in towerPoints)
        {
            Spot spot = towerPoint.GetComponent<Spot>();
            if (spot != null)
            {
                spot.Deselect();
            }
        }

        selectedSpot = null;
    }

    public Tower CreateTower(TowerType towerType)
    {
        Tower tower = Instantiate(towerPrefabs[(int)towerType], selectedSpot.transform.position, Quaternion.identity).GetComponent<Tower>();
        selectedSpot.IsOccupied = true;
        return tower;
    }
}
