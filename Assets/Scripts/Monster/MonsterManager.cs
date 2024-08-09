using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Entities;


public class MonsterManager : MonoBehaviour
{
    List<GameObject> listPathNodes;
    public GameObject monsterPrefab;
    private bool isGameStart = true;
    private float timer = 0f;
    private HashSet<int> spawnedTimes = new HashSet<int>();


    void Start()
    {
        GameObject parentGameObject = GameObject.Find("PathNode");
        if (parentGameObject != null)
        {
            if (listPathNodes == null)
            {
                listPathNodes = new List<GameObject>();
            }
            foreach (Transform child in parentGameObject.transform)
            {
                if (child.name.StartsWith("Path"))
                {
                    listPathNodes.Add(child.gameObject);
                }
            }
        }
        else
        {
            Debug.LogError("Parent GameObject 'PathNode' not found");
        }
    }

    private IEnumerator SpawnMonstersWithDelay(int amount, int monsterType)
    {
        var monsterData = AppData.monsterData;
        for (int i = 0; i < amount; i++)
        {
            float random = Random.Range(0.03f, 0.5f);

            if (listPathNodes.Count > 0)
            {
                GameObject monster = Instantiate(monsterPrefab, listPathNodes[0].transform.position, Quaternion.identity);
                if (monster.TryGetComponent<Monster>(out var monsterScript))
                {
                    monsterScript.SetPathNodes(listPathNodes);
                    monsterScript.Speed = Random.Range(monsterData[monsterType].Speed - 0.5f, monsterData[monsterType].Speed + 0.5f);
                    monsterScript.Health = monsterData[monsterType].Health;
                    monsterScript.Damage = monsterData[monsterType].Damage;
                    monsterScript.MonsterType = monsterData[monsterType].MonsterType;
                    monsterScript.AttackRange = monsterData[monsterType].AttackRange;
                }
            }
            else
            {
                Debug.Log("No path nodes available for spawning the monster");
            }
            yield return new WaitForSeconds(random);
        }
    }

    public void SpawnMonsters(int amount, int monsterType)
    {
        StartCoroutine(SpawnMonstersWithDelay(amount, monsterType));
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (isGameStart)
        {
            foreach (var entry in AppData.Waves)
            {
                int spawnTime = entry[0];
                int amount = entry[1];
                int monsterType = entry[2];
                if (Mathf.FloorToInt(timer) == spawnTime && !spawnedTimes.Contains(spawnTime))
                {
                    StartCoroutine(SpawnMonstersWithDelay(amount, monsterType));
                    spawnedTimes.Add(spawnTime);
                }
            }
            if (spawnedTimes.Count == AppData.Waves.Count)
            {
                isGameStart = false;
            }
        }
    }
}