using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScr : MonoBehaviour {

	// 定数宣言
	const int TAILPIECE_MAX = 25;		// しっぽかけらの定数
	const int TAILPIECE_POS_MAX = (TAILPIECE_MAX * 5) + 5;

	// 変数宣言
	Vector3 m_rot;						// 向き
	float m_deg;						// 角度入力値

	public int m_move_st;				// 移動ステータス
	int m_move_cnt;						// 移動カウント
	float m_move_speed;					// スピード
	public float m_move_speed_setting;			
	public float SPEED_DEF;				// スピードの標準値
	public float SPEED_MAX;				// スピードの最大値
	public float SPEED_MIN;				// スピードの最小値
	public float UP_SPEED;				// スピードの増加値
	public float DOWN_SPEED;			// スピードの減速値

	public float m_move_dist;

	int m_motion_st;					// モーションステータス
	int m_motion_cnt;					// モーションカウント
	float m_motion_head_rot_z;			// モーション（頭）の回転角度
	float m_motion_body_rot_z;			// モーション（体）の回転角度
	float m_motion_leg_rot_z;			// モーション（足）の回転角度

	// アニメーション
	Transform m_dummy_tsf;				// アニメーション用のダミー
	Animator m_roll_anim;				// ロールのアニメーション

	// しっぽかけら
	GameObject[] m_tailpiece_obj;			// オブジェクト
	int[] m_tailpiece_st;					// ステータス
	int[] m_tailpiece_cnt;					// カウント
	Vector3[] m_tailpiece_pos;				// 現在座標
	Vector3[] m_tailpiece_vec;				// 移動方向

	int test_score;
	int m_is_get_black;

	public GameObject m_dummy_piece_obj;
	public Sprite m_dummy_blockpiece_obj;

	// SE（かけら取得）
	AudioSource m_audio_source;
	public AudioClip m_se_get_1;
	public AudioClip m_se_get_2;
	public AudioClip m_se_get_3;


	/* --------------------------------------------------
	 * @パラメータ初期化
	*/
	void Start () {

		// プレイヤー
		m_rot = new Vector3 (0.0f, 180.0f, 0.0f);
		m_move_st = 0;
		m_move_cnt = 0;
		m_move_speed = 0.0f;
		m_move_speed_setting = SPEED_DEF;
		m_motion_st = 0;
		m_motion_cnt = 0;
		m_motion_head_rot_z = 0.0f;
		m_motion_body_rot_z = 0.0f;
		m_motion_leg_rot_z = 0.0f;

		// アニメーション
		m_dummy_tsf = transform.FindChild ("player_dummy");
		m_roll_anim = transform.FindChild ("player_dummy").GetComponent<Animator> ();

		// しっぽかけら
		m_tailpiece_obj = new GameObject[TAILPIECE_MAX];
		m_tailpiece_st = new int[TAILPIECE_MAX];
		m_tailpiece_cnt = new int[TAILPIECE_MAX];
		m_tailpiece_pos = new Vector3[TAILPIECE_POS_MAX];
		m_tailpiece_vec = new Vector3[TAILPIECE_MAX];
		for (int i = 0; i < TAILPIECE_MAX; i++) {
			m_tailpiece_obj [i] = null;
			m_tailpiece_st [i] = 0;
			m_tailpiece_cnt [i] = 0;
		}
		for (int i = 0; i < TAILPIECE_POS_MAX; i++) {
			m_tailpiece_pos [i] = new Vector3 (0.0f, 0.0f, 0.0f);
		}

		test_score = 0;
		m_is_get_black = 60;

		// SE（かけら取得）
		m_audio_source = gameObject.GetComponent<AudioSource> ();
		m_audio_source.clip = m_se_get_1;
	}



	/* --------------------------------------------------
	 * @毎フレーム更新
	*/
	void Update ()
	{
		// ※シーンストッパー※
		int scene = GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_scene;
		if (scene != 0 && scene != 1 && scene != 12 && scene != 20) {
			
			// 移動の呼び出し
			Move ();

			// 回転の呼び出し
			Rotate ();

			// モーション管理の呼び出し
			MotionControl ();

			// 取得したかけら管理の呼び出し
			TailPieceControl ();
		}

		if (scene == 20) {
			m_rot = new Vector3 (0.0f, 180.0f, 0.0f);
			transform.rotation = Quaternion.Euler (m_rot);
			m_move_st = 0;
			m_move_speed = 0.0f;
			m_motion_st = 0;
			m_motion_cnt = 0;
			m_motion_head_rot_z = 0.0f;
			m_motion_body_rot_z = 0.0f;
			m_motion_leg_rot_z = 0.0f;
			for (int i = 0; i < TAILPIECE_MAX; i++) {
				if (m_tailpiece_obj [i] != null) {
					DestroyObject (m_tailpiece_obj [i]);
					m_tailpiece_st [i] = 0;
				}
			}
			transform.FindChild ("PlayerEffect").GetComponent<ParticleEmitter> ().maxSize = 0.0f;
			transform.FindChild ("PlayerEffect").GetComponent<ParticleEmitter> ().minSize = 0.0f;
		}
	}



	/* --------------------------------------------------
	 * @透明化
	 * @プレイヤーを透明にする（※処理文が長めになってしまうので、まとめるためメソッド化させてるだけ※）
	 * @param<alpha> カラーキーの透明値
	 * @param<dummy_alpha>ダミーのカラーキーの透明値
	*/
	void ColorControl(float alpha, float dummy_alpha){

		Color cl = m_dummy_tsf.GetComponent<SpriteRenderer> ().color;
		cl.a = dummy_alpha;
		m_dummy_tsf.GetComponent<SpriteRenderer> ().color = cl;
		cl.a = alpha;
		transform.FindChild ("player_head_001.1").GetComponent<SpriteRenderer> ().color = cl;
		transform.FindChild ("player_head_001.2").GetComponent<SpriteRenderer> ().color = cl;
		transform.FindChild ("player_head_001.3").GetComponent<SpriteRenderer> ().color = cl;
		transform.FindChild ("player_head_001.4").GetComponent<SpriteRenderer> ().color = cl;
		transform.FindChild ("player_body_001.1").GetComponent<SpriteRenderer> ().color = cl;
		transform.FindChild ("player_body_001.2").GetComponent<SpriteRenderer> ().color = cl;
		transform.FindChild ("player_mant_001").GetComponent<SpriteRenderer> ().color = cl;
		transform.FindChild ("player_leg_001.1").GetComponent<SpriteRenderer> ().color = cl;
		transform.FindChild ("player_leg_001.2").GetComponent<SpriteRenderer> ().color = cl;
		transform.FindChild ("player_leg_002.1").GetComponent<SpriteRenderer> ().color = cl;
		transform.FindChild ("player_leg_002.2").GetComponent<SpriteRenderer> ().color = cl;
	}



	/* --------------------------------------------------
	 * @移動
	 * @タップした場所による移動処理ができる
	*/
	void Move(){

		float x1 = transform.position.x;

		// ---
		// タップ（マウス）のスクリーン座標取得
		Vector3 mouse_screen_pos = Input.mousePosition;
		mouse_screen_pos.z = transform.position.z - Camera.main.transform.position.z;
		// タップ（マウス）のワールド座標取得
		Vector3 mouse_world_pos = Camera.main.ScreenToWorldPoint (mouse_screen_pos);
		// タップ（マウス）座標の角度取得
		float p1 = mouse_world_pos.x - transform.position.x;
		float p2 = mouse_world_pos.y - transform.position.y;
		float rad = Mathf.Atan2 (p2, p1);
		m_deg = rad *= Mathf.Rad2Deg;
		// 角度の修正（0~360度）
		m_deg += 180.0f;
		// ---

		// ------------------------------
		// ステータス遷移
		// ------------------------------
		if (m_move_st == 0) {
			// デフォルト

			// 軌跡パーティクルの停止
			transform.FindChild ("PlayerEffect").GetComponent<ParticleEmitter> ().maxSize = 0.0f;
			transform.FindChild ("PlayerEffect").GetComponent<ParticleEmitter> ().minSize = 0.0f;

			// カウントの加算
			m_move_cnt++;

			// カウントのループ
			if (m_move_cnt > 240) {
				m_move_cnt = 0;
			}

			// ----------
			// ふわふわ処理
			// ----------
			if (m_move_cnt > 0 && m_move_cnt <= 120) {
				// 上に
				float y = (m_move_cnt - 60) * 0.0003f;
				transform.position += new Vector3 (0.0f, y, 0.0f);
			
			} else if (m_move_cnt > 120 && m_move_cnt <= 240) {
				// 下に
				float y = (m_move_cnt - 180) * 0.0003f;
				transform.position -= new Vector3 (0.0f, y, 0.0f);
			}
			// ----------

			if (Input.GetMouseButtonDown (0)) {
				// マウスをクリックした瞬間

				// ジャンプのステータスへ
				m_move_cnt = 0;
				m_move_st = 2;
			}

		} else if (m_move_st == 1) {
			// 移動操作

			// カウントの加算
			m_move_cnt++;

			// カウントのループ
			if (m_move_cnt > 240) {
				m_move_cnt = 0;
			}

			// ----------
			// ふわふわ処理
			// ----------
			if (m_move_cnt > 0 && m_move_cnt <= 120) {
				// 上に
				float y = (m_move_cnt - 60) * 0.0003f;
				transform.position += new Vector3 (0.0f, y, 0.0f);

			} else if (m_move_cnt > 120 && m_move_cnt <= 240) {
				// 下に
				float y = (m_move_cnt - 180) * 0.0003f;
				transform.position -= new Vector3 (0.0f, y, 0.0f);
			}
			// ----------

			// 現在座標とタップの入力座標との中間座標を取得
			Vector3 half_pos = mouse_world_pos - ((mouse_world_pos - transform.position) * 0.5f); 

			if (Input.GetMouseButton (0)) {
				// マウスをクリックしている間

				// 中間座標との距離を取得
				float dist = Vector2.Distance (new Vector2 (transform.position.x, transform.position.y), new Vector2 (half_pos.x, half_pos.y));

				// 距離の制限
				if (dist > 20.0f) {
//					dist = 20.0f;
				}

				// 距離に相対したスピードの取得
				m_move_speed = SPEED_DEF * dist;

				// 軌跡パーティクルの発生
				transform.FindChild ("PlayerEffect").GetComponent<ParticleEmitter> ().maxSize = 0.25f;
				transform.FindChild ("PlayerEffect").GetComponent<ParticleEmitter> ().minSize = 0.05f;
			
			} else {
				// マウスを離している間

				// スピードの減速
				m_move_speed *= 0.99f;

				// 一定まで減速したならば
				if (m_move_speed < 0.01f) {

					// 軌跡パーティクルの停止
					transform.FindChild ("PlayerEffect").GetComponent<ParticleEmitter> ().maxSize = 0.0f;
					transform.FindChild ("PlayerEffect").GetComponent<ParticleEmitter> ().minSize = 0.0f;

					// 停止のステータスへ
					m_move_speed = 0.0f;
					m_move_st = 3;
				}
			}

			// ----------
			// 疑似衝突判定
			// ----------
			Vector3 c1 = transform.position;
			Vector3 c2 = GameObject.FindGameObjectWithTag ("Tree").transform.FindChild("TreePoint").transform.position;
			if (c1.x + 0.75f < c2.x || c1.x > c2.x + 0.75f || c1.y + 0.75f < c2.y || c1.y > c2.y + 1.5f) {

			} else {

				// 
				SPEED_DEF = m_move_speed_setting;

				// 
				m_move_speed = 0.0f;

				// 
				m_move_cnt = 0;
				m_move_st = 5;
			}
			// ----------

		} else if (m_move_st == 2) {
			// ジャンプ

			// 軌跡パーティクルの発生
			transform.FindChild ("PlayerEffect").GetComponent<ParticleEmitter> ().maxSize = 0.25f;
			transform.FindChild ("PlayerEffect").GetComponent<ParticleEmitter> ().minSize = 0.05f;

			// 向きを直す
			m_rot.z = 0.0f;

			// 現在座標とタップの入力座標との中間座標を取得
			Vector3 half_pos = mouse_world_pos - ((mouse_world_pos - transform.position) * 0.5f);

			// ホップさせる
			transform.position = Vector3.MoveTowards (transform.position, half_pos, SPEED_DEF);

			// 「ジャンプモーション」の呼び出し
			MotionJump ();

		} else if (m_move_st == 3) {
			// 停止

			// 落下のステータスへ
			m_move_cnt = 120;
			m_move_st = 4;


		} else if (m_move_st == 4) {
			// 落下

			// 軌跡パーティクルの停止
			transform.FindChild ("PlayerEffect").GetComponent<ParticleEmitter> ().maxSize = 0.0f;
			transform.FindChild ("PlayerEffect").GetComponent<ParticleEmitter> ().minSize = 0.0f;

			// カウントの加算
			m_move_cnt++;

			// ダメージ演出
			if (m_move_cnt < 120) {

				// ぐるぐる回転
				m_rot.z -= 10.0f;

				// ちょっとホップする
				transform.position += new Vector3 (0.0f, (m_move_cnt - 120) * -0.0001f, 0.0f);

			} else {

				// 向きを仰向きにする
				m_rot.z = 180.0f;

				// 落下していく
				transform.position += new Vector3 (0.0f, (m_move_cnt - 120) * -0.0005f, 0.0f);
			}

			if (Input.GetMouseButtonDown (0)) {
				// マウスをクリックした瞬間

				// ジャンプのステータスへ
				m_move_speed = 0.0f;
				m_move_cnt = 0;
				m_move_st = 2;
			}
		
		} else if (m_move_st == 5) {
			// 着陸

			// 軌跡パーティクルの停止
			transform.FindChild ("PlayerEffect").GetComponent<ParticleEmitter> ().maxSize = 0.0f;
			transform.FindChild ("PlayerEffect").GetComponent<ParticleEmitter> ().minSize = 0.0f;

			// カウントの加算
			m_move_cnt++;

			// 着陸地点の座標取得
			Vector3 t_pos = GameObject.FindGameObjectWithTag ("Tree").transform.FindChild ("TreePoint").transform.position;

			// ゆっくり着陸する
			if (m_move_cnt < 60) {

				// 着陸座標の修正
				t_pos.y += 0.3f;

				// 着陸座標に移動する
				transform.position = Vector3.MoveTowards (transform.position, t_pos, (60 - m_move_cnt) * 0.00075f);

				// 向きを直す
				if ((m_rot.z > 0.0f && m_rot.z < 180.0f) || m_rot.z > 360.0f) {
					m_rot.z -= m_move_cnt * 0.09f; 

				} else {
					m_rot.z += m_move_cnt * 0.09f;
				}
			
			} else {

				// 着陸座標の修正
				t_pos.y += 0.3f;

				// 着陸する
				transform.position = t_pos;
			}

			// しっぽかけらを木にもどす
			if (m_move_cnt >= 60) {

				// もどすテンポ
				if (m_move_cnt % 6 == 0) {

					for (int i = 0; i < TAILPIECE_MAX; i++) {

						if (m_tailpiece_obj [i] != null && m_tailpiece_st [i] == 1) {

							// しっぽかけらのステータスを
							m_tailpiece_obj [i].transform.position = transform.position;
							m_tailpiece_st [i] = 3;
							break;
						}
					}
				}
			}

			if (Input.GetMouseButtonDown (0)) {
				// マウスをクリックした瞬間

				// しっぽかけらを配列に詰める
				for (int i = 0; i < TAILPIECE_MAX - 1; i++) {
					if (m_tailpiece_st [i] == 0 || m_tailpiece_st [i] == 3) {
						// [i]より後ろの要素を1つだけ[i]に移動する
						for (int j = i + 1; j < TAILPIECE_MAX; j++) {
							if (m_tailpiece_st [j] == 1) {
								GameObject work_obj = m_tailpiece_obj [i];
								int work_st = m_tailpiece_st [i];

								m_tailpiece_obj [i] = m_tailpiece_obj [j];
								m_tailpiece_st [i] = m_tailpiece_st [j];

								m_tailpiece_obj [j] = work_obj;
								m_tailpiece_st [j] = work_st;
								break;
							}
						}
					}
				}

				// ジャンプのステータスへ
				m_move_cnt = 0;
				m_move_st = 2;
			}
		}
		// ------------------------------


		if (m_is_get_black < 60) {
			m_is_get_black++;
			m_move_speed = SPEED_MIN; 
		}


		if (m_move_st != 3 && m_move_st != 4 && m_move_st != 5) {

			// 移動処理の更新
			transform.Translate (Vector3.left * m_move_speed);
		}

		// ----------
		// 移動制限
		// ----------
		if (transform.position.y < -3.0f) {

			float y = -3.0f;
			transform.position = new Vector3 (transform.position.x, y, transform.position.z);
		}
		// ----------

		float x2 = transform.position.x;
		m_move_dist = x2 - x1;
	}



	/* --------------------------------------------------
	 * @回転
	*/
	void Rotate(){

		// ------------------------------
		// // パッドの入力角度を逆転させるストッパー
		// ※右向きのときパッドの入力角度をそのまま反映させると逆回転になるため※
		if (m_motion_st == 1) {
			if (m_deg > 0.0f) {
				m_deg *= -1.0f;
				m_deg += 180.0f;
			}
			if (m_deg <= 0.0f) {
				m_deg += 360.0f;
			}
		}
		// ------------------------------

		// ※ステータスストッパー※
		if (m_move_st == 1) {

			// 入力した方向を向く
			if (m_rot.z != m_deg) {
				// プレイヤーの向きと入力角度が異なるならば

				if (m_rot.z > m_deg) {
					// 入力角度のほうが小さいならば

					// 内角と外角の差を比較して小さいほうに回転させる
					float r1 = m_rot.z - m_deg;				// 内角
					float r2 = 360.0f - m_rot.z + m_deg; 	// 外角
					if (r1 < r2) {
						m_rot.z -= (r1 / 360.0f) * 25.0f;		// 左回転（回転の速度）

					} else if (r1 > r2) {
						m_rot.z += (r1 / 360.0f) * 25.0f;		// 右回転（回転の速度）

					}

				} else if (m_rot.z < m_deg) {
					// 入力角度のほうが大きいならば

					// 内角と外角の差を比較して小さいほうに回転させる
					float r1 = m_deg - m_rot.z;				// 内角
					float r2 = 360.0f - m_deg + m_rot.z; 	// 外角
					if (r1 < r2) {
						m_rot.z += (r1 / 360.0f) * 30.0f;		// 右回転（回転の速度）

					} else if (r1 > r2) {
						m_rot.z -= (r1 / 360.0f) * 30.0f;		// 左回転（回転の速度）

					}
				}
			}

			// プレイヤーの向きの制限（0~360度）
			if (m_rot.z < 0.0f) {
				m_rot.z = 360.0f;
			} else if (m_rot.z > 360.0f) {
				m_rot.z = 0.0f;
			}

			// 回転処理の反映
			transform.rotation = Quaternion.Euler (m_rot);
		}
	}



	/* --------------------------------------------------
	 * @モーション管理
	*/
	void MotionControl(){
		
		// 「移動・回転モーション」の呼び出し
		MotionMove ();

		// 「落下モーション」の呼び出し
		MotionFall();

		// 「着陸モーション」の呼び出し
		MotionTakeDown();

		// 頭
		Vector3 head_rot = new Vector3(m_rot.x, m_rot.y, m_motion_head_rot_z);
		transform.FindChild ("player_head_001.2").transform.rotation = Quaternion.Euler (head_rot);
		transform.FindChild ("player_head_001.3").transform.rotation = Quaternion.Euler (head_rot);
		transform.FindChild ("player_head_001.4").transform.rotation = Quaternion.Euler (head_rot);
		// フード
		head_rot.z += Random.Range(0.0f, m_move_speed * 100.0f);
		transform.FindChild ("player_head_001.1").transform.rotation = Quaternion.Euler (head_rot);

		// 体
		Vector3 body_rot = new Vector3(m_rot.x, m_rot.y, m_motion_body_rot_z);
		transform.FindChild ("player_body_001.1").transform.rotation = Quaternion.Euler (body_rot);
		transform.FindChild ("player_body_001.2").transform.rotation = Quaternion.Euler (body_rot);
		// マント
		body_rot.z += Random.Range(0.0f, m_move_speed * 100.0f);
		transform.FindChild ("player_mant_001").gameObject.transform.rotation = Quaternion.Euler (body_rot);

		// 足
		Vector3 leg_rot = new Vector3(m_rot.x, m_rot.y, m_motion_leg_rot_z);
		transform.FindChild ("player_leg_001.1").transform.rotation = Quaternion.Euler (leg_rot);
		transform.FindChild ("player_leg_001.2").transform.rotation = Quaternion.Euler (leg_rot);
		transform.FindChild ("player_leg_002.1").transform.rotation = Quaternion.Euler (leg_rot);
		transform.FindChild ("player_leg_002.2").transform.rotation = Quaternion.Euler (leg_rot);
		// ------------------------------
	}



	/* --------------------------------------------------
	 * @移動・回転モーション
	 * @宙返りやスピードを伴う移動の際に風や重力の影響を受けて飛行しているモーションを表現する
	*/
	void MotionMove(){

		// ※ステータスストッパー※
		if (m_move_st != 4 && m_move_st != 5) {

			// モーション：頭
			m_motion_head_rot_z = m_rot.z;

			// モーション：体、マント
			m_motion_body_rot_z = m_rot.z + (m_move_speed * 1000.0f);

			// モーション：足
			m_motion_leg_rot_z = m_rot.z + (m_move_speed * 1000.0f);
		}
	}



	/* --------------------------------------------------
	 * @ジャンプモーション
	 * @移動始めにアニメーションで半回転または全回転を表現する
	*/
	void MotionJump(){

		// マウスのスクリーン座標取得
		Vector3 mouse_screen_pos = Input.mousePosition;
		mouse_screen_pos.z = transform.position.z - Camera.main.transform.position.z;
		// マウスのワールド座標取得
		Vector3 mouse_world_pos = Camera.main.ScreenToWorldPoint (mouse_screen_pos);

		// ------------------------------
		// ステータス遷移
		// ------------------------------
		if (m_motion_st == 0) {
			// デフォルト

			m_motion_st = 1;

		} else if (m_motion_st == 1) {
			// 右向き

			// ----------
			// 画像をひっくり返す
			m_rot.y = 180.0f;

			// タップの座標が左だったならば
			if (mouse_world_pos.x < transform.position.x) {

				// ダミーの向きを合わせて
				m_dummy_tsf.transform.rotation = Quaternion.Euler (m_rot.x, 180.0f, m_rot.z);

				// 半回転のアニメーションをセット
				m_roll_anim.SetTrigger ("RollHalf");

				// プレイヤーを透明に
				ColorControl (0.0f, 1.0f);

				// ハーフロールのステータスへ
				m_motion_st = 3;

			} else {

				m_dummy_tsf.transform.rotation = Quaternion.Euler (m_rot.x, 180.0f, m_rot.z);

				// 全回転のアニメーションをセット
				m_roll_anim.SetTrigger ("RollAll");

				ColorControl (0.0f, 1.0f);

				// オールロールのステータスへ
				m_motion_st = 4;
			}
			// ----------

		} else if (m_motion_st == 2) {
			// 左向き

			// ----------
			// 画像をひっくり返さない
			m_rot.y = 0.0f;

			// タップの座標が左だったならば
			if (mouse_world_pos.x > transform.position.x) {

				// ダミーの向きを合わせて
				m_dummy_tsf.transform.rotation = Quaternion.Euler (m_rot.x, 0.0f, m_rot.z);

				// 半回転のアニメーションをセット
				m_roll_anim.SetTrigger ("RollHalf");

				// プレイヤーを透明に
				ColorControl (0.0f, 1.0f);

				// ハーフロールのステータスへ
				m_motion_st = 3;

			} else {

				m_dummy_tsf.transform.rotation = Quaternion.Euler (m_rot.x, 0.0f, m_rot.z);

				// 全回転のアニメーションをセット
				m_roll_anim.SetTrigger ("RollAll");

				ColorControl (0.0f, 1.0f);

				// オールロールのステータスへ
				m_motion_st = 4;
			}
			// ----------

		} else if (m_motion_st == 3) {
			// ハーフロール

			// カウントの加算
			m_motion_cnt++;
			if (m_motion_cnt > 58) {

				// ダミーを透明に
				ColorControl (1.0f, 0.0f);

				// ----------
				if (m_rot.y == 180.0f) {
					// 右向きならば

					// 左向きに
					m_rot.y = 0.0f;
					m_motion_cnt = 0;
					m_motion_st = 2;

					// 移動のステータスへ
					m_move_st = 1;

				} else {
					// 左向きならば

					// 右向きに
					m_rot.y = 180.0f;
					m_motion_cnt = 0;
					m_motion_st = 1;

					// 移動のステータスへ
					m_move_st = 1;
				}
				// ----------
			}

		} else if (m_motion_st == 4) {
			// オールロール

			// カウントの加算
			m_motion_cnt++;
			if (m_motion_cnt > 58) {
				
				// ダミーを透明に
				ColorControl (1.0f, 0.0f);

				if (m_rot.y == 180.0f) {
					// 右向きならば

					// 右向きに
					m_motion_cnt = 0;
					m_motion_st = 1;

					// 移動のステータスへ
					m_move_st = 1;

				} else {
					// 左向きならば

					// 左向きに
					m_rot.y = 0.0f;
					m_motion_cnt = 0;
					m_motion_st = 2;

					// 移動のステータスへ
					m_move_st = 1;
				}
			}
		}
		// ------------------------------
	}



	/* --------------------------------------------------
	 * @落下モーション
	 * @ダメージを受けた後または停止した後に落下していくモーションを表現する
	*/
	void MotionFall(){

		// ※ステータスストッパー※
		if (m_move_st == 4 && m_move_cnt <= 120) {
			
			// モーション：頭
			m_motion_head_rot_z = m_rot.z + 50.0f;

			// モーション：体、マント
			m_motion_body_rot_z = m_rot.z - 70.0f;

			// モーション：足
			m_motion_leg_rot_z = m_rot.z - 50.0f;
		}
	}



	/* --------------------------------------------------
	 * @着陸モーション
	*/
	void MotionTakeDown(){

		// ※ステータスストッパー※
		if (m_move_st == 5) {

			// モーション：頭
			m_motion_head_rot_z = m_rot.z + 50.0f;

			// モーション：体、マント
			m_motion_body_rot_z = m_rot.z;

			// モーション：足
			m_motion_leg_rot_z = m_rot.z;
		}
	}



	/* --------------------------------------------------
	 * @しっぽかけらの処理
	 * @しっぽかけらをプレイヤーの移動に合わせてついてこさせる（※へびのしっぽのように※）
	 * @毎フレームごとにプレイヤーの現在座標を配列に格納していく
	 * @	→取得したかけらを配列のオブジェクトにいれて座標を更新する
	*/
	void TailPieceControl(){
		
		int tailpiece_cnt = 0;

		// ----------
		// しっぽかけらのパラメータ別
		SPEED_DEF = m_move_speed_setting;
		for (int i = 0; i < TAILPIECE_MAX; i++) {
			if (m_tailpiece_obj [i] != null) {

				m_tailpiece_obj [i].transform.localRotation = Quaternion.Euler(new Vector3 (0.0f, 0.0f, 0.0f));

				if (m_tailpiece_obj [i].GetComponent<PieceScr> ().m_st == 1) {
					// 白色
				} else if (m_tailpiece_obj [i].GetComponent<PieceScr> ().m_st == 2) {
					// 黄色
					if (SPEED_DEF < SPEED_MAX) {
						SPEED_DEF += UP_SPEED;
					}
				} else if (m_tailpiece_obj [i].GetComponent<PieceScr> ().m_st == 3) {
					// 黒色
					if (SPEED_DEF > SPEED_MIN) {
						//SPEED_DEF -= DOWN_SPEED;
					}
				}
			}
		}
		// ----------

		// ------------------------------
		// ステータス遷移
		// ------------------------------
		for (int i = 0; i < TAILPIECE_MAX; i++) {

			if (m_tailpiece_st [i] == 0) {
				// デフォルト

				if (m_tailpiece_obj [i] != null) {

					// オブジェクトを空っぽに
					//Destroy(m_tailpiece_obj[i]);
					m_tailpiece_obj [i] = null;
				}

			} else if (m_tailpiece_st [i] == 1) {
				// プレイヤーの追従

				if (m_tailpiece_obj [i] != null) {

					// 追従の座標取得
					// ※オブジェクトの座標＝しっぽかけら座標の番号×何マスずつ＋最初何マスあけるか※
					m_tailpiece_obj [i].transform.position = m_tailpiece_pos [(i * 5) + 5];
				}

			} else if (m_tailpiece_st [i] == 2) {
				// 被弾でバラバラ

				if (m_tailpiece_obj [i] != null) {

					// しっぽかけらとプレイヤーとの距離取得
					Vector3 p1 = m_tailpiece_obj [i].transform.position;
					Vector3 p2 = transform.position;
					float dist = Vector3.Distance (p1, p2);

					// プレイヤーと反対方向にしっぽかけらを吹っ飛ばす
					m_tailpiece_obj [i].transform.position += m_tailpiece_vec [i] * (3.0f - dist) * 0.01f;

					// 回転させる
					m_tailpiece_obj [i].transform.Rotate (new Vector3 (0.0f, 0.0f, 2.0f));

					// 距離の制限
					if (dist > 3.0f) {

						// デフォルトに
						m_tailpiece_st [i] = 0;
					}
				}

			} else if (m_tailpiece_st [i] == 3) {
				// 木に吸収

				if (m_tailpiece_obj [i] != null) {

					// 木に子分化
					m_tailpiece_obj [i].transform.parent = GameObject.FindGameObjectWithTag ("Tree").transform;

					// 木に向かって移動する
					Vector3 p1 = m_tailpiece_obj [i].transform.position;
					Vector3 p2 = GameObject.FindGameObjectWithTag ("Tree").transform.position;
					p2.y -= 2.0f;  // 座標修正
					// 周囲を回転しながら移動する
					m_tailpiece_obj [i].transform.RotateAround (p2, new Vector3 (0.0f, 0.0f, 1.0f), 2.0f);
					m_tailpiece_vec [i] = p2 - p1;
					m_tailpiece_obj [i].transform.position += m_tailpiece_vec [i] * 0.01f;

					// カウントが一定を超えると
					m_tailpiece_cnt [i]++;
					if (m_tailpiece_cnt [i] > 180) {

						// しっぽかけらのデストロイ
						DestroyObject (m_tailpiece_obj [i]);

						// デフォルトに
						m_tailpiece_cnt [i] = 0;
						m_tailpiece_st [i] = 0;

						if (m_tailpiece_obj [i].GetComponent<PieceScr> ().m_st == 1) {
							GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_white_score++;
						} else if (m_tailpiece_obj [i].GetComponent<PieceScr> ().m_st == 2) {
							GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_gold_score++;
						} else if (m_tailpiece_obj [i].GetComponent<PieceScr> ().m_st == 3) {
							GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_black_score++;
						}

						// ※シーンストッパー※
						if (GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_scene >= 7) { 
							
							test_score++;
							if (test_score % 2 == 0) {

								// スコアの加算
								GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_score++;
								break;
							}

						} else {

							// スコアの加算
							GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_score++;
							break;
						}
					}
				}
			} 
		}
		// ------------------------------


		/*
		for (int i = 0; i < TAILPIECE_MAX; i++) {
			if (m_tailpiece_obj [i] != null) {
				tailpiece_cnt++;
			}
		}			
		if (tailpiece_cnt == TAILPIECE_MAX) {
			for (int i = 0; i < TAILPIECE_MAX; i++) {
				if (i % 2 == 0) {
					if (m_tailpiece_obj [i] != null) {
						m_tailpiece_obj [i].GetComponent<SpriteRenderer> ().sprite = m_dummy_blockpiece_obj;
						m_tailpiece_obj [i].transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
						m_audio_source.Stop ();
						m_audio_source.clip = m_se_get_2;
						m_audio_source.Play ();
					}
				}
			}
		}
		*/

		// ----------
		// しっぽかけらの座標取得
		for (int i = TAILPIECE_POS_MAX - 1 - 1; i >= 0; i--) {

			// 
			m_tailpiece_pos [i + 1] = m_tailpiece_pos [i];
		}
		m_tailpiece_pos [0] = transform.position;
		// ----------

		/*
		for(int i = 0; i < TAILPIECE_MAX; i++){
			if (i != 0) {
				if (m_tailpiece_obj [i - 1] == null) {
					if (m_tailpiece_obj [i] != null) {
						
						m_tailpiece_obj [i - 1] = Instantiate (m_dummy_piece_obj) as GameObject;
						m_tailpiece_st [i - 1] = 1;
						m_tailpiece_obj [i - 1].GetComponent<PieceScr> ().m_st = m_tailpiece_obj [i].GetComponent<PieceScr> ().m_st;
						m_tailpiece_obj [i - 1].GetComponent<PieceScr> ().m_is_tailpiece = true;
						DestroyObject (m_tailpiece_obj [i]);

						m_tailpiece_obj [i].transform.position = m_tailpiece_pos [((i - 1) * 5) + 5];
						m_tailpiece_obj [i - 1] = m_tailpiece_obj [i];
						m_tailpiece_st [i] = 0;
					}
				}
			}
		}*/
	}



	/* --------------------------------------------------
	 * @衝突判定
	*/
	void OnTriggerEnter2D (Collider2D collider)
	{

		// ※シーンストッパー※
		int scene = GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_scene;
		if (scene != 0 && scene != 1 && scene != 11 && scene != 12 && scene != 20) {

			// ※ステータスストッパー※
			if (m_move_st == 1 || m_move_st == 2) {

				// ----------
				// 衝突判定：かけら
				// ----------
				if (collider.gameObject.tag == "Piece") {

					// 衝突したかけらの取得がまだであるならば
					if (collider.gameObject.GetComponent<PieceScr> ().m_is_tailpiece == false) {
					
						// ※配列を回し空っぽのオブジェクトがあるならばそこに衝突したかけらを格納する※
						for (int i = 0; i < TAILPIECE_MAX; i++) {

							// オブジェクトの中身が空っぽならば
							if (m_tailpiece_obj [i] == null && m_tailpiece_st [i] == 0) {
							
								// SE再生
								if (collider.gameObject.GetComponent<PieceScr> ().m_st == 1) {

									// エフェクト再生
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().Stop ();
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().startColor = Color.green;
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().Play ();

									m_audio_source.Stop ();
									m_audio_source.clip = m_se_get_1;
									m_audio_source.Play ();
								} else if (collider.gameObject.GetComponent<PieceScr> ().m_st == 2) {
									// エフェクト再生
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().Stop ();
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().startColor = Color.yellow;
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().Play ();
									m_audio_source.Stop ();
									m_audio_source.clip = m_se_get_2;
									m_audio_source.Play ();
								} else if (collider.gameObject.GetComponent<PieceScr> ().m_st == 3) {
									// エフェクト再生
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().Stop ();
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().startColor = Color.black;
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().Play ();
									m_audio_source.Stop ();
									m_audio_source.clip = m_se_get_3;
									m_audio_source.Play ();
								}
						
								// 衝突したかけらを配列のオブジェクトに格納する
								m_tailpiece_obj [i] = collider.gameObject;

								// ステータスを追従に
								m_tailpiece_st [i] = 1;

								// かけらをしっぽかけらに
								collider.gameObject.GetComponent<PieceScr> ().m_is_tailpiece = true;

								// タグをしっぽかけらに
								collider.gameObject.tag = "TailPiece";

								if (collider.gameObject.GetComponent<PieceScr> ().m_st == 3) {
									m_is_get_black = 0;
								}

								// ※上記の処理は一回でかまわないので※
								break;
							}
						}
					}
				}
			
				// ----------

				// ----------
				// 衝突判定：かけら（固まり）
				// ----------
				if (collider.gameObject.tag == "BlockPiece") {

					// 衝突したかけらの取得がまだであるならば
					if (collider.gameObject.GetComponent<PieceScr> ().m_is_tailpiece == false) {

						// ※配列を回し空っぽのオブジェクトがあるならばそこに衝突したかけらを格納する※
						for (int i = 0; i < TAILPIECE_MAX; i++) {
						
							// オブジェクトの中身が空っぽならば
							if (m_tailpiece_obj [i] == null && m_tailpiece_st [i] == 0) {

								// SE再生
								if (collider.gameObject.GetComponent<PieceScr> ().m_st == 1) {

									m_dummy_piece_obj.GetComponent<PieceScr> ().m_st = 1;

									// エフェクト再生
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().Stop ();
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().startColor = Color.green;
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().Play ();

									m_audio_source.Stop ();
									m_audio_source.clip = m_se_get_1;
									m_audio_source.Play ();
								} else if (collider.gameObject.GetComponent<PieceScr> ().m_st == 2) {
									m_dummy_piece_obj.GetComponent<PieceScr> ().m_st = 2;
									// エフェクト再生
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().Stop ();
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().startColor = Color.yellow;
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().Play ();
									m_audio_source.Stop ();
									m_audio_source.clip = m_se_get_2;
									m_audio_source.Play ();
								} else if (collider.gameObject.GetComponent<PieceScr> ().m_st == 3) {
									m_dummy_piece_obj.GetComponent<PieceScr> ().m_st = 3;
									// エフェクト再生
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().Stop ();
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().startColor = Color.black;
									transform.FindChild ("GetEffect").GetComponent<ParticleSystem> ().Play ();
									m_audio_source.Stop ();
									m_audio_source.clip = m_se_get_3;
									m_audio_source.Play ();
								}

								int number = 0;

								if (i <= (TAILPIECE_MAX - 4)) {
									number = 4;
								} else if (i == (TAILPIECE_MAX - 3)) {
									number = 3;
								} else if (i == (TAILPIECE_MAX - 2)) {
									number = 2;
								} else if (i == (TAILPIECE_MAX - 1)) {
									number = 1;
								}

								// 
								m_tailpiece_obj [i] = Instantiate (m_dummy_piece_obj) as GameObject;

								// 
								m_tailpiece_st [i] = 1;

								//
								m_tailpiece_obj [i].GetComponent<PieceScr> ().m_is_tailpiece = true;

								// 
								m_tailpiece_obj [i].tag = "TailPiece";

								if (number == 4) {
									m_tailpiece_obj [i + 1] = Instantiate (m_dummy_piece_obj, transform.position, Quaternion.identity) as GameObject;
									m_tailpiece_obj [i + 2] = Instantiate (m_dummy_piece_obj, transform.position, Quaternion.identity) as GameObject;
									m_tailpiece_obj [i + 3] = Instantiate (m_dummy_piece_obj, transform.position, Quaternion.identity) as GameObject;
									m_tailpiece_st [i + 1] = 1;
									m_tailpiece_st [i + 2] = 1;
									m_tailpiece_st [i + 3] = 1;
									m_tailpiece_obj [i + 1].GetComponent<PieceScr> ().m_is_tailpiece = true;
									m_tailpiece_obj [i + 2].GetComponent<PieceScr> ().m_is_tailpiece = true;
									m_tailpiece_obj [i + 3].GetComponent<PieceScr> ().m_is_tailpiece = true;
									m_tailpiece_obj [i + 1].tag = "TailPiece";
									m_tailpiece_obj [i + 2].tag = "TailPiece";
									m_tailpiece_obj [i + 3].tag = "TailPiece";
								} else if (number == 3) {
									m_tailpiece_obj [i + 1] = Instantiate (m_dummy_piece_obj) as GameObject;
									m_tailpiece_obj [i + 2] = Instantiate (m_dummy_piece_obj) as GameObject;
									m_tailpiece_st [i + 1] = 1;
									m_tailpiece_st [i + 2] = 1;
									m_tailpiece_obj [i + 1].GetComponent<PieceScr> ().m_is_tailpiece = true;
									m_tailpiece_obj [i + 2].GetComponent<PieceScr> ().m_is_tailpiece = true;
									m_tailpiece_obj [i + 1].tag = "TailPiece";
									m_tailpiece_obj [i + 2].tag = "TailPiece";
								} else if (number == 2) {
									m_tailpiece_obj [i + 1] = Instantiate (m_dummy_piece_obj) as GameObject;
									m_tailpiece_st [i + 1] = 1;
									m_tailpiece_obj [i + 1].GetComponent<PieceScr> ().m_is_tailpiece = true;
									m_tailpiece_obj [i + 1].tag = "TailPiece";
								} else if (number == 1) {

								}

								// 
								Destroy (collider.gameObject);

								if (collider.gameObject.GetComponent<PieceScr> ().m_st == 3) {
									m_is_get_black = 0;
								}

								// ※上記の処理は一回でかまわないので※
								break;
							}
						}
					}
				}
			
				// ----------
			}

			// ----------
			// 衝突判定：鳥
			// ----------
			if (collider.gameObject.tag == "Bird") {

				for (int i = 0; i < TAILPIECE_MAX; i++) {

					if (m_tailpiece_obj [i] != null && m_tailpiece_st [i] == 1) {
				
						// 
						Vector3 p1 = m_tailpiece_obj [i].transform.position;
						float x = Random.Range (p1.x - 20.0f, p1.x + 20.0f);
						float y = Random.Range (p1.y - 20.0f, p1.y + 20.0f);
						Vector3 p2 = new Vector3 (x, y, p1.z);
						m_tailpiece_vec [i] = p2 - p1;

						// 
						m_tailpiece_obj [i].GetComponent<PieceScr> ().m_is_tailpiece = false;

						// 
						m_tailpiece_obj [i].tag = "Piece";

						// 
						m_tailpiece_st [i] = 2;
					}
				}
				
				// 「透明」の呼び出し
				ColorControl (1.0f, 0.0f);

				// 
				m_move_cnt = 0;
				m_move_st = 4;
			}
			// ----------
		}
	}
}
		
		
