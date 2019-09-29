using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITank : MonoBehaviour {
    //Item
    public GameObject[] Item;
    //AI
    GameObject aiGenerate;
    bool deathCheck = false;
    //Nav
    NavMeshAgent nav;
    GameObject player;
    //Tank
    public GameObject mainWeapon;
    public GameObject firePoint;
    public int health;
    //Shoot
    public GameObject bulletType;
    public float shootSpeed = 1;
    public float coolDownSet;
    float coolDown;
    public Vector3 playerOffset;
    //Material
    Transform[] children;
    MeshRenderer[] meshRender;
    SkinnedMeshRenderer[] skinMeshRender;
    Material material;
    float t;
    // Use this for initialization
    void Start () {       
        player = GameObject.Find("PlayerTank");
        aiGenerate = GameObject.Find("AITankGenerate");
        nav = GetComponent<NavMeshAgent>();
        children = gameObject.GetComponentsInChildren<Transform>();
        meshRender = new MeshRenderer[children.Length];
        skinMeshRender = new SkinnedMeshRenderer[children.Length];
        t = 0;         
    }

    // Update is called once per frame
    void Update()
    {
        //AI--------------------------------------------------
        health = Mathf.Clamp(health, 0, 100);
        //存活才做動作       
        if(health>0)
        {
            //直接旋轉車身至玩家
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, transform.up);
            nav.SetDestination(player.transform.position);
            //停下來就開火
            if (nav.velocity == Vector3.zero && health > 0)
            {

                if (coolDown == coolDownSet)
                {
                    coolDown = 0;

                    GameObject bullet = Instantiate(bulletType, firePoint.transform.position, new Quaternion());
                    bullet.GetComponent<Rigidbody>().AddForce((player.transform.position - transform.position + playerOffset).normalized * shootSpeed, ForceMode.Impulse);
                }
                //冷卻
                coolDown += Time.deltaTime;
                coolDown = Mathf.Clamp(coolDown, 0, coolDownSet);
            }
        }        
        //死亡
        else
        {
            //關掉碰撞
            GetComponent<BoxCollider>().enabled = false;
            //擊殺AI
            if (!deathCheck)
            {
                //掉落物品
                int rmd = Random.Range(0, Item.Length - 1);
                Instantiate<GameObject>(Item[rmd], transform.position+new Vector3(0,1,0), Item[rmd].transform.rotation);
                //AIGenerate
                aiGenerate.GetComponent<AITankGenerate>().killAI++;
                aiGenerate.GetComponent<AITankGenerate>().totalAI--;
                deathCheck = true;
            }            
            //死亡動畫
            for (int i = 0; i < children.Length; i++)
            {
                meshRender[i] = children[i].GetComponentInChildren<MeshRenderer>();
                skinMeshRender[i] = children[i].GetComponentInChildren<SkinnedMeshRenderer>();
                for (int j = 0; j < meshRender.Length; j++)
                {
                    if (meshRender[j] != null)
                    {
                        material = meshRender[j].materials[0];
                        material.color = new Color(material.color.r, material.color.g, material.color.b, Mathf.Lerp(1f,0f,t));
                    }
                    else if(skinMeshRender[j]!=null)
                    {
                        material = skinMeshRender[j].materials[0];
                        material.color = new Color(material.color.r, material.color.g, material.color.b, Mathf.Lerp(1f, 0f, t));
                    }
                }
            }
            t += 0.2f*Time.deltaTime;
            //動畫跑完Destroy
            if(t==1)
                Destroy(this.gameObject);
        }

    }

}
