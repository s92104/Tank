using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public AudioClip boomSound;
    public GameObject boomParticle;
    public GameObject fire;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter(Collider other)
    {
        //大砲子彈
        if (name == "Bullet(Clone)" && (other.tag == "Terrain" || other.tag == "Player" || other.tag == "AI" || other.tag=="Tree"))
        {
            if (other.tag == "Player")
            {
                other.gameObject.GetComponentInParent<Tank>().health -= 5;
            }
            else if (other.tag == "AI")
            {
                other.gameObject.GetComponentInParent<AITank>().health -= 20;
            }      
            else if(other.tag=="Tree")
            {
                GameObject tmp = Instantiate<GameObject>(fire, other.transform.position, fire.transform.rotation);
                GameObject player = GameObject.Find("PlayerTank");
                player.GetComponent<Tank>().fireRec.Add(tmp);
                player.GetComponent<Tank>().fireTime.Add(0);
                player.GetComponent<Tank>().tree.Add(other.gameObject);

            }
            AudioSource.PlayClipAtPoint(boomSound, transform.position);
            Instantiate(boomParticle, transform.position, new Quaternion());
            Destroy(this.gameObject);
        }  
        //機關槍子彈
        else if (name == "SmallBullet(Clone)" && (other.tag == "Terrain" || other.tag == "AI"))
        {
            if (other.tag == "AI")
                other.gameObject.GetComponentInParent<AITank>().health -= 1;
            Instantiate(boomParticle, transform.position, new Quaternion());
            Destroy(this.gameObject);
        }
        //火箭子彈
        else if (name == "RocketBullet(Clone)" && (other.tag == "Terrain" || other.tag == "AI"))
        {
            if (other.tag == "AI")
                other.gameObject.GetComponentInParent<AITank>().health -= 40;
            Instantiate(boomParticle, transform.position, new Quaternion());
            Destroy(this.gameObject);
        }

    }
}
