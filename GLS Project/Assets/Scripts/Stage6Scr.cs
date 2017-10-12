using UnityEngine;
using System.Collections;

public class Stage6Scr : MonoBehaviour {

	// 変数宣言
	int m_score;
	float m_cnt;
	Color m_color;


	/* --------------------------------------------------
	 * @パラメータ初期化
	*/
	void Start () {

		m_score = 0;
		m_cnt = 1.0f;
		m_color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		ColorControl ();
	}



	/* --------------------------------------------------
	 * @毎フレーム更新
	*/
	void Update () {

		// スコアの取得
		m_score = GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_score;

		ScrollControl ();

		if (m_score >= 25) {
			// 遷移開始

			// カウントの減算
			m_cnt -= 0.01f;

			// アルファ値の取得
			m_color.a = m_cnt;

			// 「カラーコントロール」の呼び出し
			ColorControl ();

			if (m_cnt <= 0.0f) {
			
				// デストロイ
				DestroyObject (this.gameObject);
			}
		}
	}



	/* --------------------------------------------------
	 * @背景スクロール
	*/
	void ScrollControl(){

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



	/* --------------------------------------------------
	 * @カラーコントロール
	*/
	void ColorControl(){
		
		// 手前
		transform.FindChild ("Belt_001").transform.FindChild ("1").GetComponent<SpriteRenderer> ().color = m_color;
		transform.FindChild ("Belt_001").transform.FindChild ("1.1").GetComponent<SpriteRenderer> ().color = m_color;
		transform.FindChild ("Belt_001").transform.FindChild ("1.2").GetComponent<SpriteRenderer> ().color = m_color;
		// 中間
		transform.FindChild ("Belt_002").transform.FindChild ("2").GetComponent<SpriteRenderer> ().color = m_color;
		transform.FindChild ("Belt_002").transform.FindChild ("2.1").GetComponent<SpriteRenderer> ().color = m_color;
		transform.FindChild ("Belt_002").transform.FindChild ("2.2").GetComponent<SpriteRenderer> ().color = m_color;
		// 奥
		transform.FindChild ("Belt_003").transform.FindChild ("3").GetComponent<SpriteRenderer> ().color = m_color;
		transform.FindChild ("Belt_003").transform.FindChild ("3.1").GetComponent<SpriteRenderer> ().color = m_color;
		transform.FindChild ("Belt_003").transform.FindChild ("3.2").GetComponent<SpriteRenderer> ().color = m_color;
		transform.FindChild ("Belt_004").transform.FindChild ("4").GetComponent<SpriteRenderer> ().color = m_color;
		transform.FindChild ("Belt_005").transform.FindChild ("5").GetComponent<SpriteRenderer> ().color = m_color;
		transform.FindChild ("Belt_006").transform.FindChild ("6").GetComponent<SpriteRenderer> ().color = m_color;
		transform.FindChild ("Belt_007").transform.FindChild ("7").GetComponent<SpriteRenderer> ().color = m_color;
		// 最奥
		transform.FindChild ("Belt_008").transform.FindChild ("8").GetComponent<SpriteRenderer> ().color = m_color;
		transform.FindChild ("Belt_009").transform.FindChild ("9").GetComponent<SpriteRenderer> ().color = m_color;
		transform.FindChild ("Belt_010").transform.FindChild ("10").GetComponent<SpriteRenderer> ().color = m_color;
	}
}
