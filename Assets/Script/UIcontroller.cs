using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour {

    public Text SubAmmo;
    public Text MainReload;
    public Text Lose;
    public GameObject PlayerTank;
    public RawImage[] WeaponSlot = new RawImage[3];
    public Image[] HealthBar = new Image[2];
    public Camera PlayerView;
    private Tank PlayerStat;
    private int MaxiumHealth = 100;
    private bool ViewMode;
    void HealthBarUpdate()
    {
        if(PlayerStat.health> MaxiumHealth)
        {
            HealthBar[1].transform.localScale=new Vector3(
                HealthBar[0].transform.localScale.x, HealthBar[0].transform.localScale.y, HealthBar[0].transform.localScale.z);
        }
        else if(PlayerStat.health > 0)
        {
            HealthBar[1].transform.localScale = new Vector3(HealthBar[0].transform.localScale.x* PlayerStat.health/MaxiumHealth, 
                HealthBar[0].transform.localScale.y, HealthBar[0].transform.localScale.z);
        }
        else
        {
            HealthBar[1].transform.localScale = new Vector3(
                0.0f, HealthBar[0].transform.localScale.y, HealthBar[0].transform.localScale.z);
            Lose.enabled = true;
        }
    }
    void SwitchView()
    {
        if(ViewMode)
        {
            PlayerView.transform.localPosition = new Vector3(0.0f, 2.0f, 2.0f);
        }
        else
        {
            PlayerView.transform.localPosition = new Vector3(0.0f, 5.0f, -6.0f);
        }
    }

    // Use this for initialization
    void Start () {
        PlayerStat = PlayerTank.GetComponent<Tank>();
        Lose.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        SubAmmo.text = "<color=white>" + PlayerStat.subWeapon[PlayerStat.subWeaponIndex].name + ":</color>";
        if (PlayerStat.armo[PlayerStat.subWeaponIndex] <= 10)
        {
            SubAmmo.color = Color.red;
        }
        else if (PlayerStat.armo[PlayerStat.subWeaponIndex] <= 25)
        {
            SubAmmo.color = Color.yellow;
        }
        else
        {
            SubAmmo.color = Color.green;
        }
        SubAmmo.text +=PlayerStat.armo[PlayerStat.subWeaponIndex].ToString();

        {
            if(PlayerStat.coolDown == PlayerStat.coolDownSet)
            {
                MainReload.text = "<color=green>Ready!</color>";
            }
            else
            {
                MainReload.text = "<color=red>Reloading</color>";
            }
        }

        {
            for(int i=0;i< WeaponSlot.Length;i++)
            {
                if(i == PlayerStat.subWeaponIndex)
                {
                    if(PlayerStat.subCoolDown[i] == PlayerStat.subCoolDownSet[i])
                    {
                        WeaponSlot[i].color = Color.green;
                    }
                    else
                    {
                        WeaponSlot[i].color = Color.red;
                    }
                }
                else
                {
                    WeaponSlot[i].color = Color.grey;
                }
            }
        }
        HealthBarUpdate();
        if(Input.GetKeyDown(KeyCode.F))
        {
            ViewMode = !ViewMode;
            SwitchView();
        }
    }
}
