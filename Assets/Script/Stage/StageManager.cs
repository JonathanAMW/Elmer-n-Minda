using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
	[SerializeField] GameObject[] balok;
	public int tambahBalok;

	private void Start()
	{
		for (int i = 0; i < balok.Length; i++)
		{
			balok[i].SetActive(false);
		}
	}

	public void NambahBalok(int tambah)
	{
		if (tambah > balok.Length)
			return;
		balok[tambah - 1].SetActive(true);
	}

	public void Undo()
	{
		if (tambahBalok == 0)
			return;
		tambahBalok--;
		balok[tambahBalok].SetActive(false);
	}
}
