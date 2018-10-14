using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject obj;
    public uint max = 7;
    public uint interval = 5;
    public float probability = 0.7f;

    public float rangeMin = 0;
    public float rangeMax = 20;

    int number;
	// Use this for initialization
	void Start () {
        number = 0;
	}

    float timer = 0;
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0;
            GameObject[] arr = GameObject.FindGameObjectsWithTag(obj.tag);

            number = arr.Length;
            if (number >= max) return;
            float factor = ((float)max - number) / max * probability;

            if(Random.Range(0.0f, 1.0f) <= factor)
            {
     
                float dist = Random.Range(rangeMin, rangeMax);
                float rot = Random.Range(0f, 360f);
                Vector3 pos = new Vector3(0, 50, dist);
                pos = Quaternion.Euler(0, rot, 0) * pos;
                pos += transform.position;

                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(pos, new Vector3(0, -1, 0),out hit,100f, 1<<9))
                {
                    if (hit.point.y > 2)
                    {
                        Instantiate(obj, hit.point, transform.rotation);
                    }
                }
                
            }
        }
        

    }
}
