﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond2Init : AbstractSpellInit
{
    private GameObject diamond;
    private Transform ownerTransform;
    private string[] aliases = { "diamond", "bullet" };

    // Name in the current game session
    public string SessionName = "";
    private string currentModName;

    private float timeToCast = 0;
    public float startingTimeToCast = 10.0f;
    public int startingCharges = 6;
    private int charges = 0;

    private UIElementsController UIctrl;

    // Use this for initialization
    protected override void Start()
    {
        UIctrl = GameObject.Find("Canvas").GetComponent<UIElementsController>();
        SessionName = this.gameObject.GetComponent<NetSpellCreating>().addSpell(this);
        diamond = Resources.Load("Diamond2", typeof(GameObject)) as GameObject;
    }

    Vector3 makeSpellSpawnPos(Vector3 adder, Transform owner)
    {
        return owner.position + adder;
    }

    public override void RMBReact()
    {
        if (timeToCast > 0 && charges > 0)
        {
            charges--;
            CmdCast(currentModName);
        }
        if (charges == 0)
        {
            UIctrl.ActivateRMBTimer(false);
            timeToCast = 0;
        }
    }
    
    public void CmdCast(string smName)
    {
        SpawnLogic(smName);
    }

    public void SpawnLogic(string smName)
    {
        ownerTransform = this.gameObject.transform;
        Vector3 spellSpawnPosition = makeSpellSpawnPos(ownerTransform.forward * 2.0F, ownerTransform);
        spellSpawnPosition += new Vector3(0, 0.5f, 0);
        GameObject spawned = Instantiate(diamond, spellSpawnPosition, ownerTransform.rotation);

        RpcOtherStuff(spawned, smName);
    }
    
    public void RpcOtherStuff(GameObject spell, string smName)
    {
        gameObject.GetComponent<NetSpellCreating>().getModIfExists(smName);
        spell.GetComponent<NetDiamond2Logic>().ownerForward = transform.forward;
    }


    public override void cast(string smName)
    {
        currentModName = smName;
        charges = startingCharges;
        timeToCast = startingTimeToCast;
        UIctrl.ActivateRMBTimer(true);
    }

    void Update()
    {
        if (timeToCast > 0)
        {
            timeToCast -= Time.deltaTime;
            UIctrl.SetTimerSliderValue(timeToCast, startingTimeToCast);
            if (timeToCast < 0) UIctrl.ActivateRMBTimer(false);
        }

    }

    public override string Description
    {
        get
        {
            string translation = "";
            switch (SessionName)
            {
                case "diamond":
                    translation = "<color=#ffffffff>Алмаз</color>";
                    break;
                case "bullet":
                    translation = "<color=#ffffffff>Пуля</color>";
                    break;
                default:
                    break;
            }
            return translation + "(" + SessionName + ") " + "Создает крайне острый <color=#ffffffff>объект</color>. Использовать с осторожностью!";
        }
    }

    public override string[] Aliases
    {
        get
        {
            return aliases;
        }
    }
}
