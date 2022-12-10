using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
	public GameObject nextPos, nextStage, currentStage;

	public void GantiStage()
	{
		nextStage.SetActive(true);
		currentStage.SetActive(false);
	}
}
