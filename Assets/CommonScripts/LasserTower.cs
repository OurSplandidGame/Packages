using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LasserTower : AiCharacter {

    public GameObject tip;

    Light flash;
    LineRenderer lr;
    float lightTimer = 0;
    float flashTime = 0.2f;
    protected override void Awake()
    {

        base.Awake();

        flash = tip.GetComponent<Light>();
        lr = tip.GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (lightTimer > 0)
        {
            lightTimer -= Time.deltaTime;
            flash.enabled = true;
        }
        else
        {
            flash.enabled = false ;
            lr.enabled = false;
        }
    }

    protected override void AnimAttack()
    {
        base.AnimAttack();
        lightTimer = flashTime;
        lr.SetPosition(0, tip.transform.position);
        lr.SetPosition(1, target.transform.position);
        lr.enabled = true;
        DamageTarget();
    }

}
