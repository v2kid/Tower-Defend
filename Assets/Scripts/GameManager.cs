
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject SoidlerPrefab;
    
    private GameObject SoilderSpawner;
    private Soilder[] SoildersData;
    void Start()
    {
        SoilderSpawner = GameObject.Find("SoilderSpawn");
        if (SoilderSpawner == null)
        {
            Debug.LogError("SoilderSpawner not found");
        }
    SoildersData = AppData.SoidlerData;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
             GameObject Soidler = Instantiate(SoidlerPrefab, SoilderSpawner.transform.position, Quaternion.identity);
             var soilderScript = Soidler.GetComponent<Soilder>();
                soilderScript.Health = SoildersData[0].Health;
                soilderScript.Damage = SoildersData[0].Damage;
                soilderScript.Speed = SoildersData[0].Speed;    
                soilderScript.AttackRange = SoildersData[0].AttackRange;
                soilderScript.DetectionRange = SoildersData[0].DetectionRange;
                soilderScript.SoilderType = SoildersData[0].SoilderType;
        }
    }
}