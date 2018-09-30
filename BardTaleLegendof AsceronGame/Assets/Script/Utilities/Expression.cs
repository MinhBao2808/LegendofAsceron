using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Expression  {
	public static float GetExpExpression (float x) {
		return Mathf.Pow(x, 3.0f) + (25 * Mathf.Pow(x, 2.0f)) - (20 * x) - 6;
	}
}
