using UnityEngine;
using System.Collections;

public class PieceRandomSpownScr : MonoBehaviour {

	// 変数宣言
	public GameObject m_piece_obj;
	public GameObject m_blockpiece_obj;
	public int m_rnd_blockpiece;
	int m_cnt;
	public int m_interval;

	public float m_speed_max;
	public float m_speed_min;
	public int m_wind;
	public int m_white_percent;
	public int m_gold_percent;
	public int m_blak_percent;



	/* --------------------------------------------------
	 * @パラメータ初期化
	*/
	void Start () {
	
		// 「かけらランダム生成」の呼び出し
		//StartCoroutine ("PieceRandomSpown");

	}



	/* --------------------------------------------------
	 * @毎フレーム更新
	*/
	void Update () {
	
		// ----------
		// ※シーンストッパー※
		// ----------
		int scene = GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_scene;
		if (scene != 0 && scene != 1 && scene != 11 && scene != 12 && scene != 20) {
			
			m_cnt++;
			if (m_cnt > m_interval) {
			
				// 生成座標のランダム取得
				Vector3 pos = transform.position;
				float x = Random.Range (pos.x - 10.0f, pos.x + 10.0f);
				float y = Random.Range (pos.y + 20.0f, pos.y + 25.0f);

				// 
				int rnd = Random.Range (0, m_rnd_blockpiece);

				if (rnd == (m_rnd_blockpiece - 1)) {

					// 固まりかけら生成
					Instantiate (m_blockpiece_obj, new Vector3 (x, y, 0.0f), Quaternion.identity);

				} else {

					// 通常かけら生成
					Instantiate (m_piece_obj, new Vector3 (x, y, 0.0f), Quaternion.identity);
				}

				m_cnt = 0;
			}
		}
	}



	/* --------------------------------------------------
	 * @かけらランダム生成
	*/
	IEnumerator PieceRandomSpown(){
		
		while (true) {

			// 生成座標のランダム取得
			Vector3 pos = transform.position;
			float x = Random.Range (pos.x - 10.0f, pos.x + 10.0f);
			float y = Random.Range (pos.y + 20.0f, pos.y + 25.0f);

			// 
			int rnd = Random.Range(0, m_rnd_blockpiece);

			if (rnd == (m_rnd_blockpiece - 1)) {

				// 固まりかけら生成
				Instantiate(m_blockpiece_obj, new Vector3 (x, y, 0.0f), Quaternion.identity);

			} else {
			
				// 通常かけら生成
				Instantiate (m_piece_obj, new Vector3 (x, y, 0.0f), Quaternion.identity);
			}

			// インターバル
			yield return new WaitForSeconds (m_interval);
		}
	}
}
