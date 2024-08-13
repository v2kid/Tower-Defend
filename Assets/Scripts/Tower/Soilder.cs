
using UnityEngine;

public interface ISoilderState
{
    void EnterState(Soilder Soilder);
    void UpdateState(Soilder Soilder);
    void ExitState(Soilder Soilder);
}

public class SoilderMoveState : ISoilderState
{
    public void EnterState(Soilder Soilder)
    {
        // Enter logic for Move state, if any
    }

    public void UpdateState(Soilder Soilder)
    {
        // Logic to handle Soilder movement, if needed
    }

    public void ExitState(Soilder Soilder)
    {
        // Exit logic for Move state, if any
    }
}

public class SoilderAttackState : ISoilderState
{
    private float nextAttackTime = 0f;

    public void EnterState(Soilder Soilder)
    {
        // Initialization logic for entering the Attack state
    }

    public void UpdateState(Soilder Soilder)
    {
        if (Soilder.TargetMonsters.Length == 0)
        {
            Soilder.TransitionToState(new SoilderMoveState());
            return;
        }

        Monster closestMonster = FindClosestMonsterInRange(Soilder);

        if (closestMonster == null)
        {
            Soilder.TransitionToState(new SoilderMoveState());
        }
        else if (Time.time > nextAttackTime)
        {
            MoveAndAttackMonster(Soilder, closestMonster);
        }
    }

    private Monster FindClosestMonsterInRange(Soilder Soilder)
    {
        Monster closestMonster = null;
        float minDistance = float.MaxValue;

        foreach (var monster in Soilder.TargetMonsters)
        {
            if (monster == null) continue;

            float distance = Vector3.Distance(monster.transform.position, Soilder.transform.position);
            if (distance <= Soilder.DetectionRange && distance < minDistance)
            {
                minDistance = distance;
                closestMonster = monster;
            }
        }

        return closestMonster;
    }

    private void MoveAndAttackMonster(Soilder Soilder, Monster monster)
    {
        if (monster != null)
        {
            if (Vector3.Distance(Soilder.transform.position, monster.transform.position) > Soilder.AttackRange)
            {
                Soilder.transform.position = Vector3.MoveTowards(Soilder.transform.position, monster.transform.position, Soilder.Speed * Time.deltaTime);
            }
            else
            {
                Soilder.PerformAttack();
                monster.Health -= Soilder.Damage;
                Debug.Log($"Soilder attacked monster. Monster health: {monster.Health}");
                nextAttackTime = Time.time + 1f;

                if (monster.Health <= 0)
                {
                    // Handle monster's death
                }
            }
        }
    }

    public void ExitState(Soilder Soilder)
    {
        Debug.Log("Exiting Attack State");
    }
}

public enum SoilderType
{
    Melee = 0,
    Ranged = 1,
}

public class Soilder : MonoBehaviour
{
    public Monster[] TargetMonsters { get; set; }
    
    [SerializeField]
    private float health;
    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    [SerializeField]
    private int damage;
    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public float Speed { get; set; }
    
    [SerializeField]
    private float detectionRange;
    public float DetectionRange
    {
        get { return detectionRange; }
        set { detectionRange = value; }
    }
    public SoilderType SoilderType { get; set; }

    [SerializeField]
    private float attackRange;
    public float AttackRange
    {
        get { return attackRange; }
        set { attackRange = value; }
    }

    private ISoilderState currentState;
    public Animator Animator { get; private set; }

    private void Start()
    {
        Animator = GetComponent<Animator>();
        TransitionToState(new SoilderMoveState());
        gameObject.tag = "Soilder";
    }

    private void Update()
    {
        currentState?.UpdateState(this);
        DetectAndTransitionToAttack();
        CheckIfDead();
    }

    public void TransitionToState(ISoilderState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    private void DetectAndTransitionToAttack()
    {
        if (currentState is SoilderAttackState) return;

        TargetMonsters =  FindObjectsByType<Monster>(FindObjectsSortMode.None);

        foreach (var monster in TargetMonsters)
        {
            if (Vector3.Distance(monster.transform.position, transform.position) <= DetectionRange)
            {
                TransitionToState(new SoilderAttackState());
                break;
            }
        }
    }

    private void CheckIfDead()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void PerformAttack()
    {
        Animator.SetBool("Attack", true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
