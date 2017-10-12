using UnityEngine;
using System.Collections;

public class StageScr : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		// 現在座標の取得
		Vector3 pos = transform.position;

		// カメラ座標の取得
		Vector3 cam_pos = GameObject.FindWithTag ("MainCamera").transform.position;

		// 手前
		transform.FindChild ("Belt_001").transform.position = new Vector3(cam_pos.x * -3.0f, pos.y, pos.z);
		// 中間
		transform.FindChild ("Belt_002").transform.position = new Vector3(cam_pos.x * -1.0f, pos.y, pos.z);
		// 奥
		transform.FindChild ("Belt_003").transform.position = new Vector3(cam_pos.x * -0.75f, pos.y, pos.z);
		transform.FindChild ("Belt_004").transform.position = new Vector3(cam_pos.x * -0.75f, pos.y, pos.z);
		transform.FindChild ("Belt_005").transform.position = new Vector3(cam_pos.x * -0.25f, pos.y, pos.z);
		transform.FindChild ("Belt_006").transform.position = new Vector3(cam_pos.x * 0.1f, pos.y, pos.z);
		transform.FindChild ("Belt_007").transform.position = new Vector3(cam_pos.x * 0.2f, pos.y, pos.z);
		// 最奥
		transform.FindChild ("Belt_008").transform.position = new Vector3(cam_pos.x * 0.2f, pos.y, pos.z);
		transform.FindChild ("Belt_009").transform.position = new Vector3(cam_pos.x * 0.2f, pos.y, pos.z);
		transform.FindChild ("Belt_010").transform.position = new Vector3(cam_pos.x * 0.2f, pos.y, pos.z);
	}
}
