using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonScr : MonoBehaviour {

	public bool is_button_push;
	public Sprite help_a;
	public Sprite help_b;

	// Use this for initialization
	void Start () {
		is_button_push = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		//GameObject.FindGameObjectWithTag ("GameMain").GetComponent<GameMainScr> ().is_help = is_button_push;
		int scene = GameObject.Find ("GameMain").GetComponent<GameMainScr> ().m_scene;
		if (scene != 0 && scene != 1 && scene != 12 && scene != 20) {
			GetComponent<RectTransform> ().localPosition = new Vector3 (800.0f, 1400.0f, 0.0f);
			if (is_button_push) {
				this.GetComponent<Image> ().sprite = help_b; 
			} else {
				this.GetComponent<Image> ().sprite = help_a; 
			}
		} else {
			GetComponent<RectTransform> ().localPosition = new Vector3 (0.0f, -3000.0f, 0.0f);
		}
	}

	public void ButtonPush(){
		if (is_button_push) {
			is_button_push = false;
		} else {
			is_button_push = true;
		}
	}
}
