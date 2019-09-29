using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {
    //Tank
    public float moveSpeed=1;
    public float rotateSpeed=1;
    public float mainWeaponRotateSpeed = 1;
    public GameObject mainWeapon;
    public GameObject firePoint;
    public int health;
    //Shoot
    public GameObject bulletType;
    public float shootSpeed = 1;
    public float coolDownSet;
    public float coolDown;
    //SubWeapon
    public int subWeaponIndex=0;
    public GameObject[] subWeapon;
    public bool[] subWeaponLock;
    public int[] armo;
    public float[] subCoolDownSet;
    public float[] subCoolDown;
    bool subWeaponShoot;
    public GameObject[] subBulletType;
    public float[] subShootTimeSet;
    //Item
    public GameObject pick;
    //Tree
    public List<GameObject> fireRec = new List<GameObject>();
    public List<float> fireTime=new List<float>();
    public List<GameObject> tree = new List<GameObject>();
    // Use this for initialization
    void Start () {

        //血量
        health = 100;
        //主武器
        coolDown = coolDownSet;
        //副武器
        for (int i = 0; i < subCoolDown.Length; i++)
            subCoolDown[i] = subCoolDownSet[i];       
	}
    // Update is called once per frame
    void Update () {
        //血量
        health = Mathf.Clamp(health, 0, 100);
        //移動
        if (Input.GetKey(KeyCode.W))           
            transform.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * moveSpeed,ForceMode.Force);
        if (Input.GetKey(KeyCode.S))
            transform.GetComponent<Rigidbody>().AddForce(-transform.forward.normalized * moveSpeed, ForceMode.Force);
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(new Vector3(0, -rotateSpeed, 0));
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(new Vector3(0, rotateSpeed, 0));
        //砲管
        if (Input.GetKey(KeyCode.UpArrow))
            mainWeapon.transform.Rotate(new Vector3(-mainWeaponRotateSpeed, 0, 0));
        if (Input.GetKey(KeyCode.DownArrow))
            mainWeapon.transform.Rotate(new Vector3(mainWeaponRotateSpeed, 0, 0));
        if (Input.GetKey(KeyCode.LeftArrow))
            mainWeapon.transform.Rotate(new Vector3(0, -mainWeaponRotateSpeed, 0), Space.World);
        if (Input.GetKey(KeyCode.RightArrow))
            mainWeapon.transform.Rotate(new Vector3(0, mainWeaponRotateSpeed, 0), Space.World);
        //加速
        if (Input.GetKeyDown(KeyCode.LeftShift))
            moveSpeed *= 2;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            moveSpeed /= 2;
            //翻車回復
            if (Input.GetKeyDown(KeyCode.F1))
            transform.rotation = new Quaternion();
        //主武器--------------------------------------------------
        //發射
        if (Input.GetKeyDown(KeyCode.Space) && coolDown==coolDownSet && health>0)
        {
            coolDown = 0;
            GameObject bullet = Instantiate(bulletType,firePoint.transform.position,new Quaternion());
            bullet.GetComponent<Rigidbody>().AddForce(-mainWeapon.transform.up*shootSpeed, ForceMode.Impulse);
        }
        //冷卻
        coolDown += Time.deltaTime;
        coolDown = Mathf.Clamp(coolDown, 0, coolDownSet);
        //副武器--------------------------------------------------
        //切換
        if (Input.GetKeyDown(KeyCode.Z)|| Input.GetKeyDown(KeyCode.C))
        {         
            subWeapon[subWeaponIndex].SetActive(false);
            if (Input.GetKeyDown(KeyCode.Z))
            {
                subWeaponIndex--;
                if (subWeaponIndex < 0)
                    subWeaponIndex += subWeapon.Length;
            }
            else
            {
                subWeaponIndex++;
                if (subWeaponIndex >= subWeapon.Length)
                    subWeaponIndex -= subWeapon.Length;
            }
            if (subWeaponLock[subWeaponIndex] && subWeaponIndex!=2)
                subWeapon[subWeaponIndex].SetActive(true);
        }
        //發射
        if(Input.GetKeyDown(KeyCode.X) && subWeaponLock[subWeaponIndex] && subCoolDown[subWeaponIndex] == subCoolDownSet[subWeaponIndex] && armo[subWeaponIndex]>0 && health>0)
        {
            subWeaponShoot = true;
            subCoolDown[subWeaponIndex] = 0;
        }
        //實際發射
        if(subWeaponShoot)
        {
            //機關槍
            if(subWeaponIndex==0)
            {
                //檢查射擊時間
                if(subCoolDown[subWeaponIndex]<subShootTimeSet[subWeaponIndex] && armo[subWeaponIndex]>0)
                {
                    float rmd = Random.Range(-2f, 2f);
                    print(rmd);
                    GameObject tmpBullet = Instantiate<GameObject>(subBulletType[subWeaponIndex], transform.position+transform.right.normalized*rmd, Quaternion.identity);
                    tmpBullet.GetComponent<ConstantForce>().force = transform.forward.normalized * 50;
                    armo[subWeaponIndex]--;
                }
                else
                    subWeaponShoot = false;
            }
            //飛彈
            else if(subWeaponIndex==1)
            {
                GameObject tmpBullet = Instantiate<GameObject>(subBulletType[subWeaponIndex], subBulletType[subWeaponIndex].transform.position, subBulletType[subWeaponIndex].transform.rotation);
                tmpBullet.SetActive(true);
                tmpBullet.GetComponent<ConstantForce>().force = transform.forward.normalized * 50;
                armo[subWeaponIndex]--;
                subWeaponShoot = false;
            }
            //防護罩
            else if(subWeaponIndex==2)
            {
                if (subCoolDown[subWeaponIndex] < subShootTimeSet[subWeaponIndex])
                    subWeapon[subWeaponIndex].SetActive(true);
                else
                {
                    subWeapon[subWeaponIndex].SetActive(false);
                    armo[subWeaponIndex]--;
                    subWeaponShoot = false;
                }
            }
        }
        //冷卻
        subCoolDown[subWeaponIndex] += Time.deltaTime;
        subCoolDown[subWeaponIndex] = Mathf.Clamp(subCoolDown[subWeaponIndex], 0, subCoolDownSet[subWeaponIndex]);
        //樹
        if(fireRec.Count>0)
        {
            for(int i=0;i<fireRec.Count;i++)
            {
                fireTime[i] += Time.deltaTime;
                fireRec[i].transform.localScale += new Vector3(0, 0, 0.8f * Time.deltaTime);
                if(fireTime[i]>5)
                {
                    Destroy(fireRec[i].gameObject);
                    Destroy(tree[i].gameObject);
                    tree.RemoveAt(i);
                    fireRec.RemoveAt(i);
                    fireTime.RemoveAt(i);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Item")
        {
            //機關槍
            if (other.name[0] == 'M')
                armo[0] += 200;
            //火箭
            else if (other.name[0] == 'R')
                armo[1] += 10;
            //防護罩
            else if (other.name[0] == 'P')
                armo[2] += 10;
            Instantiate<GameObject>(pick, other.transform.position, pick.transform.rotation);
            Destroy(other.gameObject);
        }
    }
   
}
