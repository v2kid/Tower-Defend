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
        // Debug.Log("Entering Moving State");
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
    public void EnterState(Monster monster)
    {
        Debug.Log("Entering Attacking State");
    }

    public void UpdateState(Monster monster)
    {
        bool soldierInRange = false;
        
        foreach (var soldier in monster.soldiersList)
        {
            if (Vector3.Distance(soldier.transform.position, monster.transform.position) <= monster.AttackRange)
            {
                soldierInRange = true;
                break;
            }
        }

        if (!soldierInRange)
        {
            monster.TransitionToState(new MovingState());
        }
        else
        {
            // Attack logic here, e.g., dealing damage to the soldier.
        }
    }

    public void ExitState(Monster monster)
    {
        
    }
    //check not any solder in range
    //if not change state to moving
    
}

public class DyingState : IMonsterState
{
    public void EnterState(Monster monster)
    {
        Debug.Log("Entering Dying State");
        // Additional initialization for dying
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
    public Soldier[] soldiersList;
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
        CheckSoidlerInRange();

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

        // Reset all animator parameters
        animator.SetBool("right", false);
        animator.SetBool("left", false);
        animator.SetBool("forward", false);
        animator.SetBool("back", false);

        // Set the appropriate animator parameter based on direction
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

    public void CheckSoidlerInRange()
    {
        soldiersList = FindObjectsOfType<Soldier>();
        foreach (var solder in soldiersList)
        {
            if (Vector3.Distance(solder.transform.position, transform.position) <= AttackRange)
            {
                TransitionToState(new AttackingState());
            }
        }
    }
}

