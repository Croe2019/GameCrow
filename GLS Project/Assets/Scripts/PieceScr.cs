using UnityEngine;
using System.Collections;

public class PieceScr : MonoBehaviour {

	// 変数宣言
	public float m_speed;
	float m_rot;
	int m_dir;
	int m_cnt;
	public int m_wind;

	public int m_st;

	public bool m_is_tailpiece;



	/* --------------------------------------------------
	 * @パラメータ初期化
	*/
	void Start () {
	
		m_dir = 0;
		m_cnt = 0;
		m_is_tailpiece = false;

		float speed_max = GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_max;
		float speed_min = GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_min;
		m_speed = Random.Range(speed_max, speed_min);
		m_rot = m_speed * 100.0f;
		m_wind = GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_wind;
		int white_percent = GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_white_percent;
		int gold_percent = GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_gold_percent;
		int black_percent = GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_blak_percent;
		Color cl = GetComponent<SpriteRenderer> ().color;

		if (this.tag != "TailPiece") {
			int index = Random.Range (0, 100);
			if (index > 0 && index <= white_percent) {
				m_st = 1;
				cl = Color.white;
			} else if (index > white_percent && index <= white_percent + gold_percent) {
				m_speed *= 2.0f;
				m_st = 2;
				cl = Color.yellow;
			} else if (index > white_percent + gold_percent && index <= white_percent + gold_percent + black_percent) {
				m_st = 3;
				cl = Color.black;
			}
		} else {
			if (m_st == 1) {
				cl = Color.white;
			} else if (m_st == 2) {
				cl = Color.yellow;
			} else if (m_st == 3) {
				cl = Color.black;
			}
		}
		GetComponent<SpriteRenderer> ().color = cl;
	}



	/* --------------------------------------------------
	 * @毎フレーム更新
	*/
	void Update () {

		// ※シーンストッパー※
		int scene = GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_scene;
		if (scene != 0 && scene != 1 && scene != 12 && scene != 20) {

			// カウントの加算
			m_cnt++;

			if (m_is_tailpiece == false) {
				// プレイヤーに取得されていなかったならば

				float x = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScr> ().m_move_dist;
				transform.position += new Vector3 (x * -0.5f, 0.0f, 0.0f);

		
				// 落下させる
				transform.position -= new Vector3 (0.0f, m_speed, 0.0f);

				// 回転させる
				transform.Rotate (new Vector3 (0.0f, 0.0f, 1.0f), m_rot);

				//wind == 0 //左右に揺れる
				if (m_wind == 0) {
					if (m_dir == 0) {
						transform.position += new Vector3 (-0.0035f, -0.003f, 0.0f);
						if (m_cnt >= 150) {
							m_dir = 1;
							m_cnt = 0;
						}
					}
					if (m_dir == 1) {
						transform.position += new Vector3 (+0.0035f, -0.003f, 0.0f);
						if (m_cnt >= 150) {
							m_dir = 0;
							m_cnt = 0;
						}
					}
				}//wind == 1 //右に流れる
				if (m_wind == 1) { 

					transform.position += new Vector3 (+m_speed, 0.0f, 0.0f);
				}
				//wind == 2 //左に流れる
				if (m_wind == 2) { 

					transform.position += new Vector3 (-m_speed, 0.0f, 0.0f);
				}
				//wind == 3 //風が吹いていない
				if (m_wind == 3) {
					if (m_dir == 0) {
						transform.position += new Vector3 (-0.01f, -0.03f, 0.0f);
						if (m_cnt >= 150) {
							m_dir = 1;
							m_cnt = 0;
						}
					}
					if (m_dir == 1) {
						transform.position += new Vector3 (+0.01f, -0.03f, 0.0f);
						if (m_cnt >= 150) {
							m_dir = 0;
							m_cnt = 0;
						}
					}
				}
		
			} else {
				// プレイヤーに取得されているならば

				transform.localRotation = Quaternion.Euler (new Vector3 (0.0f, 0.0f, 0.0f));
			}
		}
	}



	/* --------------------------------------------------
	 * @衝突判定
	*/
	void OnTriggerEnter2D(Collider2D collider){

		// 
		if (collider.gameObject.tag == "Field") {

			if (this.tag != "TailPiece") {

				// 
				Destroy (gameObject);
			}
		}
	}
}
