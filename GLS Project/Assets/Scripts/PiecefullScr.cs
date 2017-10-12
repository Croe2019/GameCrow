using UnityEngine;
using System.Collections;

public class PiecefullScr : MonoBehaviour {

	// 変数宣言
	int m_st;
	float m_cnt;



	/* --------------------------------------------------
	 * @パラメータ初期化
	*/
	void Start () {
	
		m_st = 0;
		m_cnt = 50.0f;
	}



	/* --------------------------------------------------
	 * @毎フレーム更新
	*/
	void Update () {
	


		// 
		transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f));

		// 
		transform.localScale  = new Vector3 (m_cnt * 0.003f, m_cnt * 0.003f, 0.0f);

		// ----------
		// ステータス遷移
		// ----------
		if (m_st == 0) {

			// 
			m_cnt += 2.0f;
			if (m_cnt > 100.0f) {

				m_st = 1;
			}

		} else if (m_st == 1) {

			m_cnt -= 0.25f;
			if (m_cnt <= 0.0f) {

				m_st = 0;
			}
		}
		// ----------


	}
}
