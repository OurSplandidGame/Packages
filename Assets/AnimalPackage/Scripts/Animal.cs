using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : AiCharacter
{
    public GameObject dropObj;
    public int[] dropItemIds;
    protected override void AnimAttack()
    {
        base.AnimAttack();
        animator.SetTrigger("Attack");
    }

    protected override void AnimDie()
    {
        if (dropItemIds.Length > 0)
        {
            GameObject drop = Instantiate(dropObj, transform.position, transform.rotation);
            Rotate rotate = drop.GetComponent<Rotate>();
            int index = Random.Range(0, dropItemIds.Length - 1);
            rotate.updateItemInfo(dropItemIds[index]);
        }

        base.AnimDie();
        animator.SetTrigger("Death");
    }


    protected override void AnimMove(float speed)
    {
        base.AnimMove(speed);
        animator.SetFloat("Speed", speed);
    }


}
