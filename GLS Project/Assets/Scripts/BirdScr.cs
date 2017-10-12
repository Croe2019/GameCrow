using UnityEngine;
using System.Collections;

public class BirdScr : MonoBehaviour {

	// 変数宣言
	int m_st;									// ステータス
	int m_cnt;									// カウント
	float m_speed;								// スピード
	Vector3 m_rot;								// 向き

	public float m_speed_setting;				// 
	public float m_pursuit_speed_setting;
	public int m_pursuit_cnt_setting;			
	public float m_search_range_setting;		//
	float m_pos_y;							// 

	// SE
	AudioSource m_audio_source;



	/* --------------------------------------------------
	 * @パラメータ初期化
	*/
	void Start () {
		
		m_st = 0;
		m_cnt = 0;
		m_speed = m_speed_setting;
		m_rot = new Vector3 (0.0f, 0.0f, 0.0f);
		m_pos_y = transform.position.y;

		// SE
		m_audio_source = GetComponent<AudioSource>();
	}



	/* --------------------------------------------------
	 * @毎フレーム更新
	*/
	void Update () {
	
		// シーンストッパー
		float scene = GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_scene;
		if (scene != 0 && scene != 1) {

			// 現在座標の取得
			Vector3 pos = transform.position;

			// タグ「プレイヤー」の現在座標の取得
			Vector3 p_pos = GameObject.FindGameObjectWithTag ("Player").transform.position;

			// ------------------------------
			// ステータス遷移
			// ------------------------------
			if (m_st == 0) {
				// デフォルト

				m_st = 1;

			} else if (m_st == 1) {
				// 移動

				float x = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScr> ().m_move_dist;
				transform.position += new Vector3 (x * -0.5f, 0.0f, 0.0f);

				// ----------
				// 移動処理
				// ----------
				m_rot.z = 0.0f;

				if (transform.position.x > 15.0f) {

					// 
//					m_audio_source.PlayOneShot(m_audio_source.clip);
				
					// 
					m_rot.y = 180.0f;

				} else if (transform.position.x < -15.0f) {

					// 
//					m_audio_source.PlayOneShot(m_audio_source.clip);

					// 
					m_rot.y = 0.0f;

				}

				// 高さ調節
				if (m_pos_y > pos.y) {

					// 
					transform.position += new Vector3 (0.0f, 0.01f, 0.0f);
				
				} else if (m_pos_y < pos.y) {

					// 
					transform.position -= new Vector3 (0.0f, 0.01f, 0.0f);
				}
				// ----------

				// 左向き
				if (m_rot.y == 0.0f) {

					// サーチ範囲に引っかかったならば
					if (pos.x + m_search_range_setting < p_pos.x || pos.x > p_pos.x
						|| pos.y + m_search_range_setting * 0.5f < p_pos.y || pos.y > p_pos.y + m_search_range_setting * 0.5f) {

					} else {

						// 
						float p1 = pos.x - p_pos.x;
						float p2 = pos.y - p_pos.y;
						float rad = Mathf.Atan2 (p2, p1);
						float deg = rad *= Mathf.Rad2Deg;
						deg += 180.0f;

						// 
						m_rot.z = deg;

						// 
						m_speed = m_speed_setting;

						// ステータスをサーチ＆デストロイに
						m_st = 2;
					}
				}

				// 右向き
				if (m_rot.y == 180.0f) {

					// サーチ範囲に引っかかったならば
					if (pos.x < p_pos.x || pos.x > p_pos.x + m_search_range_setting
					    || pos.y + m_search_range_setting * 0.5f < p_pos.y || pos.y > p_pos.y + m_search_range_setting * 0.5f) {

					} else {

						// 
						float p1 = pos.x - p_pos.x;
						float p2 = pos.y - p_pos.y;
						float rad = Mathf.Atan2 (p2, p1);
						float deg = rad *= Mathf.Rad2Deg;
						deg += 180.0f;

						// 
						if (deg > 0.0f) {
							deg *= -1.0f;
							deg += 180.0f;
						}
						if (deg <= 0.0f) {
							deg += 360.0f;
						}
						m_rot.z = deg;

						// 
						m_speed = m_speed_setting;

						// ステータスをサーチ＆デストロイに
						m_st = 2;
					}
				}

			} else if (m_st == 2) {
				// サーチ＆デストロイ

				// 
				m_cnt++;

				// 
				m_speed = m_speed_setting * m_pursuit_speed_setting;

				// 
				if (m_cnt > m_pursuit_cnt_setting) {

					// 
					m_speed = m_speed_setting;
					m_cnt = 0;
					m_st = 0;
				}
			}

			// 向きの更新
			transform.rotation = Quaternion.Euler (m_rot);

			// 移動の更新
			transform.Translate (Vector3.left * -m_speed);
			// ------------------------------



		}
	}

	/* --------------------------------------------------
	 * @衝突判定
	*/
	void OnTriggerEnter2D(Collider2D collider){

	}
}
