using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterState
{
    void EnterState(Monster monster);
    void UpdateState(Monster monster);
    void ExitState(Monster monster);
}

public class MovingState : IMonsterState
{
    public void EnterState(Monster monster)
    {
        Debug.Log("Entering Moving State");
    }

    public void UpdateState(Monster monster)
    {
        if (monster.PathNodes == null)
        {
            Debug.LogError("PathNodes is null");
            return;
        }

        if (monster.CurrentNodeIndex < 0 || monster.CurrentNodeIndex >= monster.PathNodes.Count)
        {
            Debug.LogError("CurrentNodeIndex is out of bounds: " + monster.CurrentNodeIndex);
            return;
        }

        monster.MoveTowardsNode(monster.PathNodes[monster.CurrentNodeIndex]);
    }

    public void ExitState(Monster monster)
    {
        Debug.Log("Exiting Moving State");
    }
}

public class AttackingState : IMonsterState
{
    private float nextAttackTime = 0f;

    public void EnterState(Monster monster)
    {
        Debug.Log("Entering Attacking State");
    }

    public void UpdateState(Monster monster)
    {
        if (monster.soldiersList.Length == 0)
        {
            monster.TransitionToState(new MovingState());
            return;
        }
        bool soldierInRange = false;
        Soilder closestSoldier = null;
        float minDistance = float.MaxValue;

        foreach (var soldier in monster.soldiersList)
        {
            if (soldier == null)
                continue;
            float distance = Vector3.Distance(soldier.transform.position, monster.transform.position);
            if (distance <= monster.AttackRange)
            {
                soldierInRange = true;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestSoldier = soldier;
                }
            }
        }

        if (!soldierInRange)
        {
            monster.TransitionToState(new MovingState());
        }
        else if (Time.time > nextAttackTime && closestSoldier != null)
        {
            if (closestSoldier != null) // Check again before attacking
            {
                closestSoldier.Health -= monster.Damage;
                // Debug.Log("Monster attacked soldier. Soldier health: " + closestSoldier.Health);
                nextAttackTime = Time.time + 1f;

                if (closestSoldier.Health < 0)
                {
                    // Check if the soldier is not null before trying to destroy it
                    if (closestSoldier != null)
                    {
                        // closestSoldier.Died(); // Implement this method to handle soldier's death
                    }
                }
            }
        }
    }
    public void ExitState(Monster monster)
    {
        Debug.Log("Exiting Attacking State");
    }
}


public class DyingState : IMonsterState
{
    public void EnterState(Monster monster)
    {
    }

    public void UpdateState(Monster monster)
    {
        // Implement dying logic
    }

    public void ExitState(Monster monster)
    {
        Debug.Log("Exiting Dying State");
    }
}

public enum MonsterType
{
    Meele = 0,
    Ranged = 1,
    Flying = 2,
    Boss = 3
}
public class Monster : MonoBehaviour
{
    public List<GameObject> PathNodes { get; private set; }
    public int CurrentNodeIndex { get; private set; } = 0;
    private bool isDead = false;
    public Soilder[] soldiersList;
    [SerializeField]
    private float health;
    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    public float Speed { get; set; }
    public int Damage { get; set; }
    [SerializeField]
    private int monsterType;
    public int MonsterType
    {
        get { return monsterType; }
        set { monsterType = value; }
    }
    public float ReachThreshold = 0.1f;
    private float attackRange = 2f;
    public float AttackRange
    {
        get { return attackRange; }
        set { attackRange = value; }
    }

    private IMonsterState currentState;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        TransitionToState(new MovingState());
        gameObject.tag = "monster";
    }


    void Update()
    {
        currentState?.UpdateState(this);
        CheckSoldierInRange();
        CheckDied();
    }


    public void SetPathNodes(List<GameObject> nodes)
    {
        PathNodes = nodes;
        CurrentNodeIndex = 0;
    }

    public void TransitionToState(IMonsterState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }


    public void MoveTowardsNode(GameObject node)
    {
        Vector3 direction = node.transform.position - transform.position;
        direction.z = 0;
        transform.position += Speed * Time.deltaTime * direction.normalized;

        animator.SetBool("right", false);
        animator.SetBool("left", false);
        animator.SetBool("forward", false);
        animator.SetBool("back", false);

        if (direction.x > 0)
        {
            animator.SetBool("right", true);
        }
        else if (direction.x < 0)
        {
            animator.SetBool("left", true);
        }
        else if (direction.y > 0)
        {
            animator.SetBool("forward", true);
        }
        else if (direction.y < 0)
        {
            animator.SetBool("back", true);
        }

        if (direction.magnitude < ReachThreshold)
        {
            CurrentNodeIndex++;
            if (CurrentNodeIndex >= PathNodes.Count)
            {
                Destroy(gameObject);
            }
        }
    }
    private void CheckDied()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            Speed = 0;
            animator.SetTrigger("die");
            StartCoroutine(WaitForAnimationAndDestroy());
        }
    }


    private IEnumerator WaitForAnimationAndDestroy()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName("die") || stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        Destroy(gameObject);
    }



    public void CheckSoldierInRange()
    {
        if (currentState is AttackingState)
            return;

        soldiersList = FindObjectsByType<Soilder>(FindObjectsSortMode.None);

        foreach (var soldier in soldiersList)
        {
            if (Vector3.Distance(soldier.transform.position, transform.position) <= AttackRange)
            {
                TransitionToState(new AttackingState());
                break;
            }
        }
    }
}

