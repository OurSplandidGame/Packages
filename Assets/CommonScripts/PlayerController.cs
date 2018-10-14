using UnityEngine;
using System.Collections;
using MagicalFX;

[RequireComponent (typeof (Animator))]
public class PlayerController : Character {
    public float mana;
    public float maxMana;
    public float armor;
    public float maxArmor;
    public float maxDamage;
    public float speed;
    public GameObject skill1;
    public float skill1_Cooldown;
    public GameObject skill2;
    public Camera cam;
    public ParticleSystem attackEffect;
    private CharacterController player;
    int floorMask;
    float skill1_Timer;
    public AudioClip[] Audios;
    public AudioSource audioSource;
    public GameObject deadUI;
    protected override void Awake()
    {
        base.Awake();
        floorMask = LayerMask.GetMask("Floor");
        player = GetComponent<CharacterController>();
        skill1_Timer = skill1_Cooldown;
        skill1.GetComponentInChildren<FX_HitSpawner>().attacker = gameObject;
        skill2.GetComponent<Explosion>().attacker = gameObject;
        audioSource = GetComponent<AudioSource>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!isActive) return;
        if (Input.GetKeyDown("z") && !attacking)
        {
            Move(new Vector3());
            Attack();
        }else if(Input.GetKeyDown("x") && !attacking && skill1_Timer <= 0)
        {
            skill1_Timer = skill1_Cooldown;
            animator.SetTrigger("Attack2");
            Skill2();
        }
        else if (!attacking && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
        {
            animator.ResetTrigger("Attack1");
            Vector3 goFront = cam.transform.forward;
            goFront.y = 0;
            goFront = Vector3.Normalize(goFront);
            Vector3 goRight = cam.transform.right;
            goRight.y = 0;
            goRight = Vector3.Normalize(goRight);
            Vector3 moveDir = Vector3.Normalize(goRight * Input.GetAxis("Horizontal") + goFront * Input.GetAxis("Vertical"));
            Move(speed * moveDir);
        }
        else
        {
            Move(new Vector3());
        }


        skill1_Timer -= Time.deltaTime;
    }
    void OnTriggerEnter(Collider other)
    {
        //print(other.gameObject.tag);
        if(other.gameObject.tag == "Pickup")
        {
            PlayerInventory playerInventory = gameObject.GetComponent<PlayerInventory>();
            Rotate rotate = other.gameObject.GetComponent<Rotate>();
            playerInventory.addItemToInventory(rotate.getItemId());
            Destroy(other.gameObject);
            audioSource.clip = Audios[0];
            audioSource.Play();
        }
    }
    void Move(Vector3 velocity)
    {
        player.transform.forward =Vector3.Lerp(velocity, player.transform.forward,2.0f*Time.deltaTime);
        player.SimpleMove(velocity);
    }

    protected override void Attack()
    {
        if(target != null) TurnToTarget();
        base.Attack();
    }

    protected static void PositionCast(Vector3 position, GameObject skill)
    {
        Instantiate(skill, position, skill.transform.rotation);
    }


    protected void Skill1()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, 100f, floorMask))
        {
            PositionCast(floorHit.point,skill1);
        }
    }

    protected void Skill2()
    {
        PositionCast(transform.position, skill2);
        audioSource.clip = Audios[3];
        audioSource.Play();
    }

    protected override void AnimMove(float speed)
    {
        base.AnimMove(speed);
        animator.SetFloat("Speed",speed);
    }

    protected override void AnimAttack()
    {
        base.AnimAttack();
        animator.SetTrigger("Attack1");
        attackEffect.Play();
    }

    protected override void AnimDie()
    {
        base.AnimDie();
        animator.SetTrigger("Death");
    }
    
    private void playAudio(string audio){
        if(audio == "attack")
        {
            audioSource.clip = Audios[1];
            audioSource.Play();
        }
    }
    public override void TakeDamage(GameObject attacker, int type, float amount)
    {
        base.TakeDamage(attacker, type, amount);
        audioSource.clip = Audios[2];
        audioSource.Play();
    }

    public void DeathUI()
    {
        audioSource.clip = Audios[4];
        audioSource.Play();
        deadUI.SetActive(true);
    }
}
