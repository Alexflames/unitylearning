﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondLogic : MonoBehaviour {

	public Rigidbody rb;
	public Collider colli;
	int layerMask;
	public int attackPower = 150;
	private float attackFactor = 1.0f;
	private float speedFactor = 1.0f;

	void Awake () {
		Destroy (this.gameObject, 5);
	}

	void Start () {
		rb = GetComponent<Rigidbody> ();
		colli = GetComponent<Collider> ();
	}

	public void ApplyModificator (SpellModificator sm)
	{
		if (sm == null) return;
		if(sm is StrongModificator)
		{
				attackFactor =  (((StrongModificator)sm).factor);
		}
		if(sm is GreatModificator)
		{
		float sF = ((GreatModificator)sm).scaleFactor;
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
			if (collision.gameObject.CompareTag("Destroyable"))
			{   // Объект, в который врезались, уничтожаемый?
			   Mortal HP = collision.gameObject.GetComponent<Mortal>();
			   HP.lowerHP((int)(attackPower * attackFactor));
			}
			if (!collision.gameObject.CompareTag("Spell"))
			{
				Object.Destroy(gameObject);
			}
	}

	void FixedUpdate () {
		rb.AddForce (transform.forward * 1.5f * speedFactor);
	}
}
