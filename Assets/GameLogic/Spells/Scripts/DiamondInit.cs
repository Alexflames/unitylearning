﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondInit : MonoBehaviour, SpellInit {
    public GameObject diamond;
    private Transform ownerTransform;
    private string[] aliases = {"diamond", "bullet"};
    // Use this for initialization
    void Start () {
        this.gameObject.GetComponent<SpellCreating>().addSpell(aliases,this);
    }

    Vector3 makeSpellSpawnPos (Vector3 adder, Transform owner)
    {
        return owner.position + adder;
    }

    private IEnumerator repeatCast (float wait, Vector3 spellSpawnPos, Quaternion rotation) {
        yield return new WaitForSeconds (wait);
        Instantiate(diamond, spellSpawnPos, rotation);
    }

    public void cast(SpellModificator sm)
    {
        ownerTransform = this.gameObject.transform;
        Vector3 spellSpawnPosition = makeSpellSpawnPos (ownerTransform.forward * 2.0F, ownerTransform);
        spellSpawnPosition += new Vector3 (0, 0.5f, 0);
        GameObject d = Instantiate (diamond, spellSpawnPosition, ownerTransform.rotation);
        if (sm != null && sm is RepeatModificator) {
             float wait = ((RepeatModificator)sm).wait;
             StartCoroutine (repeatCast (wait, spellSpawnPosition, ownerTransform.rotation));
        }
        else d.GetComponent<DiamondLogic>().ApplyModificator (sm);
        
    }
}
