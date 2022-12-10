using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemTantangan : MonoBehaviour
{
	public List<DuniaTantangan> dataTantangan;
}

[System.Serializable]
public class DuniaTantangan
{
	public string nama;
	public bool isDuniaTerbuka;
	public List<Tantangan> listTantanganDunia;
}

[System.Serializable]
public class Tantangan
{
	public string nama, deksTantangan;
	public bool isSelesai, isLevelTerbuka;
}
