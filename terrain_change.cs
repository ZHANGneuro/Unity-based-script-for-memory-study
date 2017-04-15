
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;


//[ExecuteInEditMode] 

public class terrain_change : MonoBehaviour{

	float set_timeleft = 10;  //10
	float time_for_fixation = 1;
	int total_Number_of_trial=10;
	int trial_ith=0;
	int temp_button_index = -1;
	bool IsExploring = false;
	float timeleft;
	static Vector2 button_size = new Vector2(150,85);
	float button_gap;
	static int Number_of_fence = 24;
	float angle_for_fence_rotation=0;
	float angle_precision = 15;
	float reference_coordinate_x=233.0f;
	float reference_coordinate_z=250.0f;
	float new_coordinate_x;
	float new_coordinate_z;
	string which_facing_location;
	string which_balloon;
	string which_goal_location;
	string which_balloon_to_show;

	List<string> facing_position = new List<string>();
	List<string> goal_location = new List<string>();
	List<string> balloon = new List<string>();
	List<int> trial_order = new List<int>();
	List<int> button_ith_order = new List<int>();
	List<float> button_position_order = new List<float>();
	List<Vector3> item_location = new List<Vector3>();

	Vector3 facing_direction_1 = new Vector3(Mathf.Sin ((330 * Mathf.PI) / 180), 0, Mathf.Cos ((330 * Mathf.PI) / 180));
	Vector3 facing_direction_2 = new Vector3(-1,0,0);
	Vector3 facing_direction_3 = new Vector3(Mathf.Sin ((210 * Mathf.PI) / 180), 0, Mathf.Cos ((210 * Mathf.PI) / 180));
	Vector3 facing_direction_4 = new Vector3(Mathf.Sin ((150 * Mathf.PI) / 180), 0, Mathf.Cos ((150 * Mathf.PI) / 180));
	Vector3 facing_direction_5 = new Vector3(1,0,0);
	Vector3 facing_direction_6 = new Vector3(Mathf.Sin ((30 * Mathf.PI) / 180), 0, Mathf.Cos ((30 * Mathf.PI) / 180));
	Vector3 subject_initial_position= new Vector3 (250, 0, 200);
	Vector3 subject_response_position= new Vector3 (250, 0, 252);
	Vector3 button1_position;
	Vector3 button2_position;
	Vector3 button3_position;
	Vector3 button4_position;
	Vector3 button5_position;
	Vector3 current_facing_location;
	Vector3 current_random_location;
	Vector3 current_goal_location;

	GameObject canvasobject;
	GameObject textObject;
	GameObject buttonObject1;
	GameObject buttonObject2;
	GameObject buttonObject3;
	GameObject buttonObject4;
	GameObject buttonObject5;
	GameObject facing_balloon;
	GameObject random_balloon;
	GameObject goal_balloon;

	Button button_1;
	Button button_2;
	Button button_3;
	Button button_4;
	Button button_5;

	public GameObject button_prefab1;
	public GameObject button_prefab2;
	public GameObject button_prefab3;
	public GameObject button_prefab4;
	public GameObject button_prefab5;
	public GameObject FPScontroller;
	public GameObject Fence_long;
	public GameObject Fence_short;
	public GameObject ballon_prefab_1;
	public GameObject ballon_prefab_2;
	public GameObject ballon_prefab_3;
	public GameObject mountain;

	void Start(){
		Screen.SetResolution (1024,768,false,60);
		item_location.Add (new Vector3(242,3,266));
		item_location.Add (new Vector3(235,3,252));
		item_location.Add (new Vector3(242.5f,3,239));
		item_location.Add (new Vector3(258,3,238));
		item_location.Add (new Vector3(266,3,252));
		item_location.Add (new Vector3(258,3,266));
		randomize_button_position ();
		create_table ();
		create_environment ();
		Invoke ("fixation",0);
	}

	void randomize_button_position(){
		List<int> temp_list = new List<int>();
		temp_list = Enumerable.Range(0,5).ToList();
		button_ith_order = ShuffleList (temp_list);
		button_gap = (Screen.width-button_size.x*5)/8;
		button_position_order.Add (2.0f * button_gap + button_size.x / 2);
		button_position_order.Add (3.0f * button_gap + button_size.x + button_size.x / 2);
		button_position_order.Add (4.0f * button_gap + button_size.x * 2.0f + button_size.x / 2);
		button_position_order.Add (5.0f * button_gap + button_size.x * 3.0f + button_size.x / 2);
		button_position_order.Add (6.0f * button_gap + button_size.x * 4.0f + button_size.x / 2);
		button1_position = new Vector3(button_position_order.ElementAt(button_ith_order.ElementAt(0)),80,0);
		button2_position = new Vector3(button_position_order.ElementAt(button_ith_order.ElementAt(1)),80,0);
		button3_position = new Vector3(button_position_order.ElementAt(button_ith_order.ElementAt(2)),80,0);
		button4_position = new Vector3(button_position_order.ElementAt(button_ith_order.ElementAt(3)),80,0);
		button5_position = new Vector3(button_position_order.ElementAt(button_ith_order.ElementAt(4)),80,0);
	}
		
	void create_environment(){
		Instantiate (FPScontroller, subject_initial_position, Quaternion.Euler (0, 0, 0));
		mountain.transform.localScale = new Vector3 (0.2f,0.2f,0.2f);
		Instantiate (mountain, new Vector3 (250, 7, 450), Quaternion.Euler (0, 270, 0));
		// create fence
		for (int i = 0; i < Number_of_fence; i++) {
			if (i == 0) {
				new_coordinate_x = reference_coordinate_x;
				new_coordinate_z = reference_coordinate_z;
				Instantiate (Fence_long, new Vector3 (new_coordinate_x, 0.0f, new_coordinate_z), Quaternion.Euler (0, angle_for_fence_rotation, 0));
			} 
			if (i != 0) { 
				angle_for_fence_rotation = angle_for_fence_rotation + angle_precision;
				if (i == 1) {
					new_coordinate_x = reference_coordinate_x;
					new_coordinate_z = reference_coordinate_z + 4.5f;
					Instantiate(Fence_long, new Vector3(reference_coordinate_x, 0.0f, new_coordinate_z), Quaternion.Euler(0, angle_for_fence_rotation, 0));
				}
				if (i > 1) {
					new_coordinate_x = new_coordinate_x + 4.5f * Mathf.Sin (((angle_for_fence_rotation-angle_precision) * Mathf.PI)/180);
					new_coordinate_z = new_coordinate_z + 4.5f * Mathf.Cos (((angle_for_fence_rotation-angle_precision) * Mathf.PI)/180);
					if(i!=18){
						Instantiate(Fence_long, new Vector3(new_coordinate_x, 0.0f, new_coordinate_z), Quaternion.Euler(0, angle_for_fence_rotation, 0));
					}
					if(i==18){
						Instantiate(Fence_short, new Vector3(250.57f, 0, 236.97f), Quaternion.Euler(0, 150, 0));
						Instantiate(Fence_short, new Vector3(248.62f, 0, 237.31f), Quaternion.Euler(0, 200, 0));
					}
				}
			}
		}
	}
		
	void Update(){

		if (IsExploring) {
			timeleft = timeleft - Time.deltaTime;
			if (timeleft <= 0) {
				IsExploring = false;  
				FirstPersonController.NO_MOVE = true;
				Invoke ("imagining",0);
			}
		}
		if(!IsExploring){
			if(Input.GetKeyDown(KeyCode.Alpha1)){
				temp_button_index = button_ith_order.IndexOf (0);
			}
			if(Input.GetKeyDown(KeyCode.Alpha2)){
				temp_button_index = button_ith_order.IndexOf (1);
			}
			if(Input.GetKeyDown(KeyCode.Alpha3)){
				temp_button_index = button_ith_order.IndexOf (2);
			}
			if(Input.GetKeyDown(KeyCode.Alpha4)){
				temp_button_index = button_ith_order.IndexOf (3);
			}
			if(Input.GetKeyDown(KeyCode.Alpha5)){
				temp_button_index = button_ith_order.IndexOf (4);
			}

			if(temp_button_index==0){
				temp_button_index = -1;
				if (trial_ith<total_Number_of_trial){
					button_1.onClick.Invoke();
					button_1.GetComponent<Image>().color = Color.white;
					trial_ith = trial_ith + 1;
					Invoke ("fixation",0.5f);
				} else {
					thank_you ();
				}
			}
			if(temp_button_index==1){
				temp_button_index = -1;
				if (trial_ith<total_Number_of_trial){
					button_2.onClick.Invoke();
					button_2.GetComponent<Image>().color = Color.white;
					trial_ith = trial_ith + 1;
					Invoke ("fixation",0.5f);
				} else {
					thank_you ();
				}
			}
			if(temp_button_index==2){
				temp_button_index = -1;
				if (trial_ith<total_Number_of_trial){
					button_3.onClick.Invoke();
					button_3.GetComponent<Image>().color = Color.white;
					trial_ith = trial_ith + 1;
					Invoke ("fixation",0.5f);
				} else {
					thank_you ();
				}
			}
			if(temp_button_index==3){
				temp_button_index = -1;
				if (trial_ith<total_Number_of_trial){
					button_4.onClick.Invoke();
					button_4.GetComponent<Image>().color = Color.white;
					trial_ith = trial_ith + 1;
					Invoke ("fixation",0.5f);
				} else {
					thank_you ();
				}
			}
			if(temp_button_index==4){
				temp_button_index = -1;
				if (trial_ith<total_Number_of_trial){
					button_5.onClick.Invoke();
					button_5.GetComponent<Image>().color = Color.white;
					trial_ith = trial_ith + 1;
					Invoke ("fixation",0.5f);
				} else {
					thank_you ();
				}
			}
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit ();
		}
	}

	void fixation(){
		Destroy (canvasobject);
		canvasobject = new GameObject ("Canvas");
		var canvas = canvasobject.AddComponent<Canvas> ();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		var blackscreen = canvasobject.AddComponent<Image> ();
		blackscreen.rectTransform.sizeDelta = new Vector2 (1024, 768);
		blackscreen.rectTransform.anchoredPosition = Vector3.zero;
		blackscreen.color = Color.black;
		textObject = new GameObject("Text");
		textObject.transform.SetParent(canvas.transform);
		var text = textObject.AddComponent<Text>();
		text.horizontalOverflow = HorizontalWrapMode.Overflow;
		text.alignment = TextAnchor.MiddleCenter;
		text.font = Resources.FindObjectsOfTypeAll<Font>()[0];
		text.color = Color.white;
		text.text = "+";
		text.fontSize = 80;
		Destroy (buttonObject1);
		Destroy (buttonObject2);
		Destroy (buttonObject3);
		Destroy (buttonObject4);
		Destroy (buttonObject5);
		Destroy (facing_balloon);
		Destroy (random_balloon);
		Destroy (goal_balloon);
		Invoke ("remove_fixation", time_for_fixation);
	}

	void remove_fixation(){
		Destroy (canvasobject);
		Destroy (textObject);
		create_balloon ();
		FirstPersonController.m_CharacterController.transform.position = subject_initial_position;
		FirstPersonController.NO_MOVE = false;
		timeleft = set_timeleft;
		IsExploring = true;
	}

	void imagining(){
		canvasobject = new GameObject ("Canvas");
		var canvas = canvasobject.AddComponent<Canvas> ();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		var blackscreen = canvasobject.AddComponent<Image> ();
		blackscreen.rectTransform.sizeDelta = new Vector2 (1024, 768);
		blackscreen.rectTransform.anchoredPosition = Vector3.zero;
		blackscreen.color = Color.black;
		textObject = new GameObject("Text");
		textObject.transform.SetParent(canvas.transform);
		var text = textObject.AddComponent<Text>();
		text.horizontalOverflow = HorizontalWrapMode.Overflow;
		text.verticalOverflow = VerticalWrapMode.Overflow;
		text.alignment = TextAnchor.MiddleCenter;
		text.font = Resources.FindObjectsOfTypeAll<Font>()[0];
		text.color = Color.white;
		text.text = "Imagine the environment";
		text.fontSize = 50;
		Invoke ("remove_imagination", 2);
	}

	void remove_imagination(){
		Destroy (canvasobject);
		Destroy (textObject);

		FirstPersonController.m_CharacterController.transform.position = subject_response_position;

		if (string.Equals (which_facing_location, "fp1")) {
			FirstPersonController.m_CharacterController.transform.forward = facing_direction_1;
			Camera.main.transform.forward = facing_direction_1; 
		}
		if (string.Equals (which_facing_location, "fp2")) {
			FirstPersonController.m_CharacterController.transform.forward = facing_direction_2;
			Camera.main.transform.forward = facing_direction_2; 
		}
		if (string.Equals (which_facing_location, "fp3")) {
			FirstPersonController.m_CharacterController.transform.forward = facing_direction_3;
			Camera.main.transform.forward = facing_direction_3; 
		}
		if (string.Equals (which_facing_location, "fp4")) {
			FirstPersonController.m_CharacterController.transform.forward = facing_direction_4;
			Camera.main.transform.forward = facing_direction_4; 
		}
		if (string.Equals (which_facing_location, "fp5")) {
			FirstPersonController.m_CharacterController.transform.forward = facing_direction_5;
			Camera.main.transform.forward = facing_direction_5; 
		}
		if (string.Equals (which_facing_location, "fp6")) {
			FirstPersonController.m_CharacterController.transform.forward = facing_direction_6;
			Camera.main.transform.forward = facing_direction_6; 
		}
		randomize_button_position ();
		show_question ();
	}

	void show_question(){

		canvasobject = new GameObject ("Canvas");
		var canvas = canvasobject.AddComponent<Canvas> ();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;

		var Image_object = new GameObject("Image");
		var image = Image_object.AddComponent<Image>();
		image.transform.SetParent(canvas.transform);
		image.transform.position = new Vector3 (Screen.width/2,Screen.height-50,0);
		image.rectTransform.sizeDelta = new Vector2(270, 80);
		image.color = new Color(0.973f, 0.973f, 1.000f,0.4f);
		var textObject = new GameObject("Text");
		textObject.transform.parent = image.transform;
		var text = textObject.AddComponent<Text>();
		text.horizontalOverflow = HorizontalWrapMode.Overflow;
		text.font = Resources.FindObjectsOfTypeAll<Font>()[0];
		text.fontSize = 40;
		text.rectTransform.anchoredPosition = new Vector2 (0.5f,0.5f);
		text.alignment = TextAnchor.MiddleCenter;
		text.text = which_balloon_to_show;
		text.color = Color.black;

		buttonObject1 = (GameObject)Instantiate (button_prefab1);
		buttonObject2 = (GameObject)Instantiate (button_prefab2);
		buttonObject3 = (GameObject)Instantiate (button_prefab3);
		buttonObject4 = (GameObject)Instantiate (button_prefab4);
		buttonObject5 = (GameObject)Instantiate (button_prefab5);

		button_1 = buttonObject1.GetComponent<Button> ();
		button_1.transform.SetParent(canvas.transform);
		button_1.onClick.AddListener (onclick1);
		button_1.transform.position = button1_position;
		button_1.image.rectTransform.sizeDelta = button_size;
		button_1.GetComponent<Image>().color = new Color(0.973f, 0.973f, 1.000f,0.9f);
		var text1 = button_1.transform.GetChild(0).GetComponent<Text>();
		text1.text = "Left\nForward";
		text1.fontSize = 30;

		button_2 = buttonObject2.GetComponent<Button> ();
		button_2.transform.SetParent(canvas.transform);
		button_2.onClick.AddListener (onclick2);
		button_2.transform.position = button2_position;
		button_2.image.rectTransform.sizeDelta = button_size;
		button_2.GetComponent<Image>().color = new Color(0.973f, 0.973f, 1.000f,0.9f);
		var text2 = button_2.transform.GetChild(0).GetComponent<Text>();
		text2.text = "Left\nBackward";
		text2.fontSize = 30;

		button_3 = buttonObject3.GetComponent<Button> ();
		button_3.transform.SetParent(canvas.transform);
		button_3.onClick.AddListener (onclick3);
		button_3.transform.position = button3_position;
		button_3.image.rectTransform.sizeDelta = button_size;
		button_3.GetComponent<Image>().color = new Color(0.973f, 0.973f, 1.000f,0.9f);
		var text3 = button_3.transform.GetChild(0).GetComponent<Text>();
		text3.text = "Back";
		text3.fontSize = 30;

		button_4 = buttonObject4.GetComponent<Button> ();
		button_4.transform.SetParent(canvas.transform);
		button_4.onClick.AddListener (onclick4);
		button_4.transform.position = button4_position;
		button_4.image.rectTransform.sizeDelta = button_size;
		button_4.GetComponent<Image>().color = new Color(0.973f, 0.973f, 1.000f,0.9f);
		var text4 = button_4.transform.GetChild(0).GetComponent<Text>();
		text4.text = "Right\nForward";
		text4.fontSize = 30;

		button_5 = buttonObject5.GetComponent<Button> ();
		button_5.transform.SetParent(canvas.transform);
		button_5.onClick.AddListener (onclick5);
		button_5.transform.position = button5_position;
		button_5.image.rectTransform.sizeDelta = button_size;
		button_5.GetComponent<Image>().color = new Color(0.973f, 0.973f, 1.000f,0.9f);
		var text5 = button_5.transform.GetChild(0).GetComponent<Text>();
		text5.text = "Right\nBackward";
		text5.fontSize = 30;
	}



	void onclick1(){
		Debug.Log ("button1 clicked");
	}
	void onclick2(){
		Debug.Log ("button2 clicked");
	}
	void onclick3(){
		Debug.Log ("button3 clicked");
	}
	void onclick4(){
		Debug.Log ("button4 clicked");
	}
	void onclick5(){
		Debug.Log ("button5 clicked");
	}


	void create_table(){
		facing_position.AddRange(Enumerable.Repeat("fp1",15));// facing location
		balloon.AddRange(Enumerable.Repeat("b1",5));
		goal_location.AddRange(Enumerable.Repeat("gl2",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl3",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl4",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl5",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl6",1));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b2",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl2",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl3",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl4",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl5",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl6",1));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b3",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl2",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl3",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl4",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl5",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl6",1));

		facing_position.InsertRange(facing_position.Count,Enumerable.Repeat("fp2",15));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b1",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl1",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl3",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl4",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl5",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl6",1));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b2",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl1",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl3",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl4",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl5",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl6",1));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b3",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl1",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl3",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl4",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl5",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl6",1));

		facing_position.InsertRange(facing_position.Count,Enumerable.Repeat("fp3",15));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b1",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl1",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl2",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl4",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl5",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl6",1));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b2",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl1",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl2",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl4",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl5",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl6",1));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b3",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl1",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl2",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl4",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl5",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl6",1));

		facing_position.InsertRange(facing_position.Count,Enumerable.Repeat("fp4",15));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b1",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl1",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl2",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl3",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl5",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl6",1));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b2",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl1",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl2",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl3",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl5",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl6",1));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b3",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl1",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl2",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl3",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl5",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl6",1));

		facing_position.InsertRange(facing_position.Count,Enumerable.Repeat("fp5",15));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b1",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl1",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl2",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl3",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl4",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl6",1));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b2",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl1",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl2",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl3",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl4",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl6",1));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b3",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl1",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl2",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl3",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl4",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl6",1));

		facing_position.InsertRange(facing_position.Count,Enumerable.Repeat("fp6",15));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b1",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl1",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl2",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl3",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl4",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl5",1));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b2",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl1",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl2",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl3",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl4",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl5",1));
		balloon.InsertRange(balloon.Count,Enumerable.Repeat("b3",5));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl1",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl2",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl3",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl4",1));
		goal_location.InsertRange(goal_location.Count,Enumerable.Repeat("gl5",1));

		List<int> temp_list = new List<int>();
		temp_list = Enumerable.Range(0,90).ToList();
		trial_order = ShuffleList (temp_list);
	}


	List<E> ShuffleList<E>(List<E> inputList)
	{
		List<E> randomList = new List<E>();
		System.Random r = new System.Random();
		int randomIndex = 0;
		while (inputList.Count > 0)
		{
			randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
			randomList.Add(inputList[randomIndex]); //add it to the new, random list
			inputList.RemoveAt(randomIndex); //remove to avoid duplicates
		}
		return randomList; //return the new random list
	}

	// get the min or max value from array;
	float find_min_from_array(float[] array){
		float minimum = array[0];
		for (int i = 0; i < array.Length; i++) {
			if (minimum > array [i]) {
				minimum = array [i];
			}
		}
		return minimum;
	}
	float find_max_from_array(float[] array){
		float minimum = array[0];
		for (int i = 0; i < array.Length; i++) {
			if (minimum < array [i]) {
				minimum = array [i];
			}
		}
		return minimum;
	}

	void thank_you(){
		Destroy (canvasobject);
		Destroy (textObject);
		canvasobject = new GameObject ("Canvas");
		var canvas = canvasobject.AddComponent<Canvas> ();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		var blackscreen = canvasobject.AddComponent<Image> ();
		blackscreen.rectTransform.sizeDelta = new Vector2 (1024, 768);
		blackscreen.color = Color.black;
		textObject = new GameObject("Text");
		textObject.transform.SetParent(canvas.transform);
		var text = textObject.AddComponent<Text>();
		text.horizontalOverflow = HorizontalWrapMode.Overflow;
		text.alignment = TextAnchor.MiddleCenter;
		text.font = Resources.FindObjectsOfTypeAll<Font>()[0];
		text.color = Color.white;
		text.text = "You have finished.\n\nThank You!";
		text.fontSize = 50;
	}


	void create_balloon (){
		int trial_index = trial_order.ElementAt (trial_ith);
		which_facing_location = facing_position.ElementAt (trial_index);
		which_balloon = balloon.ElementAt (trial_index);
		which_goal_location = goal_location.ElementAt (trial_index);

		List<int> init_for_random_position = new List<int>();
		List<int> final_for_random_position = new List<int>();

		if(string.Equals(which_facing_location,"fp1")){
			current_facing_location = item_location.ElementAt(0);
			if(string.Equals(which_goal_location,"gl2")){
				current_goal_location = item_location.ElementAt (1);
				init_for_random_position.Add (2);
				init_for_random_position.Add (3);
				init_for_random_position.Add (4);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl3")){
				current_goal_location = item_location.ElementAt (2);
				init_for_random_position.Add (1);
				init_for_random_position.Add (3);
				init_for_random_position.Add (4);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl4")){
				current_goal_location = item_location.ElementAt (3);
				init_for_random_position.Add (1);
				init_for_random_position.Add (2);
				init_for_random_position.Add (4);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl5")){
				current_goal_location = item_location.ElementAt (4);
				init_for_random_position.Add (1);
				init_for_random_position.Add (2);
				init_for_random_position.Add (3);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl6")){
				current_goal_location = item_location.ElementAt (5);
				init_for_random_position.Add (1);
				init_for_random_position.Add (2);
				init_for_random_position.Add (3);
				init_for_random_position.Add (4);
			}
		}
		if(string.Equals(which_facing_location,"fp2")){
			current_facing_location = item_location.ElementAt(1);
			if(string.Equals(which_goal_location,"gl1")){
				current_goal_location = item_location.ElementAt (0);
				init_for_random_position.Add (2);
				init_for_random_position.Add (3);
				init_for_random_position.Add (4);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl3")){
				current_goal_location = item_location.ElementAt (2);
				init_for_random_position.Add (0);
				init_for_random_position.Add (3);
				init_for_random_position.Add (4);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl4")){
				current_goal_location = item_location.ElementAt (3);
				init_for_random_position.Add (0);
				init_for_random_position.Add (2);
				init_for_random_position.Add (4);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl5")){
				current_goal_location = item_location.ElementAt (4);
				init_for_random_position.Add (0);
				init_for_random_position.Add (2);
				init_for_random_position.Add (3);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl6")){
				current_goal_location = item_location.ElementAt (5);
				init_for_random_position.Add (0);
				init_for_random_position.Add (2);
				init_for_random_position.Add (3);
				init_for_random_position.Add (4);
			}
		}
		if(string.Equals(which_facing_location,"fp3")){
			current_facing_location = item_location.ElementAt(2);
			if(string.Equals(which_goal_location,"gl1")){
				current_goal_location = item_location.ElementAt (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (3);
				init_for_random_position.Add (4);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl2")){
				current_goal_location = item_location.ElementAt (1);
				init_for_random_position.Add (0);
				init_for_random_position.Add (3);
				init_for_random_position.Add (4);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl4")){
				current_goal_location = item_location.ElementAt (3);
				init_for_random_position.Add (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (4);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl5")){
				current_goal_location = item_location.ElementAt (4);
				init_for_random_position.Add (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (3);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl6")){
				current_goal_location = item_location.ElementAt (5);
				init_for_random_position.Add (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (3);
				init_for_random_position.Add (4);
			}
		}
		if(string.Equals(which_facing_location,"fp4")){
			current_facing_location = item_location.ElementAt(3);
			if(string.Equals(which_goal_location,"gl1")){
				current_goal_location = item_location.ElementAt (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (2);
				init_for_random_position.Add (4);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl2")){
				current_goal_location = item_location.ElementAt (1);
				init_for_random_position.Add (0);
				init_for_random_position.Add (2);
				init_for_random_position.Add (4);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl3")){
				current_goal_location = item_location.ElementAt (2);
				init_for_random_position.Add (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (4);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl5")){
				current_goal_location = item_location.ElementAt (4);
				init_for_random_position.Add (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (2);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl6")){
				current_goal_location = item_location.ElementAt (5);
				init_for_random_position.Add (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (2);
				init_for_random_position.Add (4);
			}
		}
		if(string.Equals(which_facing_location,"fp5")){
			current_facing_location = item_location.ElementAt(4);
			if(string.Equals(which_goal_location,"gl1")){
				current_goal_location = item_location.ElementAt (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (2);
				init_for_random_position.Add (3);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl2")){
				current_goal_location = item_location.ElementAt (1);
				init_for_random_position.Add (0);
				init_for_random_position.Add (2);
				init_for_random_position.Add (3);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl3")){
				current_goal_location = item_location.ElementAt (2);
				init_for_random_position.Add (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (3);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl4")){
				current_goal_location = item_location.ElementAt (3);
				init_for_random_position.Add (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (2);
				init_for_random_position.Add (5);
			}
			if(string.Equals(which_goal_location,"gl6")){
				current_goal_location = item_location.ElementAt (5);
				init_for_random_position.Add (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (2);
				init_for_random_position.Add (3);
			}
		}
		if(string.Equals(which_facing_location,"fp6")){
			current_facing_location = item_location.ElementAt(5);
			if(string.Equals(which_goal_location,"gl1")){
				current_goal_location = item_location.ElementAt (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (2);
				init_for_random_position.Add (3);
				init_for_random_position.Add (4);
			}
			if(string.Equals(which_goal_location,"gl2")){
				current_goal_location = item_location.ElementAt (1);
				init_for_random_position.Add (0);
				init_for_random_position.Add (2);
				init_for_random_position.Add (3);
				init_for_random_position.Add (4);
			}
			if(string.Equals(which_goal_location,"gl3")){
				current_goal_location = item_location.ElementAt (2);
				init_for_random_position.Add (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (3);
				init_for_random_position.Add (4);
			}
			if(string.Equals(which_goal_location,"gl4")){
				current_goal_location = item_location.ElementAt (3);
				init_for_random_position.Add (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (2);
				init_for_random_position.Add (4);
			}
			if(string.Equals(which_goal_location,"gl5")){
				current_goal_location = item_location.ElementAt (4);
				init_for_random_position.Add (0);
				init_for_random_position.Add (1);
				init_for_random_position.Add (2);
				init_for_random_position.Add (3);
			}
		}
		final_for_random_position = ShuffleList (init_for_random_position);
		current_random_location = item_location.ElementAt (final_for_random_position.ElementAt (0));

		List<int> init_for_random_balloon = new List<int>();
		List<int> array_for_random_balloon = new List<int>();
		init_for_random_balloon.Add (0);
		init_for_random_balloon.Add (1);
		array_for_random_balloon = ShuffleList (init_for_random_balloon);

		if(string.Equals(which_balloon,"b1")){
			facing_balloon = Instantiate(ballon_prefab_1, current_facing_location, Quaternion.Euler(0, 0, 0));
			if (array_for_random_balloon.ElementAt (0) == 0) {
				random_balloon = Instantiate(ballon_prefab_2, current_random_location, Quaternion.Euler(0, 0, 0));
				goal_balloon = Instantiate(ballon_prefab_3, current_goal_location, Quaternion.Euler(0, 0, 0));
				which_balloon_to_show = "Blue Balloon";
			}
			if (array_for_random_balloon.ElementAt (0) == 1) {
				random_balloon = Instantiate(ballon_prefab_3, current_random_location, Quaternion.Euler(0, 0, 0));
				goal_balloon = Instantiate(ballon_prefab_2, current_goal_location, Quaternion.Euler(0, 0, 0));
				which_balloon_to_show = "Red Balloon";
			}
		}


		if(string.Equals(which_balloon,"b2")){
			facing_balloon = Instantiate(ballon_prefab_2, current_facing_location, Quaternion.Euler(0, 0, 0));
			if (array_for_random_balloon.ElementAt (0) == 0) {
				random_balloon = Instantiate(ballon_prefab_1, current_random_location, Quaternion.Euler(0, 0, 0));
				goal_balloon = Instantiate(ballon_prefab_3, current_goal_location, Quaternion.Euler(0, 0, 0));
				which_balloon_to_show = "Blue Balloon";
			}
			if (array_for_random_balloon.ElementAt (0) == 1) {
				random_balloon = Instantiate(ballon_prefab_3, current_random_location, Quaternion.Euler(0, 0, 0));
				goal_balloon = Instantiate(ballon_prefab_1, current_goal_location, Quaternion.Euler(0, 0, 0));
				which_balloon_to_show = "Yellow Balloon";
			}
		}
		if(string.Equals(which_balloon,"b3")){
			facing_balloon = Instantiate(ballon_prefab_3, current_facing_location, Quaternion.Euler(0, 0, 0));
			if (array_for_random_balloon.ElementAt (0) == 0) {
				random_balloon = Instantiate(ballon_prefab_1, current_random_location, Quaternion.Euler(0, 0, 0));
				goal_balloon = Instantiate(ballon_prefab_2, current_goal_location, Quaternion.Euler(0, 0, 0));
				which_balloon_to_show = "Red Balloon";
			}
			if (array_for_random_balloon.ElementAt (0) == 1) {
				random_balloon = Instantiate(ballon_prefab_2, current_random_location, Quaternion.Euler(0, 0, 0));
				goal_balloon = Instantiate(ballon_prefab_1, current_goal_location, Quaternion.Euler(0, 0, 0));
				which_balloon_to_show = "Yellow Balloon";
			}


		}
	}
}

