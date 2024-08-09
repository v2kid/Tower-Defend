using UnityEngine;

public class Spot : MonoBehaviour
{
    [SerializeField]
    private bool isOccupied = false;
    private static Spot selectedSpot = null;
    private TowerManager towerManager;

    public bool IsOccupied
    {
        get { return isOccupied; }
        set { isOccupied = value; }
    }

    private void OnDrawGizmos()
    {
        if (selectedSpot == this)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
    void Start()
    {
        towerManager = FindObjectOfType<TowerManager>();
    }

    void OnMouseDown()
    {
        towerManager.SelectSpot(this);
    }

    public void Select()
    {
        if (selectedSpot != null)
        {
            selectedSpot.Deselect();
        }
        selectedSpot = this;
    }

    public void Deselect()
    {
        if (selectedSpot == this)
        {
            selectedSpot = null;
        }
    }
}