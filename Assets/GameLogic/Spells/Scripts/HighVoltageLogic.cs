﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighVoltageLogic : MonoBehaviour {
    float timeToAppear;
    float timeToFade;
    public int attackPower;
    private double attackFactor = 1.0;
    private double speedFactor = 1.0;
    public GameObject part1;
    public GameObject part2;
    private GameObject part1_1;
    private GameObject part1_2;
    private GameObject part2_1;
    private GameObject part2_2;
    private GameObject owner;
    private bool greatMod = false;
    private float sF = 1.0f;//scale factor for great mod

    public void ApplyModificator (SpellModificator sm)
    {
        if (sm == null) return;
        if(sm is StrongModificator)
        {
                attackFactor =  (((StrongModificator)sm).factor);
        }
        if(sm is GreatModificator)
        {
                greatMod = true;
		sF = (float)(((GreatModificator)sm).scaleFactor);
                gameObject.transform.localScale += new Vector3 (sF - 1.0f, 0, sF - 1.0f) ;
        }
        if(sm is QuickModificator)
        {
                QuickModificator qm = (QuickModificator)sm;
                attackFactor = 1 / qm.weakFactor;
                speedFactor = qm.speedFactor;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name != "WaterSplash(Clone)")
        {
            if (collision.gameObject.CompareTag("Destroyable"))
            {   // Объект, в который врезались, уничтожаемый?
                
                if (collision.gameObject != owner)
                {
                    Mortal HP = collision.gameObject.GetComponent<Mortal>();
                    HP.lowerHP((int)(attackPower * attackFactor));
                }
            }
            else if (!collision.gameObject.CompareTag("Spell"))
            {
                Object.Destroy(gameObject);
            }
        }
    }

	// Use this for initialization
	void Awake () {
        timeToAppear = 0.25f;
        timeToFade = 0.35f;
        part1_1 = part1.transform.Find("HighVoltage3-1").gameObject;
        part1_2 = part1.transform.Find("HighVoltage3-2").gameObject;
        part2_1 = part2.transform.Find("HighVoltage3-3").gameObject;
        part2_2 = part2.transform.Find("HighVoltage3-4").gameObject;
        part1.SetActive(false);
        part2.SetActive(false);
        part1_1.SetActive(false);
        part2_1.SetActive(false);
        part1_2.SetActive(false);
        part2_2.SetActive(false);
        if(greatMod){
              //part1_1.transform.localScale += new Vector3 (sF - 1.0f, 0, sF - 1.0f) ;
              //part1_2.transform.localScale += new Vector3 (sF - 1.0f, 0, sF - 1.0f) ;
              //part2_2.transform.localScale += new Vector3 (sF - 1.0f, 0, sF - 1.0f) ;
              //part2_1.transform.localScale += new Vector3 (sF - 1.0f, 0, sF - 1.0f) ;
        }
    }
	
	// Update is called once per frame
    void Update () {
        timeToAppear -= Time.deltaTime * (float)speedFactor;
        timeToFade -= Time.deltaTime * (float)speedFactor;
        if (!part1.activeSelf && timeToAppear < 0.1f)
        {
            part1.SetActive(true);
            part2.SetActive(true);
        }
        else if (!part1_1.activeSelf && timeToAppear < 0.0f)
        {
            part1_1.SetActive(true);
            part2_1.SetActive(true);
            part1_2.SetActive(true);
            part2_2.SetActive(true);
        }
        else if (timeToFade < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetOwner(GameObject thatOwns)
    {
        owner = thatOwns;
    }
}
