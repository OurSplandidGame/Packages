using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MagicalFX
{
	public class FX_HitSpawner : MonoBehaviour
	{

        public GameObject attacker;
        public float range = 5;
        public float damage = 50;
		public GameObject FXSpawn;
		public bool DestoyOnHit = false;
		public bool FixRotation = false;
		public float LifeTimeAfterHit = 1;
		public float LifeTime = 0;
	
		void Start ()
		{
		
		}
	
		void Spawn ()
		{
			if (FXSpawn != null) {
				Quaternion rotate = this.transform.rotation;
				if (!FixRotation)
					rotate = FXSpawn.transform.rotation;
				GameObject fx = (GameObject)GameObject.Instantiate (FXSpawn, this.transform.position, rotate);
				if (LifeTime > 0)
					GameObject.Destroy (fx.gameObject, LifeTime);
			}
			if (DestoyOnHit) {
			
				GameObject.Destroy (this.gameObject, LifeTimeAfterHit);
				if (this.gameObject.GetComponent<Collider>())
					this.gameObject.GetComponent<Collider>().enabled = false;

			}
		}
	
		void OnTriggerEnter (Collider other)
		{
            if(other.gameObject.tag == "Floor")
            {
                Character script = attacker.GetComponent<Character>();
                List<GameObject> targets =  script.GetEnemies();
                foreach(GameObject e in targets)
                {
                    print(Vector3.Distance(transform.position, e.transform.position));
                    if (Vector3.Distance(transform.position,e.transform.position) <= range)
                    {
                        
                        Character targetScript = e.GetComponent<Character>();

                        targetScript.TakeDamage(attacker, 0, damage);
                    }
                }
                Spawn();
            }
			
		}
	
		void OnCollisionEnter (Collision collision)
		{
			Spawn ();
		}
	}
}