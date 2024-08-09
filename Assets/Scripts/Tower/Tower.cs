using NUnit.Framework;
using UnityEngine;

public enum TowerType
{
    Normal,
    Archer,
    Maze,
    Bomer,
}

public abstract class Tower : MonoBehaviour
{
    public float range { get; set; }
    public float damage { get; set; }
    public float fireRate { get; set; }
    private float fireCountdown = 0f;
    public int[][] upgradeData = new int[3][]; // 3 levels of upgrade 
    public TowerType towerType;
    protected Transform target;

    void Update()
    {
        fireCountdown -= Time.deltaTime;
        if (target == null || Vector3.Distance(transform.position, target.position) > range)
        {
            FindTarget();
        }

        if (fireCountdown <= 0f)
        {
            if (target != null)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    protected abstract void Shoot();

    protected virtual void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("monster");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
}

public class NormalTower : Tower
{
    private Sprite sprite;
    void Start()
    {
        range = 1f;
        damage = 10f;
        fireRate = 1f;
        sprite = Resources.Load<Sprite>("NormalTower");
    }

    protected override void Shoot()
    {
        // Implement shooting logic here
    }
}

public class ArcherTower : Tower
{
    void Start()
    {
        range = 15f;
        damage = 15f;
        fireRate = 1.5f;
    }

    protected override void Shoot()
    {
        // Implement shooting logic here
    }
}

public class MazeTower : Tower
{
    void Start()
    {
        range = 20f;
        damage = 20f;
        fireRate = 2f;
    }

    protected override void Shoot()
    {
        // Implement shooting logic here
    }
}

public class BomerTower : Tower
{
    void Start()
    {
        range = 25f;
        damage = 25f;
        fireRate = 2.5f;
    }

    protected override void Shoot()
    {
        // Implement shooting logic here
    }
}
