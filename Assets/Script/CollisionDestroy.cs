using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDestroy : MonoBehaviour {
    public float destroyTime;
    float destroyCount;
    bool collisionCheck;
    // Use this for initialization
    void Start()
    {
        destroyCount = 0;
        collisionCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (collisionCheck)
            destroyCount += Time.deltaTime;
        if (destroyCount >= destroyTime)
            Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag=="Player")
        {
            collisionCheck = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }
}
