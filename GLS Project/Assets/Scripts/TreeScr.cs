using UnityEngine;
using System.Collections;

public class TreeScr : MonoBehaviour {

	// 定数宣言
	public int PIECEFULL_MAX = 30;
	public int SCORE_MAX;

	// 変数宣言
	int m_cnt;
	public int m_score;
	public GameObject m_piecefull_obj;
	GameObject[] m_Piecefull_obj;
	int is_piecefull_score;

	AudioSource m_audio_source;


	/* --------------------------------------------------
	 * @パラメータ初期化
	*/
	void Start () {
	
		// 
		this.GetComponent<Animator> ().speed = 0.0f;

		// 
		m_cnt = 0;
		m_score = 0;
		m_Piecefull_obj = new GameObject[PIECEFULL_MAX];
		for (int i = 0; i < PIECEFULL_MAX; i++) {
			m_Piecefull_obj[i] = null;
		}

		is_piecefull_score = 0;

		// 
		m_audio_source = gameObject.GetComponent<AudioSource> ();
	}



	/* --------------------------------------------------
	 * @毎フレーム更新
	*/
	void Update () {

		// シーンストッパー
		float scene = GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_scene;
		if (scene != 0 && scene != 1 && scene != 12 && scene != 20) {
		
			m_cnt++;
			if (m_cnt > 60) {
				transform.FindChild ("TreePoint").transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
				m_cnt = 0;
			}
			if (m_cnt < 30) {
				
				Color cl = transform.FindChild ("TreePoint").GetComponent<SpriteRenderer> ().color;
				cl.a = 0;
				transform.FindChild ("TreePoint").GetComponent<SpriteRenderer> ().color = cl;
			} else {
				transform.FindChild ("TreePoint").transform.localScale += new Vector3 (0.1f, 0.1f, 0.0f);
				Color cl = transform.FindChild ("TreePoint").GetComponent<SpriteRenderer> ().color;
				cl.a = 1;
				transform.FindChild ("TreePoint").GetComponent<SpriteRenderer> ().color = cl;
			}
		}


		// 
		Vector3 cam_pos = GameObject.FindWithTag ("MainCamera").transform.position;
		transform.position = new Vector3 (cam_pos.x * -1.0f, transform.position.y, transform.position.z);

		// 
		if (m_score < GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_score) {
		
			m_score++;
		}

		// ※シーンストッパー※
		SCORE_MAX = GameObject.Find ("GameMain").GetComponent<GameMainScr> ().SCORE_MAX;
		if (GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_scene >= 7) { 
			SCORE_MAX *= 2;
		}

		m_score = GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_score;
		AnimatorStateInfo currentState = this.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0);
		float time = currentState.normalizedTime * (float)SCORE_MAX;//GameObject.Find ("GameMain").GetComponent<GameMainScr> ().SCORE_MAX;
		if (m_score > SCORE_MAX - 1) {
			m_score = SCORE_MAX - 1;
		}
		if (time < m_score) {
			this.GetComponent<Animator> ().speed = 1.0f;
		} else {
			this.GetComponent<Animator> ().speed = 0.0f;
		}

		// 
		PieceFullControl ();
	}


	/* --------------------------------------------------
	 * @花を咲かせる
	*/
	void PieceFullControl(){
		
		// ------------------------------
		// 一次シーケンス
		// ------------------------------
		if (m_score == (SCORE_MAX * 3) / 100) {
			
			if (is_piecefull_score != m_score) {
				
				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [0] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [0].transform.parent = this.transform;
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-0.8f, -2.7f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}
		
		} else if (m_score == (SCORE_MAX * 6) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-1.2f, -2.5f, transform.position.z);

				// 
				m_Piecefull_obj [1] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [1].transform.parent = this.transform;
				m_Piecefull_obj [1].transform.localPosition = new Vector3 (-1.75f, -0.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 7) / 100) {

			if (is_piecefull_score != m_score) {
				
				// 
				transform.FindChild ("TreePoint").transform.localPosition += new Vector3 (0.0f, 0.2f, 0.0f);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 9) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [2] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [2].transform.parent = this.transform;
				m_Piecefull_obj [2].transform.localPosition = new Vector3 (-0.7f, -0.9f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 10) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-1.3f, -2.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 12) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [3] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [3].transform.parent = this.transform;
				m_Piecefull_obj [3].transform.localPosition = new Vector3 (-0.9f, 0.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 14) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-1.5f, -2.5f, transform.position.z);

				// 
				transform.FindChild ("TreePoint").transform.position += new Vector3 (0.0f, 0.2f, 0.0f);

				// 
				is_piecefull_score = m_score;
			}
		
		} else if (m_score == (SCORE_MAX * 15) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [4] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [4].transform.parent = this.transform;
				m_Piecefull_obj [4].transform.localPosition = new Vector3 (2.0f, 0.4f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 17) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				transform.FindChild ("TreePoint").transform.localPosition += new Vector3 (0.0f, 0.2f, 0.0f);

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-1.7f, -2.3f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}
		
		} else if (m_score == (SCORE_MAX * 18) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [5] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [5].transform.parent = this.transform;
				m_Piecefull_obj [5].transform.localPosition = new Vector3 (1.0f, -1.2f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 20) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				transform.FindChild ("TreePoint").transform.localPosition += new Vector3 (0.0f, 0.2f, 0.0f);

				// 
				is_piecefull_score = m_score;
			}
		
		} else if (m_score == (SCORE_MAX * 21) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [6] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [6].transform.parent = this.transform;
				m_Piecefull_obj [6].transform.localPosition = new Vector3 (2.5f, -1.0f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}
		
		} else if (m_score == (SCORE_MAX * 24) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-1.6f, -2.0f, transform.position.z);

				// 
				m_Piecefull_obj [7] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [7].transform.parent = this.transform;
				m_Piecefull_obj [7].transform.localPosition = new Vector3 (2.5f, -2.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}
		
		} else if (m_score == (SCORE_MAX * 27) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				transform.FindChild ("TreePoint").transform.localPosition += new Vector3 (0.0f, 0.2f, 0.0f);

				// 
				m_Piecefull_obj [8] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [8].transform.parent = this.transform;
				m_Piecefull_obj [8].transform.localPosition = new Vector3 (0.5f, -2.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}
		
		} else if (m_score == (SCORE_MAX * 30) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-1.5f, -1.8f, transform.position.z);

				// 
				m_Piecefull_obj [9] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [9].transform.parent = this.transform;
				m_Piecefull_obj [9].transform.localPosition = new Vector3 (0.25f, -0.2f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

			// ------------------------------
			// 二次シーケンス
			// ------------------------------
		} else if (m_score == (SCORE_MAX * 33) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [10] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [10].transform.parent = this.transform;
				m_Piecefull_obj [10].transform.localPosition = new Vector3 (1.0f, 1.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 34) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				transform.FindChild ("TreePoint").transform.localPosition += new Vector3 (-0.2f, 0.2f, 0.0f);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 36) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [11] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [11].transform.parent = this.transform;
				m_Piecefull_obj [11].transform.localPosition = new Vector3 (-2.0f, 1.0f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 37) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				transform.FindChild ("TreePoint").transform.localPosition += new Vector3 (0.0f, 0.2f, 0.0f);

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-1.6f, -1.4f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 39) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [12] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [12].transform.parent = this.transform;
				m_Piecefull_obj [12].transform.localPosition = new Vector3 (-1.0f, 2.0f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 40) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				transform.FindChild ("TreePoint").transform.localPosition += new Vector3 (0.0f, 0.2f, 0.0f);

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-2.0f, -1.0f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 42) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [13] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [13].transform.parent = this.transform;
				m_Piecefull_obj [13].transform.localPosition = new Vector3 (2.5f, 1.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 44) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-2.3f, -0.8f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 45) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [14] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [14].transform.parent = this.transform;
				m_Piecefull_obj [14].transform.localPosition = new Vector3 (3.5f, 0.0f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 47) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				transform.FindChild ("TreePoint").transform.localPosition += new Vector3 (0.2f, 0.2f, 0.0f);

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-2.7f, -0.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 48) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [15] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [15].transform.parent = this.transform;
				m_Piecefull_obj [15].transform.localPosition = new Vector3 (-1.0f, -2.25f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 50) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-2.8f, -0.2f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 51) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [16] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [16].transform.parent = this.transform;
				m_Piecefull_obj [16].transform.localPosition = new Vector3 (-2.5f, -1.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 54) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-2.8f, -0.1f, transform.position.z);

				// 
				m_Piecefull_obj [17] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [17].transform.parent = this.transform;
				m_Piecefull_obj [17].transform.localPosition = new Vector3 (-2.2f, 2.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 57) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				transform.FindChild ("TreePoint").transform.localPosition += new Vector3 (0.0f, 0.2f, 0.0f);

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-2.8f, 1.0f, transform.position.z);

				// 
				m_Piecefull_obj [18] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [18].transform.parent = this.transform;
				m_Piecefull_obj [18].transform.localPosition = new Vector3 (-3.5f, 0.0f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 60) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-2.5f, 1.8f, transform.position.z);

				// 
				m_Piecefull_obj [19] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [19].transform.parent = this.transform;
				m_Piecefull_obj [19].transform.localPosition = new Vector3 (-0.5f, -3.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

			// ------------------------------
			// 三次シーケンス
			// ------------------------------
		} else if (m_score == (SCORE_MAX * 63) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				transform.FindChild ("TreePoint").transform.localPosition += new Vector3 (0.0f, 0.2f, 0.0f);

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-1.7f, 2.5f, transform.position.z);

				// 
				m_Piecefull_obj [20] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [20].transform.parent = this.transform;
				m_Piecefull_obj [20].transform.localPosition = new Vector3 (-3.0f, 1.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 66) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [21] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [21].transform.parent = this.transform;
				m_Piecefull_obj [21].transform.localPosition = new Vector3 (1.5f, 2.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 67) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				transform.FindChild ("TreePoint").transform.localPosition += new Vector3 (0.0f, 0.4f, 0.0f);

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-1.7f, 3.3f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 69) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [22] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [22].transform.parent = this.transform;
				m_Piecefull_obj [22].transform.localPosition = new Vector3 (3.0f, 3.0f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 70) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				transform.FindChild ("TreePoint").transform.localPosition += new Vector3 (0.0f, 1.0f, 0.0f);

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-1.2f, 4.0f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 72) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [23] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [23].transform.parent = this.transform;
				m_Piecefull_obj [23].transform.localPosition = new Vector3 (4.0f, 1.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 73) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				transform.FindChild ("TreePoint").transform.localPosition += new Vector3 (0.4f, 0.0f, 0.0f);

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-0.8f, 4.6f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 75) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [24] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [24].transform.parent = this.transform;
				m_Piecefull_obj [24].transform.localPosition = new Vector3 (3.75f, -1.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 77) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-0.6f, 5.0f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 78) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [25] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [25].transform.parent = this.transform;
				m_Piecefull_obj [25].transform.localPosition = new Vector3 (2.0f, 4.0f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 80) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (-0.3f, 5.0f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 81) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [26] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [26].transform.parent = this.transform;
				m_Piecefull_obj [26].transform.localPosition = new Vector3 (0.0f, 2.25f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 83) / 100) {

			if (is_piecefull_score != m_score) {

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (0.0f, 5.2f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 84) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				m_Piecefull_obj [27] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [27].transform.parent = this.transform;
				m_Piecefull_obj [27].transform.localPosition = new Vector3 (-1.2f, 4.0f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 87) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				transform.FindChild ("TreePoint").transform.localPosition += new Vector3 (0.0f, -0.4f, 0.0f);

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (0.3f, 5.2f, transform.position.z);

				// 
				m_Piecefull_obj [28] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [28].transform.parent = this.transform;
				m_Piecefull_obj [28].transform.localPosition = new Vector3 (1.5f, -3.5f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}

		} else if (m_score == (SCORE_MAX * 90) / 100) {

			if (is_piecefull_score != m_score) {

				// SE再生
				if (m_audio_source.isPlaying == false) {
					m_audio_source.Play ();
				}

				// 
				transform.FindChild ("TreePoint").transform.localPosition += new Vector3 (-0.8f, 0.0f, 0.0f);

				// 
				m_Piecefull_obj [0].transform.localPosition = new Vector3 (0.5f, 5.2f, transform.position.z);

				// 
				m_Piecefull_obj [29] = Instantiate (m_piecefull_obj, transform.position, Quaternion.identity) as GameObject;
				m_Piecefull_obj [29].transform.parent = this.transform;
				m_Piecefull_obj [29].transform.localPosition = new Vector3 (0.0f, 0.75f, transform.position.z);

				// 
				is_piecefull_score = m_score;
			}
		}
	}



	/* --------------------------------------------------
	 * @衝突判定
	*/
	void OnTriggerEnter2D(Collider2D collider){

		/*
		// ----------
		// 衝突判定：取得済みかけら
		// ----------
		if (collider.gameObject.tag == "TailPiece") {

			// SE再生
			m_audio_source.Play();

			// 
			m_score++;
			GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_score = m_score;
		}
		*/
	}
}
