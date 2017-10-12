using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMainScr : MonoBehaviour {

	// 定数宣言
	public int SCORE_MAX = 50;

	// 変数宣言
	public int m_scene;
	int m_past_scene;
	public int m_score;
	public int m_white_score;
	public int m_gold_score;
	public int m_black_score;


	int m_cnt;
	Color m_color;

	// 木
	public GameObject m_tree_obj;

	// カラス
	public GameObject m_bird_obj;
	GameObject m_Bird_a_obj;
	GameObject m_Bird_b_obj;
	GameObject m_Bird_c_obj;

	// 風
	public GameObject m_wind_obj;

	// 背景
	public Sprite m_bg_1;
	public Sprite m_bg_2;
	public Sprite m_bg_3;
	public Sprite m_bg_4;
	public Sprite m_bg_5;
	Sprite m_next_bg;
	public GameObject m_stage_6;
	public GameObject m_stage_7;
	public GameObject m_stage_8;
	public GameObject m_stage_9;

	// BGM
	public AudioClip m_bgm_1;		
	public AudioClip m_bgm_2;
	public AudioClip m_bgm_3;
	public AudioClip m_bgm_4;
	public AudioClip m_bgm_5;
	public AudioClip m_se_start;
	public AudioClip m_se_change;
	AudioSource m_bgm_source;
	AudioSource m_se_source;

	// ステージロゴ
	public Sprite m_stage_logo_1;
	public Sprite m_stage_logo_2;
	public Sprite m_stage_logo_3;
	public Sprite m_stage_logo_4;
	public Sprite m_stage_logo_5;
	public Sprite m_stage_logo_6;

	// テキスト
	public Text m_text_1;
	public Text m_text_2;
	public Text m_text_3;

	// ヘルプ
	public bool is_help;

	// エンド
	public GameObject m_result_obj;
	public GameObject m_end_logo_obj;
	public GameObject m_end_card_obj;

	// 
	string m_scene_key = "SavedScene";
	string m_score_key = "SavedScore";
	int is_save_scene;
	int is_save_score;


	/* --------------------------------------------------
	 * @パラメータ初期化
	*/
	void Start () {
	
		// パラメータ初期化
		m_cnt = 0;
		m_scene = 0;
		m_past_scene = 0;
		m_white_score = 0;
		m_gold_score = 0;
		m_black_score = 0;

		// 
		m_next_bg = null;

		// 
		AudioSource[] audio_sources = gameObject.GetComponents<AudioSource> ();
		m_bgm_source = audio_sources[0];
		m_bgm_source.clip = m_bgm_1; 
		m_se_source = audio_sources [1];

		// 
		is_save_scene = 0;
		is_save_score = 0;

		// 
		LoadScene();
	}



	/* --------------------------------------------------
	 * @毎フレーム更新
	*/
	void Update () {

		// ----------------------
		// ※デバック※
		// ----------------------
		if (Input.GetKey(KeyCode.Space)) {
			m_score++;
		} else if(Input.GetKey(KeyCode.R)){
			m_score = 0;
			SaveScene ();
		}
		// ----------------------

		// 
		Vector3 c_pos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
		transform.position = new Vector3 (c_pos.x, c_pos.y, 0.0f);

		// ------------------------------
		// シーン遷移
		// ------------------------------
		if (m_scene == 0) {

			// タイトル

			// カウントの加算
			m_cnt++;
			if (m_cnt > 120) {
				m_cnt = 0;
			}

			// タップスタートの点滅
			if (m_cnt < 60) {
				Color cl = transform.FindChild ("TapStart").GetComponent<SpriteRenderer> ().color;
				cl.a = 1.0f;
				transform.FindChild ("TapStart").GetComponent<SpriteRenderer> ().color = cl;
			} else {
				Color cl = transform.FindChild ("TapStart").GetComponent<SpriteRenderer> ().color;
				cl.a = 0.0f;
				transform.FindChild ("TapStart").GetComponent<SpriteRenderer> ().color = cl;
			}

			// BGMを再生する（※再生していなかったら再生する、の条件ストッパー※）
			if (m_bgm_source.isPlaying == false) {
				m_bgm_source.Play ();
			}

			// マウスをクリックしたならば
			if (Input.GetMouseButtonUp (0)) {

				// 
				m_color = transform.FindChild ("TitleLogo").GetComponent<SpriteRenderer> ().color;

				// タップスタートの非表示
				GameObject obj = transform.FindChild ("TapStart").gameObject;
				Destroy (obj);

				// BGMを止める
				m_bgm_source.Stop ();
				// クリップを次のBGMに変える
				m_bgm_source.clip = m_bgm_2;

				// SE再生
				m_se_source.clip = m_se_start;
				if (m_se_source.isPlaying == false) {
					m_se_source.Play ();
				}

				// 次のシーンへ
				m_scene = 1;
			}

		} else if (m_scene == 1) {

			// メインシーン導入

			// カウントの加算
			m_cnt++;

			// 
			m_color.a = 1.0f - ((float)m_cnt / 300.0f); 
			transform.FindChild ("TitleLogo").GetComponent<SpriteRenderer> ().color = m_color;

			// 次のステージへ
			if (m_cnt > 300) {
				
				// BGMを再生する
				if (m_bgm_source.isPlaying == false) {
					m_bgm_source.Play ();
				}

				// 
				if (is_save_scene != 0) {
					m_scene = is_save_scene;
				}
				m_past_scene = m_scene;
				m_cnt = 0;
				m_scene = 20;
			}
		
		} else if (m_scene == 2) {

			// ステージ1

			if (m_cnt == 0) {
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_wind = 0;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_interval = 30;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_max = 0.03f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_min = 0.01f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_white_percent = 80;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_gold_percent = 20;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_blak_percent = 0;
			}

			if (m_score >= SCORE_MAX) {

				// セーブ
				SaveScene ();

				// SE再生
				m_se_source.clip = m_se_change;
				if (m_se_source.isPlaying == false) {
					m_se_source.Play ();
				}

				// 
				m_past_scene = m_scene;
				m_cnt = 0;
				m_scene = 20;
			}

		} else if (m_scene == 3) {

			// ステージ2

			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScr> ().SPEED_DEF = 0.025f;

			if (m_cnt == 0) {
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_wind = 1;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_interval = 30;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_max = 0.03f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_min = 0.01f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_white_percent = 80;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_gold_percent = 20;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_blak_percent = 0;
			}

			// カウントの加算
			m_cnt++;
			if (m_cnt > 3600) {
				m_cnt = 0;
			}

			if (m_score >= SCORE_MAX) {

				// セーブ
				SaveScene ();

				// ---
				// SE再生
				m_se_source.clip = m_se_change;
				if (m_se_source.isPlaying == false) {
					m_se_source.Play ();
				}
				// ---

				// 
				m_cnt = 0;
				m_past_scene = m_scene;
				m_scene = 20;
			}
		
		} else if (m_scene == 4) {

			// ステージ3

			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScr> ().SPEED_DEF = 0.04f;

			if (m_cnt == 0) {
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_wind = 0;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_interval = 60;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_max = 0.04f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_min = 0.01f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_white_percent = 60;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_gold_percent = 20;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_blak_percent = 20;
			}

			// カウントの加算
			m_cnt++;
			if (m_cnt > 3600) {
				m_cnt = 0;
			}

			/*
			// チャンスタイム
			if (m_cnt == 0) {
				if (m_se_source.isPlaying == false) {
					m_se_source.Play ();
				}
			}
			if (m_cnt > 0 && m_cnt <= 100) {
				Color cl = transform.FindChild ("FilterWhite").GetComponent<SpriteRenderer> ().color;
				cl.a = (float)m_cnt * 0.01f;
				transform.FindChild ("FilterWhite").GetComponent<SpriteRenderer> ().color = cl;
			} else if (m_cnt > 100 && m_cnt <= 200) {
				Color cl = transform.FindChild ("FilterWhite").GetComponent<SpriteRenderer> ().color;
				cl.a = (float)(m_cnt - 200) * -0.01f;
				transform.FindChild ("FilterWhite").GetComponent<SpriteRenderer> ().color = cl;
			}
			if (m_cnt > 0 && m_cnt <= 1800) {
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_interval = 0.2f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_rnd_blockpiece = 1;
			} else {
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_interval = 0.3f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_rnd_blockpiece = 5;
			}
			*/
			if (m_score >= SCORE_MAX) {

				// セーブ
				SaveScene ();

				// SE再生
				m_se_source.clip = m_se_change;
				if (m_se_source.isPlaying == false) {
					m_se_source.Play ();
				}

				// 
				m_cnt = 0;
				m_past_scene = m_scene;
				m_scene = 20;
			}
		
		} else if (m_scene == 5) {

			// ステージ4

			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScr> ().SPEED_DEF = 0.04f;

			if (m_cnt == 0) {
				m_Bird_b_obj.GetComponent<BirdScr> ().m_speed_setting = 0.03f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_wind = 2;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_interval = 60;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_rnd_blockpiece = 7;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_max = 0.03f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_min = 0.01f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_white_percent = 40;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_gold_percent = 30;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_blak_percent = 30;
			}

			// カウントの加算
			m_cnt++;
			if (m_cnt > 3600) {
				m_cnt = 0;
			}

			if (m_score >= SCORE_MAX) {
				
				// セーブ
				SaveScene ();

				// ---
				// SE再生
				m_se_source.clip = m_se_change;
				if (m_se_source.isPlaying == false) {
					m_se_source.Play ();
				}
				// ---

				// 
				m_cnt = 0;
				m_past_scene = m_scene;
				m_scene = 20;
			}

		} else if (m_scene == 6) {

			// ステージ5

			// カウントの加算
			m_cnt++;
			if (m_cnt > 3600) {
				m_cnt = 0;
			}

			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScr> ().SPEED_DEF = 0.04f;

			if (m_cnt == 0) {
				m_Bird_b_obj.GetComponent<BirdScr> ().m_speed_setting = 0.03f;
				m_Bird_c_obj.GetComponent<BirdScr> ().m_speed_setting = 0.03f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_wind = 0;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_interval = 60;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_rnd_blockpiece = 5;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_max = 0.04f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_min = 0.02f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_white_percent = 40;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_gold_percent = 30;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_blak_percent = 30;
			}

			if (m_score >= SCORE_MAX) {

				// セーブ
				SaveScene ();

				// ---
				// SE再生
				m_se_source.clip = m_se_change;
				if (m_se_source.isPlaying == false) {
					m_se_source.Play ();
				}
				// ---

				// 
				m_cnt = 0;
				m_past_scene = m_scene;
				m_scene = 20;
			}

		} else if (m_scene == 7) {

			// ステージ6

			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScr> ().SPEED_DEF = 0.06f;

			if (m_cnt == 0) {
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_wind = 0;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_max = 0.03f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_min = 0.01f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_interval = 60;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_rnd_blockpiece = 10;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_white_percent = 80;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_gold_percent = 20;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_blak_percent = 0;
			}

			// カウントの加算
			m_cnt++;
			if (m_cnt > 3600) {
				m_cnt = 0;
			}

			if (m_score >= 25) {
				
				// ---
				// SE再生
				m_se_source.clip = m_se_change;
				if (m_se_source.isPlaying == false) {
					m_se_source.Play ();
				}
				// ---

				Instantiate (m_stage_7);
			
				// 
				m_cnt = 0;
				m_past_scene = m_scene;
				m_scene = 8;
			}	

		} else if (m_scene == 8) {

			// ステージ7

			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScr> ().SPEED_DEF = 0.06f;

			if (m_cnt == 0) {
				if (m_Bird_a_obj == null) {
					m_Bird_a_obj = Instantiate (m_bird_obj, new Vector3 (-15.0f, 12.5f, 0.0f), Quaternion.identity) as GameObject;
				}
				GameObject.Find ("PieceSpown").transform.position = new Vector3 (-10.0f, 0.0f, 0.0f);
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_wind = 1;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_max = 0.04f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_min = 0.02f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_interval = 45;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_rnd_blockpiece = 7;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_white_percent = 60;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_gold_percent = 20;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_blak_percent = 20;
			}

			// カウントの加算
			m_cnt++;
			if (m_cnt > 3600) {
				m_cnt = 0;
			}

			if (m_score >= 50) {

				// ---
				// SE再生
				m_se_source.clip = m_se_change;
				if (m_se_source.isPlaying == false) {
					m_se_source.Play ();
				}
				// ---

				Instantiate (m_stage_8);

				// 
				m_cnt = 0;
				m_past_scene = m_scene;
				m_scene = 9;
			}	

		} else if (m_scene == 9) {

			// ステージ8

			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScr> ().SPEED_DEF = 0.06f;

			if (m_cnt == 0) {
				if (m_Bird_b_obj == null) {
					m_Bird_b_obj = Instantiate (m_bird_obj, new Vector3 (15.0f, 17.5f, 0.0f), Quaternion.identity) as GameObject;
					m_Bird_b_obj.GetComponent<BirdScr> ().m_speed_setting = 0.03f;
				}
				GameObject.Find ("PieceSpown").transform.position = new Vector3 (10.0f, 0.0f, 0.0f);
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_wind = 2;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_max = 0.04f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_min = 0.02f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_interval = 45;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_rnd_blockpiece = 5;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_white_percent = 40;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_gold_percent = 30;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_blak_percent = 30;
			}

			// カウントの加算
			m_cnt++;
			if (m_cnt > 3600) {
				m_cnt = 0;
			}

			if (m_score >= 75) {

				// ---
				// SE再生
				m_se_source.clip = m_se_change;
				if (m_se_source.isPlaying == false) {
					m_se_source.Play ();
				}
				// ---

				Instantiate (m_stage_9);

				// 
				m_cnt = 0;
				m_past_scene = m_scene;
				m_scene = 10;
			}	

		} else if (m_scene == 10) {

			// ステージ9

			GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScr> ().SPEED_DEF = 0.06f;

			if (m_cnt == 0) {
				if (m_Bird_c_obj == null) {
					m_Bird_c_obj = Instantiate (m_bird_obj, new Vector3 (-15.0f, 7.5f, 0.0f), Quaternion.identity) as GameObject;
					m_Bird_c_obj.GetComponent<BirdScr> ().m_speed_setting = 0.03f;
				}
				GameObject.Find ("PieceSpown").transform.position = new Vector3 (0.0f, 0.0f, 0.0f);
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_wind = 0;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_max = 0.05f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_speed_min = 0.02f;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_interval = 20;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_rnd_blockpiece = 2;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_white_percent = 40;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_gold_percent = 30;
				GameObject.Find ("PieceSpown").GetComponent<PieceRandomSpownScr> ().m_blak_percent = 30;
			}

			// カウントの加算
			m_cnt++;
			if (m_cnt > 3600) {
				m_cnt = 0;
			}

			if (m_score >= 100) {

				// ---
				// SE再生
				m_se_source.clip = m_se_change;
				if (m_se_source.isPlaying == false) {
					m_se_source.Play ();
				}
				// ---


				DestroyObject (m_Bird_a_obj);
				DestroyObject (m_Bird_b_obj);
				DestroyObject (m_Bird_c_obj);

				// 
				m_cnt = 0;
				m_past_scene = m_scene;
				m_scene = 11;
			}
		
		} else if (m_scene == 11) {

			// エンド導入

			// 
			if (m_cnt > 0 && m_cnt <= 1000) {
				Color cl = transform.FindChild ("FilterWhite").GetComponent<SpriteRenderer> ().color;
				cl.r -= 0.001f;
				cl.g -= 0.001f;
				cl.b -= 0.001f;
				cl.a = (float)m_cnt * 0.001f;
				transform.FindChild ("FilterWhite").GetComponent<SpriteRenderer> ().color = cl;
			} /*else if (m_cnt > 100 && m_cnt <= 200) {
				Color cl = transform.FindChild ("FilterWhite").GetComponent<SpriteRenderer> ().color;
				cl.a = (float)(m_cnt - 200) * -0.01f;
				transform.FindChild ("FilterWhite").GetComponent<SpriteRenderer> ().color = cl;
			}*/
			if (m_cnt == 600) {
				Instantiate (m_result_obj);
			} else if (m_cnt > 600) {
				m_text_1.text = m_white_score.ToString();
				m_text_2.text = m_gold_score.ToString();
				m_text_3.text = m_black_score.ToString();
			}

			// 
			c_pos = new Vector3 (0.0f, 7.0f, c_pos.z);
			GameObject.FindGameObjectWithTag ("MainCamera").transform.position = c_pos;
			if (GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ().orthographicSize < 12.0f) {
				GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ().orthographicSize = 6.0f + ((float)m_cnt * 0.01f);
			}

			// カウントの加算
			m_cnt++;
			if (m_cnt > 1800) {

				DestroyObject (GameObject.FindGameObjectWithTag("Result"));
				m_text_1.text = "";
				m_text_2.text = "";
				m_text_3.text = "";

				// 
				Instantiate (m_end_card_obj, new Vector3 (c_pos.x, c_pos.y, 0.0f), Quaternion.identity);
				Instantiate (m_end_logo_obj, new Vector3 (c_pos.x, c_pos.y + 6.0f, 0.0f), Quaternion.identity);

				// ---
				// BGMを止める
				m_bgm_source.Stop ();
				// クリップを次のBGMに変える
				m_bgm_source.clip = m_bgm_1;
				// BGMを再生する
				if (m_bgm_source.isPlaying == false) {
					m_bgm_source.Play ();
				}
				// ---

				// 
				Destroy (GameObject.Find ("PieceSpown").gameObject);

				// 
				m_cnt = 0;
				m_scene = 12;
			}

		} else if (m_scene == 12) {

			// エンド

			// カウントの加算
			m_cnt++;
			if (m_cnt > 300) {

				// マウスをクリックしたならば
				if (Input.GetMouseButtonUp (0)) {

					m_cnt = 0;

					// 初期化
					Application.LoadLevel ("Reset_Scene");
				}
			}
		
		} else if (m_scene == 20) {

			Destroy (GameObject.FindGameObjectWithTag ("Piece"));

			// 挿入シーン
			if (m_cnt == 0) {
				// --------------------
				// Next Scene Setting
				// --------------------
				if (m_past_scene == 0) {

				} else if (m_past_scene == 1) {
					m_next_bg = m_bg_1;
					//m_text.text = "STAGE 1\n\nWasteland";
					transform.FindChild ("StageLogo").GetComponent<SpriteRenderer> ().sprite = m_stage_logo_1;
				} else if (m_past_scene == 2) {
					m_next_bg = m_bg_2;
					//m_text.text = "STAGE 2\n\nRuins";
					transform.FindChild ("StageLogo").GetComponent<SpriteRenderer> ().sprite = m_stage_logo_2;
					Instantiate (m_wind_obj, new Vector3 (0.0f, 5.0f, 0.0f), Quaternion.Euler (new Vector3 (0.0f, 180.0f, 0.0f)));
					GameObject.Find ("PieceSpown").transform.position = new Vector3 (-10.0f, 0.0f, 0.0f);
				} else if (m_past_scene == 3) {
					m_next_bg = m_bg_3;
					//m_text.text = "STAGE 3\n\nMountain";
					transform.FindChild ("StageLogo").GetComponent<SpriteRenderer> ().sprite = m_stage_logo_3;
					m_bgm_source.Stop ();
					m_bgm_source.clip = m_bgm_3;
					if (m_bgm_source.isPlaying == false) {
						m_bgm_source.Play ();
					}
					DestroyObject (GameObject.FindGameObjectWithTag ("Wind"));
					m_Bird_a_obj = Instantiate (m_bird_obj, new Vector3 (-15.0f, 12.5f, 0.0f), Quaternion.identity) as GameObject;
					GameObject.Find ("PieceSpown").transform.position = new Vector3 (0.0f, 0.0f, 0.0f);
				} else if (m_past_scene == 4) {
					m_next_bg = m_bg_4;
					//m_text.text = "STAGE 4\n\nCanyon";
					transform.FindChild ("StageLogo").GetComponent<SpriteRenderer> ().sprite = m_stage_logo_4;
					DestroyObject (m_Bird_a_obj);
					Instantiate (m_wind_obj, new Vector3 (0.0f, 5.0f, 0.0f), Quaternion.Euler (new Vector3 (0.0f, 0.0f, 0.0f)));
					m_Bird_a_obj = Instantiate (m_bird_obj, new Vector3 (-15.0f, 12.5f, 0.0f), Quaternion.identity) as GameObject;
					m_Bird_b_obj = Instantiate (m_bird_obj, new Vector3 (15.0f, 17.5f, 0.0f), Quaternion.identity) as GameObject;
					GameObject.Find ("PieceSpown").transform.position = new Vector3 (10.0f, 0.0f, 0.0f);
				} else if (m_past_scene == 5) {
					m_next_bg = m_bg_5;
					//m_text.text = "STAGE 5\n\nCave";
					transform.FindChild ("StageLogo").GetComponent<SpriteRenderer> ().sprite = m_stage_logo_5;
					m_bgm_source.Stop ();
					m_bgm_source.clip = m_bgm_4;
					if (m_bgm_source.isPlaying == false) {
						m_bgm_source.Play ();
					}
					DestroyObject (GameObject.FindGameObjectWithTag ("Wind"));
					DestroyObject (m_Bird_a_obj);
					DestroyObject (m_Bird_b_obj);
					m_Bird_a_obj = Instantiate (m_bird_obj, new Vector3 (-15.0f, 12.5f, 0.0f), Quaternion.identity) as GameObject;
					m_Bird_b_obj = Instantiate (m_bird_obj, new Vector3 (15.0f, 17.5f, 0.0f), Quaternion.identity) as GameObject;
					m_Bird_c_obj = Instantiate (m_bird_obj, new Vector3 (15.0f, 7.5f, 0.0f), Quaternion.identity) as GameObject;
					GameObject.Find ("PieceSpown").transform.position = new Vector3 (0.0f, 0.0f, 0.0f);
				} else if (m_past_scene == 6) {
					//m_text.text = "STAGE 6\n\nBloom World";
					transform.FindChild ("StageLogo").GetComponent<SpriteRenderer> ().sprite = m_stage_logo_6;
					m_bgm_source.Stop ();
					m_bgm_source.clip = m_bgm_5;
					if (m_bgm_source.isPlaying == false) {
						m_bgm_source.Play ();
					}
					DestroyObject (m_Bird_a_obj);
					DestroyObject (m_Bird_b_obj);
					DestroyObject (m_Bird_c_obj);
				}
				// --------------------
			}
			if (m_past_scene == 6) {
				if (m_cnt == 200) {
					DestroyObject (GameObject.Find ("Stage"));
					Instantiate (m_stage_6);
				}
			}
			if (m_cnt > 0 && m_cnt <= 100) {
				Color cl = transform.FindChild ("FilterWhite").GetComponent<SpriteRenderer> ().color;
				cl.a = (float)m_cnt * 0.01f;
				transform.FindChild ("FilterWhite").GetComponent<SpriteRenderer> ().color = cl;
			} else if (m_cnt > 200 && m_cnt <= 300) {
				Color cl = transform.FindChild ("FilterWhite").GetComponent<SpriteRenderer> ().color;
				cl.a = (float)(m_cnt - 300) * -0.01f;
				transform.FindChild ("FilterWhite").GetComponent<SpriteRenderer> ().color = cl;
			}
			if (m_cnt == 200) {
				GameObject.FindGameObjectWithTag ("Stage").transform.FindChild ("Belt_008").transform.FindChild ("8").GetComponent<SpriteRenderer> ().sprite = m_next_bg;
			}
			/*
			if (m_cnt > 100 && m_cnt <= 200) {
				Color t_cl = m_text.color;
				t_cl.a = (float)(m_cnt - 100) * 0.01f;
				m_text.color = t_cl;
			} else if (m_cnt > 400 && m_cnt <= 500) {
				Color t_cl = m_text.color;
				t_cl.a = (float)(m_cnt - 500) * -0.01f;
				m_text.color = t_cl;
			}
			*/
			if (m_cnt == 200) {
				m_score = 0;
				DestroyObject (GameObject.FindGameObjectWithTag ("Tree"));
				Instantiate (m_tree_obj);
			}
			if (m_cnt == 200) {
				GameObject.FindGameObjectWithTag ("Player").transform.position = new Vector3 (0.0f, 0.0f, 0.0f);
				GameObject.FindGameObjectWithTag ("MainCamera").transform.position = new Vector3 (0.0f, 0.0f, -10.0f);
			}
			if (m_cnt > 300 && m_cnt <= 400) {
				Color l_cl = transform.FindChild ("StageLogo").GetComponent<SpriteRenderer> ().color;
				l_cl.a = (float)(m_cnt - 300) * 0.01f;
				transform.FindChild ("StageLogo").GetComponent<SpriteRenderer> ().color = l_cl;
			} else if (m_cnt > 500 && m_cnt <= 600) {
				Color l_cl = transform.FindChild ("StageLogo").GetComponent<SpriteRenderer> ().color;
				l_cl.a = (float)(m_cnt - 600) * -0.01f;
				transform.FindChild ("StageLogo").GetComponent<SpriteRenderer> ().color = l_cl;
			}
			m_cnt++;
			if (m_cnt > 600) {
				m_cnt = 0;
				m_scene = m_past_scene + 1;
			}
		}

		// ------------------------------
	}



	/* --------------------------------------------------
	 * @
	*/
	void SaveScene(){

		// 
		PlayerPrefs.SetInt(m_scene_key, m_scene);
		PlayerPrefs.SetInt (m_score_key, m_score);
		PlayerPrefs.Save();
	}



	/* --------------------------------------------------
	 * @
	*/
	void LoadScene(){

		// 

		is_save_score = PlayerPrefs.GetInt (m_score_key, 0);
	}



	/* --------------------------------------------------
	 * @
	*/
	void SceneChange(int cnt_max){
		
		// 
		if (m_cnt == 0) {

			// 
			transform.FindChild ("filter_white").transform.position = new Vector3 (0.0f, 0.0f, 0.0f);

			// 
			transform.FindChild ("filter_black_top").transform.position = new Vector3 (0.0f, 55.0f, 0.0f);
			transform.FindChild ("filter_black_under").transform.position = new Vector3 (0.0f, -40.0f, 0.0f);
		
		} else if (m_cnt > 0 && m_cnt <= cnt_max / 4) {

			// 
			transform.FindChild ("filter_black_top").transform.position -= new Vector3(0.0f, 0.1f, 0.0f);
			transform.FindChild ("filter_black_under").transform.position += new Vector3(0.0f, (float)(1 / cnt_max), 0.0f);
		}
			
		// 
		m_cnt++;
	}
}
