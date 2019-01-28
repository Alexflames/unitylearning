﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetQuickModificator : NetAbstractSpellModificator
{
	public float speedFactor = 1.0f;
	public float weakFactor = 1.0f;
	public override string Name
	{
		get
		{
			return "quick";
		}
	}
}
