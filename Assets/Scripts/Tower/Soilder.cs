using UnityEngine;

public class Soldier : MonoBehaviour
{
    public float Speed { get; set; }
    public int Health { get; set; }
    public int Damage { get; set; }
    public int SoldierType { get; set; }
    public float AttackRange { get; set; }
    private Monster[] monsterList;

    private void Start()
    {
        Speed = 1;
        Health = 100;
        Damage = 10;
        SoldierType = 0;
        AttackRange = 2;
    }

    private void Update()
    {
        CheckForEnemies();
    }

    void CheckForEnemies()
    {
        monsterList = FindObjectsOfType<Monster>();
        foreach (var monster in monsterList)
        {
            if (Vector3.Distance(monster.transform.position, transform.position) <= AttackRange)
            {
                Attack(monster);
            }
        }
    }

    void Attack(Monster monster)
    {
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
