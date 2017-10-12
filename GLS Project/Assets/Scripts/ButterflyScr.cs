using UnityEngine;
using System.Collections;

public class ButterflyScr : MonoBehaviour {

	// 変数宣言



	/* --------------------------------------------------
	 * @パラメータ初期化
	*/
	void Start () {
	
	}



	/* --------------------------------------------------
	 * @毎フレーム更新
	*/
	void Update () {

		if (Input.GetMouseButton (0)) {
			// マウスをクリックしている間

			// マウスのスクリーン座標取得
			Vector3 mouse_screen_pos = Input.mousePosition;
			mouse_screen_pos.z = transform.position.z - Camera.main.transform.position.z;
			// マウスのワールド座標取得
			Vector3 mouse_world_pos = Camera.main.ScreenToWorldPoint (mouse_screen_pos);

			// 
			transform.position = mouse_world_pos;
		
		} 

		if (Input.GetMouseButtonUp (0)) {
			// マウスを離したならば

			// 
			transform.position =  new Vector3(0.0f, -50.0f, 0.0f);
		}
	}
}
