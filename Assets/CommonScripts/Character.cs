using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    public bool debug = false;
    public float discoverRange = 5;
    public float discoverAngle = 360f;
    public float attackRange = 1.0f;
    public float attackAngle = 90f;
    public float attackInterval = 0.5f;
    public float damage = 10;
    public float maxHealth = 100;
    public float health = 100;
    public float healthRestore = 0;
    public string[] enemyTagList = { };
    public bool IsActive { get { return isActive; } }
    public GameObject hitEffect;

    protected Vector3 velocity;
    protected List<GameObject> targets;
    protected GameObject target;
    protected Animator animator;
    protected Rigidbody rb;
    protected CapsuleCollider bodyCollider;
    protected bool isActive;
    protected bool attacking;

    private float timer1s;
    protected float attackTimer;
    private Vector3 lastPos;
    
    //Lock one target from targetList
    protected virtual void AutoLockTarget()
    {
        float minDistance = float.MaxValue;
        target = null;
        foreach (GameObject e in targets)
        {
            Character script = e.GetComponent<Character>();
            if (!script.IsActive) continue;
            float distance = (e.transform.position - transform.position).magnitude;
            float angle = Vector3.Angle(e.transform.position - transform.position, transform.forward);
            if (distance <= discoverRange && distance < minDistance && angle < discoverAngle / 2f)
            {
                target = e;
                minDistance = distance;
            }
        }
        if (debug) print(tag + "Lock target: " + (target != null ? target.tag : "None") + " from list of: " + targets.Count);
    }

    //Update targetList with enemy tags
    void UpdateTargets()
    {
        targets.Clear();
        if (debug) print(tag + ": start update enemy list");
        HashSet <GameObject> dedup = new HashSet<GameObject>();
        foreach (string s in enemyTagList)
        {
            GameObject[] arr = GameObject.FindGameObjectsWithTag(s);
            
            foreach (GameObject e in arr)
            {
                Character script = e.GetComponent<Character>();
                if (dedup.Add(e) && script != null && script.IsActive)
                {
                    targets.Add(e);
                }
            }
        }
        if(debug) print(tag+" targets: " + targets.Count);
    }
    
    // Use this for initialization
    protected virtual void Awake ()
    {
        targets = new List<GameObject>();
        rb = GetComponent<Rigidbody>();
        bodyCollider = GetComponent<CapsuleCollider>();
        timer1s = 0;
        velocity = new Vector3(0,0,0);
        lastPos = transform.position;
        animator = GetComponent<Animator>();
        isActive = true;
        attacking = false;
        UpdateTargets();
        
    }
	
    protected virtual void FixedUpdate()
    {
        if (!isActive) return;
        velocity = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;
        timer1s += Time.deltaTime;

        //Restore health
        if (health < maxHealth)
        {
            health += healthRestore * Time.deltaTime;
            health = health > maxHealth ? maxHealth : health;
        }

        //Update targetList every 1 second(Time consuming method)
        if (timer1s >= 1)
        {
            timer1s = 0;
            UpdateTargets();
        }
        AutoLockTarget();

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval) attackTimer = attackInterval + 1;
        AnimMove(velocity.magnitude);
    }


    public virtual void TakeDamage(GameObject attacker, int type, float amount)
    {
        if (debug) print("Damage take: " + amount);
        if (!isActive) return;

        Instantiate(hitEffect, this.transform.position + new Vector3(0, 1, 0), hitEffect.transform.rotation);
        foreach (string e in enemyTagList)
        {
            if(e == attacker.tag)
            {
                target = attacker;
            }
        }
        AnimHurt();
        health -= amount;
        if(health <= 0)
        {
            health = 0;
            Die();
        }
        
    }

    protected virtual void Die()
    {
        if (!isActive) return;
        targets.Clear();
        target = null;
        if(bodyCollider != null) bodyCollider.enabled = false;
        isActive = false;
        AnimDie();
    }

    protected virtual void Attack()
    {
        attacking = true;
        AnimAttack();
    }
    
    protected virtual void TurnToTarget()
    {
        //transform.forward = Vector3.Lerp(transform.forward, target.transform.position - transform.position, 2f * Time.deltaTime);
        transform.forward = target.transform.position - transform.position;
    }
    
    //Animation Event
    protected virtual void DamageTarget() 
    {
        attacking = false ;
        if (!isActive) return;
        if (target != null)
        {
            if (debug) print("target is not null");
            if (InAttackRange(target.transform.position))
            {
                if (debug) print("target is in range");
                Character script = target.GetComponent<Character>();
                script.TakeDamage(this.gameObject, 1, damage);
            }
        }
    }

    //Animation Event
    protected virtual void AOEDamage()
    {
        if (!isActive) return;
        foreach(GameObject target in targets)
        {
            if (InAttackRange(target.transform.position))
            {
                Character script = target.GetComponent<Character>();
                script.TakeDamage(this.gameObject, 1, damage);
            }
        }
        attacking = false;
    }

    protected bool InAttackRange(Vector3 position)
    {
        float distance = (position - transform.position).magnitude;
        float angle = Vector3.Angle(position - transform.position, transform.forward);
        return distance <= attackRange && angle <= attackAngle / 2f;
    }

    public List<GameObject> GetEnemies()
    {
        return targets;
    }

    protected virtual void AnimMove(float speed)
    {

    }

    protected virtual void AnimAttack()
    {

    }

    protected virtual void AnimHurt()
    {

    }

    protected virtual void AnimDie()
    {

    }
}
