using UnityEngine;
using System.Collections;

public class MainCameraScr : MonoBehaviour {



	/* --------------------------------------------------
	 * @パラメータ初期化
	*/
	void Start () {
		Application.targetFrameRate = 60;
	}



	/* --------------------------------------------------
	 * @毎フレーム更新
	*/
	void Update () {

		// シーンストッパー
		float scene = GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_scene;
		if (scene != 0 && scene != 1 && scene != 11 && scene != 12) {


			int st = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScr> ().m_move_st;
			if (st == 1 || st == 3) {

				if (Input.GetMouseButton (0)) {
					// タップしている間

					// マウスのスクリーン座標取得
					Vector3 mouse_screen_pos = Input.mousePosition;
					mouse_screen_pos.z = transform.position.z - Camera.main.transform.position.z;
					// マウスのワールド座標取得
					Vector3 mouse_world_pos = Camera.main.ScreenToWorldPoint (mouse_screen_pos);

					// カメラは追いかける
					Vector3 vec = (mouse_world_pos - transform.position).normalized;
					vec *= 0.05f;
					transform.position += new Vector3 (vec.x, vec.y, 0.0f);
				}
			
			} else if (st == 2 || st == 4 || st == 5) {
				
				// カメラは追いかける
				Vector3 vec = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
				vec *= 0.1f;
				transform.position += new Vector3 (vec.x, vec.y, 0.0f);
			}
				
			// ----------
			// 移動範囲制限
			// ----------
			// 左
			if (transform.position.x < -6.2f) {
				transform.position = new Vector3 (-6.2f, transform.position.y, transform.position.z);
			}
			// 右
			if (transform.position.x > 6.2f) {
				transform.position = new Vector3 (6.2f, transform.position.y, transform.position.z);
			}
			// 上
			if (transform.position.y < 0.0f) {
				transform.position = new Vector3 (transform.position.x, 0.0f, transform.position.z);
			}
			// 下
			if (transform.position.y > 13.8f) {
				transform.position = new Vector3 (transform.position.x, 13.8f, transform.position.z);
			}
			// ----------
		}
	}
}
