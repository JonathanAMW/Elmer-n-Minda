using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartSystem : MonoBehaviour
{
    [SerializeField] GameObject[] gmbrHati;

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
		for (int i = 0; i < gmbrHati.Length; i++)
		{
			if (karakterStat.darah > i)
			{
				gmbrHati[i].SetActive(true);
			}else
				gmbrHati[i].SetActive(false);
		}

		if (karakterStat.darah < 0)
			karakterStat.darah = 0;
		else if (karakterStat.darah > gmbrHati.Length)
			karakterStat.darah = gmbrHati.Length;
	}

	// Update is called once per frame

}
