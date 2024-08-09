using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterData", menuName = "TowerDefense/MonsterData")]
public class MonsterData : ScriptableObject
{
    public int monsterType;
    public float health;
    public float speed;
    public float damage;
    public GameObject monsterPrefab;
}
