using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
	[SerializeField] Animator anim;

	[Header("Serang Biasa")]
	[SerializeField] Transform atkPointNormal;
	[SerializeField] float atkRangeNormal = 0.5f;

	[Header("Serang di Udara")]
	[SerializeField] Transform atkPointAir;
	[SerializeField] float atkRangeAir = 0.5f;

	[Header("Delay Nyerang")]
	[SerializeField] float atkRate = 2f;
	private float nextAtkTime = 0f;

	[Header("Layer Musuh")]
	[SerializeField] LayerMask layerMusuh;

	[Header("Referensi")]
	CharStat karakterStat;

	private void Start()
	{
		karakterStat = GameObject.Find("StatPemain").GetComponent<CharStat>();
	}
	private void Update()
	{
		if (DialogueBoxManager.isActive || DialogueBoxManager.isLoop) return; //mencegah pemain nyerang saat dialog
		
		if(Time.time >= nextAtkTime)
		{
			if (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0))
			{
				nextAtkTime = Time.time + 1f / atkRate;
				anim.SetTrigger("Serang");
			}
		}
	}

	public void SerangBiasa()
	{
		//deteksi musuh dalam lingkaran (range)
		Collider2D[] hitMusuh = Physics2D.OverlapCircleAll(atkPointNormal.position, atkRangeNormal, layerMusuh);

		//ngedmg musuh
		foreach (Collider2D musuh in hitMusuh)
		{
			//Debug.Log("Nge hit " + musuh.name);
			musuh.GetComponent<Enemy>().KenaDamage(karakterStat.serangan);
		}
	}

	public void SerangUdara()
	{
		//anim.SetTrigger("Serang");

		//deteksi musuh dalam lingkaran (range)
		Collider2D[] hitMusuh = Physics2D.OverlapCircleAll(atkPointAir.position, atkRangeAir, layerMusuh);

		//ngedmg musuh
		foreach (Collider2D musuh in hitMusuh)
		{
			//Debug.Log("Nge hit " + musuh.name);
			musuh.GetComponent<Enemy>().KenaDamage(karakterStat.serangan);
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (atkPointNormal != null)
			Gizmos.DrawWireSphere(atkPointNormal.position, atkRangeNormal);
		if(atkPointAir != null)
			Gizmos.DrawWireSphere(atkPointAir.position, atkRangeAir);
	}
}
