using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainKamera : MonoBehaviour
{
	public GameObject tujuan;
	public GameObject tujuanA, tujuanB;

	private void LateUpdate()
	{
		transform.position = new Vector3(tujuan.transform.position.x, tujuan.transform.position.y + 1.5f, transform.position.z);
	}
}
