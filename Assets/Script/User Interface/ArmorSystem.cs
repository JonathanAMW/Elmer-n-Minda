using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSystem : MonoBehaviour
{
	[SerializeField] GameObject[] gmbrArmor;
	[SerializeField] CharStat karakterStat;

	private void Start()
	{
		karakterStat = GameObject.Find("StatPemain").GetComponent<CharStat>();
	}

	private void Update()
	{
		TampilNyawa();
	}

	void TampilNyawa()
	{
		for (int i = 0; i < gmbrArmor.Length; i++)
		{
			if (karakterStat.zirah > i)
			{
				gmbrArmor[i].SetActive(true);
			}
			else
				gmbrArmor[i].SetActive(false);
		}

		if (karakterStat.zirah < 0)
			karakterStat.zirah = 0;
		else if (karakterStat.zirah > gmbrArmor.Length)
			karakterStat.zirah = gmbrArmor.Length;
	}
}
