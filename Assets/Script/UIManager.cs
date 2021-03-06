﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
//using com.ootii.Messages;

public class UIManager : MonoBehaviour {

	bool isBreak;
	static public UIManager instance;
	public Button breakButton;
	public Sprite breakButtonImage;
	public Sprite buildButtonImage;
	public GameObject buttonBarContent;
	public string[] menuName;
	public GameObject animationScrollBar;
	public Vector2 scrollBounds;
	bool isBarOpen;
	public bool isPaintBarOut;
	public GameObject paintBarRoot;
	public Vector2 paintBarBound;
	public bool isBarDraging;
	public Image[] typeButtons;
	public GameObject[] descriptionButtons;

	public GameObject[] bgButtonSelect;
	public GameObject changeBGWindow;
	public GameObject samplePhotoWindow;
	public GameObject largeSamplePhoto;
	public GameObject samplePhotoContent;
    public float samplePhotoContentHeight;
    public GameObject sampleVideoWindow;
	public GameObject shareButtonWindow;
	List<GameObject> samplePhotoList;
	public GameObject floatWindow;
	public Vector2 floatWindowMoveX;
	public Image floatWindowImage;
    public Text floatWindowName;
    public Text floatWindowText;
	public UIFrom3D float3DButton;
	public UIFrom3D[] CNGFloat3DButton;
	public UIFrom3D[] CoachFloat3DButton;
    public UIFrom3D pedalFloat3DButton;
    public UIFrom3D tiguanBack3DButton;
	public UIFrom3D airconditionFloat3DButton;
	public GameObject floatBackButton;

	public IButtonInfo nowSelectedButton;
	public MediaPlayerCtrl videoContent;
	public RawImage videoFrame;
	public Image saveImageFinish;
	public Text saveImageText;

    public Image paintUI;

	public RectTransform floatWindowContent;
	public bool inSpecialButton;
	int m_iOrgWidth = 0;
	int m_iOrgHeight = 0;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
		samplePhotoList = new List<GameObject> ();
		//MessageDispatcher.AddListener ("OnDragFinish",OnBarDragFinish,true);
		
		InitialTextureButton ();
		InitialColorButton ();
		TypeButtonChange (0);
		HideFloatWindow ();
		InitialPartTypeButton ();
		//ChangeDescriptionButtons (false, false, false);
		PaintBarAnimation (false);
	}

	void InitialPartTypeButton()
	{
		//if (!CarStudio.Exists("CNG")) {
		//Debug.Log(AppData.GetCarPartData(Scene1_UI.CarSeleted,"CNG").Name);
		if (Scene1_UI.CarSeleted == "Tiguan") {
			typeButtons [2].gameObject.SetActive (false);
		}
	}

	public void BackToTitle()
	{
		//GetComponent<AudioSource> ().Play ();
		//CarStudio.SaveCustumUserCar(Scene1_UI.CarSeleted + "save");
		CarStudio.CloseStudio ();
		Application.LoadLevel (Application.loadedLevel - 1);
	}

	public void ChangeTexture()
	{
		//GameManager.instance.CameraChange (false);
		if (GameManager.instance.inAnimation == false) {
			ChangeButtonList (3,AppData.GetPaintingTemplateByName(CarStudio.Car.CarBaseModle));
			TypeButtonChange (3);
			if (!isBarOpen) {
				ChangeScrollBar (true);
			}
		}
	}

	public void ChangeOutParts()
	{
		//GameManager.instance.CameraChange (false);
		//CarStudio.SaveCustumUserCar(Scene1_UI.CarSeleted + "save");
		if (GameManager.instance.inAnimation == false) {
			ChangeButtonList (0, AppData.GetCarAllParts (CarStudio.Car.CarBaseModle));
			TypeButtonChange (0);
			ResetPartButtonColor ();
			if (!isBarOpen) {
				ChangeScrollBar (true);
			}
		}
//		CarStudio.CloseStudio ();
//		CarStudio.LoadCustum (Scene1_UI.CarSeleted + "save");
	}

	public void ChangeElecDevice()
	{
		//GameManager.instance.CameraChange (true);
		if (GameManager.instance.inAnimation == false) {
			ChangeButtonCNG (2);
			TypeButtonChange (2);
			//ChangeButtonList (2,AppData.GetCarPartsByName(Scene1_UI.CarSeleted,"CNG"));
			if (!isBarOpen) {
				ChangeScrollBar (true);
			}
		}
	}

	public void ChangeSpecialCar()
	{
		//GameManager.instance.CameraChange (false);
		if (GameManager.instance.inAnimation == false) {
			ChangeButtonList (1, AppData.GetSpecialTemplateCarList (CarStudio.Car.CarBaseModle));
			TypeButtonChange (1);
			if (!isBarOpen) {
				ChangeScrollBar (true);
			}
		}
	}

	public void ChangeOther()
	{
		//GameManager.instance.CameraChange (false);
		//ChangeButtonList (4,false);
		if (!isBarOpen) {
			ChangeScrollBar (true);
		}
	}

	public void InitialTextureButton()
	{
		ChangeButtonList (0,AppData.GetCarAllParts(CarStudio.Car.CarBaseModle));
	}

	public void InitialColorButton()
	{
		var button = AppData.GetCarColorsByName (CarStudio.Car.CarBaseModle);

		for (int i = 0; i < button.Count; i++) {
			GameObject objSub = Instantiate (Resources.Load ("UI/PaintButton") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
			CustomButton btnSub = objSub.GetComponent<CustomButton> ();
			Image btnSubImg = objSub.GetComponent<Image> ();
			objSub.transform.SetParent (paintBarRoot.transform, false);
			Texture2D img;
			img = Resources.Load (button[i].Icon) as Texture2D;
			btnSub.SetThisButton (button[i]);
			btnSub.ChangeImage (Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0)));
		}
	}

	public void ChangeButtonCNG(int id)
	{
		GameManager.instance.nowSelectedCustomType = id;
		for (int m = 0; m < GameManager.instance.allButtonIcon.Count; m++) {
			//GameManager.instance.allButtonIcon.Remove(GameManager.instance.allButtonIcon[m]);
			Destroy (GameManager.instance.allButtonIcon [m]);
		}
		if (GameManager.instance.allButtonIcon.Count > 0) {
			GameManager.instance.allButtonIcon.RemoveRange (0, GameManager.instance.allButtonIcon.Count);
		}

		IButtonInfo button = AppData.GetCngCar(Scene1_UI.CarSeleted);
		GameObject obj = Instantiate (Resources.Load ("UI/PartTag") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
		GameManager.instance.allButtonIcon.Add (obj);
		CustomButton btn = obj.GetComponent<CustomButton> ();
		obj.transform.SetParent (buttonBarContent.transform, false);
		btn.ChangeText (button.Tag);

		GameObject objSub = Instantiate (Resources.Load ("UI/PartButton") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
		GameManager.instance.allButtonIcon.Add (objSub);
		CustomButton btnSub = objSub.GetComponent<CustomButton> ();
		Image btnSubImg = objSub.GetComponent<Image> ();
		objSub.transform.SetParent (buttonBarContent.transform, false);
		Texture2D img;
		if (CarStudio.Exists (button.Name)) {
			img = Resources.Load (button.Icon + "b") as Texture2D;
		} else {
			img = Resources.Load (button.Icon) as Texture2D;
		}
		btnSub.SetThisButton (button);
		btnSub.ChangeImage (Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0)));
		btnSub.ChangeText (button.Name);
		btnSub.ChangeCondition (id);
	}

	public void ChangeButtonList(int id,Dictionary<string,List<IButtonInfo>> parts)
	{
		GameManager.instance.nowSelectedCustomType = id;
		for (int m = 0; m < GameManager.instance.allButtonIcon.Count; m++) {
			//GameManager.instance.allButtonIcon.Remove(GameManager.instance.allButtonIcon[m]);
			Destroy (GameManager.instance.allButtonIcon [m]);
		}
		if (GameManager.instance.allButtonIcon.Count > 0) {
			GameManager.instance.allButtonIcon.RemoveRange (0, GameManager.instance.allButtonIcon.Count);
		}

//		for (int j = 0; j < AppData.GetCarPartsByName(AppData.CarList [GameManager.instance.selectedCarID],menuName[id]).Count; j++) {
//			GameObject obj = Instantiate (Resources.Load ("UI/PartButton") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
//			obj.name = "Button " + j;
//			GameManager.instance.allButtonIcon.Add (obj);
//			CustomButton btn = obj.GetComponent<CustomButton> ();
//			Image btnImg = obj.GetComponent<Image> ();
//			obj.transform.SetParent (buttonBarContent.transform, false);
//			Texture2D img;
//			img = Resources.Load (AppData.GetCarPartsByName(AppData.CarList [GameManager.instance.selectedCarID],menuName[id])[j].Icon) as Texture2D;
//			btn.ChangeImage (Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0)));
//			btn.ChangeText (AppData.GetCarPartsByName(AppData.CarList [GameManager.instance.selectedCarID],menuName[id])[j].Name);
//			btn.SetID (j);
//		}
		//var parts = AppData.GetCarAllParts(CarStudio.Car.CarBaseModle);
		foreach (var part in parts) {
			
			GameObject obj = Instantiate (Resources.Load ("UI/PartTag") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
			GameManager.instance.allButtonIcon.Add (obj);
			CustomButton btn = obj.GetComponent<CustomButton> ();
			obj.transform.SetParent (buttonBarContent.transform, false);
			if (part.Key == "保险处涂装") {
				btn.ChangeText ("保险车涂装");
			} else {
				btn.ChangeText (part.Key);
			}

			foreach (var button in parts[part.Key]) {
				GameObject objSub = Instantiate (Resources.Load ("UI/PartButton") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
				GameManager.instance.allButtonIcon.Add (objSub);
				CustomButton btnSub = objSub.GetComponent<CustomButton> ();
				Image btnSubImg = objSub.GetComponent<Image> ();
				objSub.transform.SetParent (buttonBarContent.transform, false);
				Texture2D img;
				if (id == 0 || id == 2) {
					if (CarStudio.Exists (button.Name)) {
						img = Resources.Load (button.Icon + "b") as Texture2D;
					} else {
						img = Resources.Load (button.Icon) as Texture2D;
					}
				} else {
					img = Resources.Load (button.Icon) as Texture2D;
				}
				btnSub.SetThisButton (button);
				btnSub.ChangeImage (Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0)));
				btnSub.ChangeText (button.Name);
				btnSub.ChangeCondition (id);
				//btn.SetID (j);
			}
		}
		ResetContentSize (buttonBarContent.GetComponent<RectTransform>(),GameManager.instance.allButtonIcon.Count,227.0f);
	}

	public void ResetContentSize(RectTransform content,int count,float eleSize)
	{
		for (int i = 0; i < count; i++) {
			content.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal,i * eleSize - eleSize / 100 * (i*i*2)); 
		}
	}

	public void ResetFloatwindowContentsize(float size)
	{
		Debug.Log ("text size " + floatWindowText.text.Length);
		floatWindowContent.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical,size);
		floatWindowContent.position = new Vector3 (floatWindowContent.position.x, 0, floatWindowContent.position.z);
		if (size == 400) {
			floatWindowText.transform.localPosition = new Vector3 (floatWindowText.transform.localPosition.x, -269.5f, floatWindowText.transform.localRotation.z);
		} else {
			floatWindowText.transform.localPosition = new Vector3 (floatWindowText.transform.localPosition.x, -270, floatWindowText.transform.localRotation.z);
		}
	}

	public void ChangeScrollBar(bool bo)
	{
        Debug.Log("is bar open " + bo);
        GameManager.instance.isInChangeColor = false;
        if (bo) {
			animationScrollBar.transform.DOLocalMoveY (scrollBounds.y, 0.5f).SetEase (Ease.InOutExpo);
			isBarOpen = true;
            if (nowSelectedButton != null)
            {
                Debug.Log("now select button " + nowSelectedButton.Name);
                if (nowSelectedButton.Tag == "外饰改装" || nowSelectedButton.Tag == "内饰改装")
                {
					ChangeDescriptionButtons(nowSelectedButton.TextureDescription != null, nowSelectedButton.MovieDescription != null, false,nowSelectedButton.MovieDescription != null ? nowSelectedButton.MovieDescription[0]:"null");
                }
            }
            
        } else {
			animationScrollBar.transform.DOLocalMoveY (scrollBounds.x, 0.5f).SetEase (Ease.InOutExpo);
			isBarOpen = false;
			ChangeDescriptionButtons (false, false, false,"null");
		}
		buttonBarContent.transform.localPosition = new Vector3 (0, buttonBarContent.transform.localPosition.y, buttonBarContent.transform.localPosition.z);
	}

	public void OnBarDragFinish()
	{
        //Debug.Log((scrollBounds.y - scrollBounds.x)/2);
        //if (animationScrollBar.transform.localPosition.y > scrollBounds.y - (scrollBounds.y - scrollBounds.x)/2) {
        if (isBarOpen) {
			ChangeScrollBar (false);
		} else {
			ChangeScrollBar (true);
		}
		isBarDraging = false;
	}

	public void PaintBarAnimation(bool bo)
	{
		if (bo) {
			isPaintBarOut = true;
			paintBarRoot.transform.DOLocalMoveX (paintBarBound.y, 0.5f).SetEase (Ease.InOutExpo);
		} else {
			isPaintBarOut = false;
			paintBarRoot.transform.DOLocalMoveX (paintBarBound.x, 0.5f).SetEase (Ease.InOutExpo);
			Texture2D tex = Resources.Load("UIImage/anniu/01") as Texture2D;
			GameManager.instance.allButtonIcon[1].GetComponent<Image>().sprite = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0, 0));
		}
	}

	public void BGWinodowShow(bool bo)
	{
		changeBGWindow.SetActive (bo);
		ChangeBGSelectionUI (GameManager.instance.selectedBG);
	}

	public void ChangeBG(int id)
	{
		GameManager.instance.ChangeBGFunc (id);
		ChangeBGSelectionUI (id);
	}

	public void ChangeBGSelectionUI(int id)
	{
		foreach (GameObject item in bgButtonSelect) {
			item.SetActive (false);
		}
		bgButtonSelect [id].SetActive (true);
	}

	public void SamplePhotoWindowShow(bool bo)
	{
		samplePhotoWindow.SetActive (bo);
		LargeSamplePhotoShow (false);
	}

	public void SampleVideoWindowShow(bool bo)
	{
		sampleVideoWindow.SetActive (bo);
		if (!bo) {
			videoContent.Stop ();
			UIManager.instance.videoContent.gameObject.SetActive (false);
		} else {
			UIManager.instance.videoContent.gameObject.SetActive (true);
		}
	}

	public void LargeSamplePhotoShow(bool bo)
	{
		largeSamplePhoto.SetActive (bo);
	}

	public void ShareWindowShow(bool bo)
	{
		shareButtonWindow.SetActive (bo);
	}

	public void SamplePhotoRefresh()
	{
		Debug.Log (samplePhotoList.Count);
		for (int m = 0; m < samplePhotoList.Count; m++) {
			Destroy (samplePhotoList[m]);
		}
		if (samplePhotoList.Count > 0) {
			samplePhotoList.RemoveRange (0, samplePhotoList.Count);
		}
		foreach (var item in nowSelectedButton.TextureDescription) {
			GameObject obj = Instantiate (Resources.Load ("UI/CarSmaplePhotoButton") as GameObject, Vector3.zero, Quaternion.identity) as GameObject;
			samplePhotoList.Add (obj);
			SamplePhotoButton btn = obj.GetComponent<SamplePhotoButton> ();
			obj.transform.SetParent (samplePhotoContent.transform, false);
			Texture2D img;
			Debug.Log (AppData.GetSamples (item).Asset);
			img = Resources.Load (AppData.GetSamples(item).Asset) as Texture2D;
			btn.SetSamplePhoto (Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0)), AppData.GetSamples(item).Name, AppData.GetSamples(item).Description);
		}
        int line = samplePhotoList.Count / 3;
        samplePhotoContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, samplePhotoContentHeight + samplePhotoContentHeight * line);
    }

	public void ChangeDescriptionButtons(bool bo0,bool bo1,bool bo2,string movieName)
	{
		descriptionButtons [0].SetActive(bo0);
		descriptionButtons [1].SetActive(bo1); 
		if (bo1) {
			InitSampleVideo (movieName);
		}
		//descriptionButtons [2].SetActive(bo2);
	}

	public void TypeButtonChange(int id)
	{
		foreach (Image item in typeButtons) {
			item.color = new Color(item.color.r,item.color.g,item.color.b,0);
		}
		typeButtons[id].color =  new Color(typeButtons[id].color.r,typeButtons[id].color.g,typeButtons[id].color.b,1);
		GameManager.instance.CameraGoBack ();
		if (GameManager.instance.inCNG == true) {
			GameManager.instance.inCNG = false;
			ShowCNGFloatButton (false);
			CarStudio.CloseStudio ();
			CarStudio.LoadCustum (Scene1_UI.CarSeleted + "save");
		}
		if (GameManager.instance.inCoach == true) {
			GameManager.instance.inCoach = false;
			ShowCoachFloatButton (false);
            ShowPedalFloat3DButton(false);
			ShowAirconditionFloat3DButton (false);
            CarStudio.CloseStudio ();
			CarStudio.LoadCustum (Scene1_UI.CarSeleted + "save");
		}
	}

	public void ShowFloatWindow(Texture2D img,string nam,string txt,bool bo)
	{
		if (GameManager.instance.inCNG == true) {
			ShowCNGFloatButton (false);
		}
		if (GameManager.instance.inCoach == true) {
			ShowCoachFloatButton (false);
            ShowPedalFloat3DButton(false);
			ShowAirconditionFloat3DButton (false);
        }
        floatWindowName.text = nam;
		floatWindowImage.sprite = Sprite.Create(img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0));
		floatWindowText.text = txt;
		float3DButton.gameObject.SetActive (bo);
		floatBackButton.SetActive (true);
		floatWindow.transform.DOLocalMoveX (floatWindowMoveX.y, 0.5f).SetEase (Ease.OutExpo);
	}

	public void HideFloatWindow()
	{
		if (GameManager.instance.inCNG == true) {
			if (GameObject.Find ("CNG").GetComponent<MeshRenderer>().enabled == true) {
				ShowCNGFloatButton (true);
				ShowCoachFloatButton (false);
                ShowPedalFloat3DButton(false);
				ShowAirconditionFloat3DButton (false);
            }
		}
		if (GameManager.instance.inCoach == true) {
			ShowCoachFloatButton (true);
			ShowCNGFloatButton (false);
            ShowPedalFloat3DButton(false);
			ShowAirconditionFloat3DButton (false);
			GameManager.instance.CameraGoBack ();
			ShowCoachFloatButton (true);
        }
		floatBackButton.SetActive (false);
		if (!GameManager.instance.inCameraPosition) {
			float3DButton.gameObject.SetActive (false);
		}
		floatWindow.transform.DOLocalMoveX (floatWindowMoveX.x, 0.5f).SetEase (Ease.OutExpo);
	}

	public void InitSampleVideo(string url)
	{
		videoContent.m_strFileName = url + ".mp4";
	}

	public void PlaySampleVideo()
	{
		if (GameManager.instance.videoIsPlaying) {
			videoContent.Stop ();
		} else {
			videoContent.Play ();
		}
		GameManager.instance.videoIsPlaying = !GameManager.instance.videoIsPlaying;
	}

	public void VideoSoundChange()
	{
		if (GameManager.instance.videoIsSoundOff) {
			videoContent.SetVolume (1.0f);
		} else {
			videoContent.SetVolume (0);
		}
		GameManager.instance.videoIsSoundOff = !GameManager.instance.videoIsSoundOff;
	}

	public void VideoFullScreen()
	{
		//videoContent.m_bFullScreen = true;
		FullScreenResize ();
	}

	void FullScreenResize()
	{
		m_iOrgWidth = Screen.width;
		m_iOrgHeight = Screen.height;

		float fRatio = (float) m_iOrgHeight / (float)m_iOrgWidth;

		videoFrame.transform.localScale = new Vector3( 20.0f / fRatio, 20.0f / fRatio, 1.0f);

		videoContent.Resize();
	}

	public void ResetButtonColor()
	{
		foreach (GameObject item in GameManager.instance.allButtonIcon) {
			CustomButton btn = item.GetComponent<CustomButton> ();
			if (btn != null) {
				if ( btn.isTag == false ) {
					Texture2D img = Resources.Load (btn.thisButton.Icon) as Texture2D;
					btn.thisImage.sprite = Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0));
				}
			}
		}
	}

	public void ResetPartButtonColor()
	{
		foreach (GameObject item in GameManager.instance.allButtonIcon) {
			CustomButton btn = item.GetComponent<CustomButton> ();
			if (btn != null) {
				if ( btn.isTag == false ) {
					Texture2D img = Resources.Load (btn.thisButton.Icon) as Texture2D;
					btn.thisImage.sprite = Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0));

					if (CarStudio.Exists (btn.thisButton.Name)) {
						img = Resources.Load (btn.thisButton.Icon + "b") as Texture2D;
					} else {
						img = Resources.Load (btn.thisButton.Icon) as Texture2D;
					}
					btn.thisImage.sprite = Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0));
				}
			}
		}
	}

	public void ShowCNGFloatButton(bool bo)
	{
		foreach (UIFrom3D item in UIManager.instance.CNGFloat3DButton) {
			item.gameObject.SetActive (bo);
		}
	}

	public void ShowCoachFloatButton(bool bo)
	{
		foreach (UIFrom3D item in UIManager.instance.CoachFloat3DButton) {
			item.gameObject.SetActive (bo);
		}
	}

    public void ShowPedalFloat3DButton(bool bo)
    {
        pedalFloat3DButton.gameObject.SetActive(bo);
    }

    public void ShowTiguanBackFloat3DButton(bool bo)
    {
        tiguanBack3DButton.gameObject.SetActive(bo);
    }

	public void ShowAirconditionFloat3DButton(bool bo)
	{
		airconditionFloat3DButton.gameObject.SetActive (bo);
	}


    public void ShowSaveImageUI()
	{
		saveImageFinish.DOFade (1.0f, 1.5f).SetEase(Ease.OutExpo).OnComplete(ShowSaveImageUIFinish);
		saveImageText.DOFade (1.0f, 1.5f).SetEase(Ease.OutExpo);
	}

	public void ShowSaveImageUIFinish()
	{
		saveImageFinish.DOFade (0, 1.5f).SetEase(Ease.OutExpo);
		saveImageText.DOFade (0, 1.5f).SetEase(Ease.OutExpo);
	}

    public void HideOtherFloatButton()
    {
        foreach (UIFrom3D item in CNGFloat3DButton)
        {
            item.gameObject.SetActive(false);
        }
        foreach (UIFrom3D item2 in CoachFloat3DButton)
        {
            item2.gameObject.SetActive(false);
        }
        pedalFloat3DButton.gameObject.SetActive(false);
        tiguanBack3DButton.gameObject.SetActive(false);
		airconditionFloat3DButton.gameObject.SetActive (false);
    }

	public void SaveImage()
	{
		ShowSaveImageUI ();
		GameManager.instance.SaveImage ();

	}
}
