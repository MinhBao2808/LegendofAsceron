using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Expression  {
	public static float GetExpExpression (float x) {
		return ((4/3) * Mathf.Pow(x, 3.0f)) - (15 * Mathf.Pow(x, 2.0f)) + (80 * x) - 102;
	}

	public static float SkillPATK () {
		return 1;
	}
}
