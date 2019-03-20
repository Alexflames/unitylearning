﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictionInit : AbstractSpellInit {
	private GameObject prediction;
	public GameObject spell;	   // Created prediction object
	private AudioSource audioSource;
	public AudioClip clockSound;
	private string[] aliases = { "prediction", "sibylla" };
	[Range(5, 20)]
	public float lastingTime = 8;

    // Use this for initialization
    protected override void Start () {
		this.gameObject.GetComponent<SpellCreating>().addSpell(this);
		audioSource = gameObject.GetComponent<AudioSource>();
        prediction = Resources.Load("Prediction_Fate", typeof(GameObject)) as GameObject;
    }
	
	public override void cast(string smName)
	{
		print("noice!");
		RaycastHit hit;

		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
		{
			Vector3 predictionDestination = hit.point;
			if (spell)
			{
				Destroy(spell);
			}
			spell = GameObject.Instantiate(prediction, transform.position + transform.forward, transform.rotation);
			Prediction_FateLogic spellLogic = spell.GetComponent<Prediction_FateLogic>();
			spellLogic.SetTimeLeft(lastingTime);
			spellLogic.SetOwner(gameObject);
			audioSource.Play();
		}
	}

	public override string Description {
		get {
			return "prediction";
		}
	}


	public override string[] Aliases {
		get {
			return aliases;
		}
	}
}