using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStat : MonoBehaviour
{
	[Header("Sistem Life")]
	public int nyawa;

	[Header("Sistem Health")]
	public int darah;
	public int darahPenuh;
	public int darahMaks;
	//public int hati;

	[Header("Serangan & Armor")]
	public int serangan, seranganMaks;
	public int dmgSentuhMusuh, zirah, zirahPenuh, zirahMaks;

	[Header("Pergerakan")]
	public float kecepatanGerak;
	public float kekuatanLompat;

	[Header("Kondisi Karakter")]
	public bool isJalan;
	public bool isTdkHadapKanan, isGrounded, isPernahMati;
}
