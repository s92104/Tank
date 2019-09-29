using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITankGenerate : MonoBehaviour {
    //Nav
    public GameObject aiTank;
    public GameObject[] position;
    //CoolDown
    public int minCoolDownSet;
    public int maxCoolDownSet;
    int coolDownSet;
    float[] coolDown;
    //MaxAI
    public int maxAISet;
    public int minAISet;
    int maxAI;
    public int totalAI;
    public int killAI=0;      
    // Use this for initialization
    void Start () {
        //目前冷卻
        coolDownSet = maxCoolDownSet;
        //重生點冷卻
        coolDown = new float[position.Length];
        for (int i = 0; i < position.Length; i++)
            coolDown[i] = coolDownSet;
        //最大AI數
        maxAI = minAISet;        
    }
	
	// Update is called once per frame
	void Update () {
        //冷卻
        for (int i = 0; i < coolDown.Length; i++)
        {
            coolDown[i] += Time.deltaTime;
            coolDown[i] = Mathf.Clamp(coolDown[i], 0, coolDownSet);
        }
        //增加坦克
        if (totalAI<maxAI)
        {
            List<int> tmp = new List<int>();
            //確認哪裡冷卻好了
            for (int i = 0; i < position.Length; i++)
                if (coolDown[i] == coolDownSet)
                    tmp.Add(i);
            //冷卻好的>=需要的AI
            if(tmp.Count>=(maxAI-totalAI))
            {
                for(int i=0;i< maxAI - totalAI; i++)
                {
                    int rmd = Random.Range(0, tmp.Count - 1);
                    Instantiate<GameObject>(aiTank, position[tmp[rmd]].transform.position, new Quaternion());
                    coolDown[tmp[rmd]] = 0;
                    tmp.RemoveAt(rmd);                   
                }
                totalAI = maxAI;
            }
            //冷卻好的不夠
            else
            {
                for(int i=0;i<tmp.Count;i++)
                {
                    Instantiate<GameObject>(aiTank, position[tmp[i]].transform.position, new Quaternion());
                    coolDown[tmp[i]] = 0;
                    totalAI++;
                }
            }
        }
        //難度上升
        if (killAI >= maxAI)
        {
            //最大AI數+2
            if(maxAI<maxAISet)
                maxAI += 2;
            //重生冷卻-1
            if (coolDownSet > minCoolDownSet)
                coolDownSet--;
        }
    }
}
