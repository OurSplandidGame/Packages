using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    public GameObject explosion;
    public GameObject attacker;
    public float clear = 3f;
    public float delay = 1.5f;
    public float damage = 50f;
    public float range = 10f;

    float timer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(timer >= delay)
        {
            explosion.SetActive(true);
            Character script = attacker.GetComponent<Character>();
            List<GameObject> targets = script.GetEnemies();
            foreach (GameObject e in targets)
            {
                print(Vector3.Distance(transform.position, e.transform.position));
                if (Vector3.Distance(transform.position, e.transform.position) <= range)
                {

                    Character targetScript = e.GetComponent<Character>();
                    targetScript.TakeDamage(attacker, 0, damage);
                }
            }
        }
        if(timer >= clear)
        {
            Destroy(gameObject);
        }
        timer += Time.deltaTime;
	}
}
