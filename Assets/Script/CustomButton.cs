﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public enum CustomType
{
	OutsidePart,
	TextureColor,
	SpecialCar,
	CNG,
	PaintingCar
}

public class CustomButton : MonoBehaviour {

	public CustomType customType;
	public int thisID;
	public Text thisText;
	public Image thisImage;
	public bool isTag;
	public bool isPreview;
	public bool isVideo;
	public bool isPaint;
	public IButtonInfo thisButton;
	IButtonInfo descriptionButton;

	// Use this for initialization
	void Start () {
		//descriptionButton = AppData.GetCarPartsByName (GameManager.instance.carType.ToString());
	}

	public void SetID(int id)
	{
		thisID = id;
	}

	public void ChangeImage(Sprite img)
	{
		thisImage.sprite = img;
	}

	public void ChangeText(string txt)
	{
		thisText.text = txt;
	}

	public void ChangeCondition(int cond)
	{
		switch (cond) {
		case 0:
			customType = CustomType.OutsidePart;
			break;
		case 1:
			customType = CustomType.SpecialCar;
			break;
		case 2:
			customType = CustomType.CNG;
			break;
		case 3:
			customType = CustomType.PaintingCar;
			break;
		default:
			break;
		}

		if (thisButton.Name == "变色模块") {
			customType = CustomType.TextureColor;
		}
	}

	public void SetThisButton(IButtonInfo button)
	{
		thisButton = button;
	}

	public void ClickThisButton()
	{
		if (!isTag && GameManager.instance.inAnimation == false) {
			GameManager.instance.nowCustomButton = this;
			GameManager.instance.isPartButtonClick = true;
			GameManager.instance.inCNG = false;
			GameManager.instance.inCoach = false;
			GameManager.instance.backParticle.SetActive (false);
			descriptionButton = thisButton;
			Texture2D img = new Texture2D(10,10);
			if (customType == CustomType.TextureColor && !UIManager.instance.isPaintBarOut) {
				UIManager.instance.PaintBarAnimation (true);
				img = Resources.Load (descriptionButton.Icon + "b") as Texture2D;
				thisImage.sprite = Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0));
				GameManager.instance.CameraGoBack ();

				//GameManager.instance.ChangeCustomTexture (thisID);
			}
			else{
				
				if (UIManager.instance.isPaintBarOut && !isPaint) {
					UIManager.instance.PaintBarAnimation (false);
				}

				if (!isPaint) {
					UIManager.instance.ChangeDescriptionButtons(descriptionButton.TextureDescription!=null,descriptionButton.MovieDescription!=null,string.IsNullOrEmpty(descriptionButton.PdfDescription));
					UIManager.instance.nowSelectedButton = thisButton;
					//ChangeButtonColor (img);
				} 


				if (customType == CustomType.OutsidePart) {
					ChangeButtonColor (img);
					if (UIManager.instance.inSpecialButton == true) {
						CarStudio.CloseStudio ();
						CarStudio.LoadCustum (Scene1_UI.CarSeleted + "save");
					}
					UIManager.instance.inSpecialButton = false;

					if (thisButton.Name != "电动踏板") {
						GameManager.instance.CloseDoor ();
					}
					if (descriptionButton.Type == null){// || GameManager.instance.inCameraPosition) {
						ChangePart ();
					} else {
						CameraGoto (int.Parse(descriptionButton.Type));
					}


				}
				else if (customType == CustomType.SpecialCar) {
					
					if (descriptionButton.Description == "sampleASB") {
						GameManager.instance.inCoach = false;
					}
					UIManager.instance.ResetButtonColor ();
					ChangeButtonColor (img);
					CarStudio.LoadTemplate (thisButton.Description);
					GameManager.instance.CameraGoBack ();
					UIManager.instance.inSpecialButton = true;
					if (descriptionButton.Name == "驾校车改装1") {
						GameManager.instance.inCoach = true;
						UIManager.instance.ShowCoachFloatButton (true);
					}
					return;
				}
				else if (customType == CustomType.CNG) {
					GameManager.instance.inCNG = true;
					ChangeButtonColor (img);
					if (UIManager.instance.inSpecialButton == true) {
						CarStudio.CloseStudio ();
						CarStudio.LoadCustum (Scene1_UI.CarSeleted + "save");
					}
					UIManager.instance.inSpecialButton = false;
//					CarData carData = AppData.GetCarAllParts().;
//					CarStudio.LoadTemplate (carData.CNG);
					CameraGoto (12);

					return;
				}
				else if (customType == CustomType.PaintingCar) {
					UIManager.instance.ResetButtonColor ();
					ChangeButtonColor (img);
					CarStudio.LoadTemplate (thisButton.Description);
					GameManager.instance.CameraGoBack ();
					UIManager.instance.inSpecialButton = true;

					return;
				}
			}
		}
	}



	void ChangeButtonColor(Texture2D img)
	{
		if (!isPaint) {
			if (CarStudio.Exists (descriptionButton.Name)) {
				img = Resources.Load (descriptionButton.Icon) as Texture2D;
			} else {
				img = Resources.Load (descriptionButton.Icon + "b") as Texture2D;
			}
			//CameraGoBack ();
			thisImage.sprite = Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0));

		}
	}

	void ChangePart()
	{
		if (customType == CustomType.CNG) {
			Texture2D img = new Texture2D(10,10);
			MeshRenderer cngRenderer = GameObject.Find ("CNG").GetComponent<MeshRenderer> ();
			GameObject[] parts = GameObject.FindGameObjectsWithTag("AnimPart");
			foreach (GameObject obj in parts) {
				if (cngRenderer.enabled == false) {
					obj.GetComponent<PartAnimation> ().SettingAnimation ("后盖开启_play");
					cngRenderer.transform.localPosition = new Vector3(cngRenderer.transform.localPosition.x,5.0f,cngRenderer.transform.localPosition.z);
					GameManager.instance.SetCNGRenderer (true);
					cngRenderer.transform.DOLocalMoveY (1.355f, 3.0f).SetEase (Ease.OutQuad);
				} else {
					obj.GetComponent<PartAnimation> ().SettingAnimation ("后盖开启_playback");
					GameManager.instance.SetCNGRenderer (false);
					cngRenderer.transform.DOLocalMoveY (5.0f, 3.0f).SetEase (Ease.OutQuad).OnComplete(ResetCNG);
				}
			}
			if (cngRenderer.enabled == false) {
				img = Resources.Load (descriptionButton.Icon) as Texture2D;
				UIManager.instance.ShowCNGFloatButton (false);
			} else {
				img = Resources.Load (descriptionButton.Icon + "b") as Texture2D;
				UIManager.instance.ShowCNGFloatButton (true);
			}
			thisImage.sprite = Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0, 0));
			//CarStudio.LoadTemplate(AppData.GetCarDataByName(Scene1_UI.CarSeleted).CNG);
		} else {
			
			if (CarStudio.Exists(thisButton.Name)) {
				StartSettingAnimation ("_playback");
				if (thisButton.Name != "电动踏板" && thisButton.Name != "后盖开启") {
					UIManager.instance.HideFloatWindow ();
					UIManager.instance.float3DButton.gameObject.SetActive (false);
					CarStudio.RemovePart (thisButton.Name);
				}

				if (thisButton.Name == "导航仪") {
					GameManager.instance.ShowIphone (false, GameManager.instance.selectedCarID);
				} else {
					GameManager.instance.iphoneHideComplete ();
				}
//				else if (thisButton.Name == "电动踏板" && !GameManager.instance.epDown) {
//					CarStudio.RemovePart (thisButton.Name);
//				}
			} else {
				CarStudio.AddPart (thisButton.Name);
				StartSettingAnimation ("_play");
				ShowFloatButton ();
				if (thisButton.Name == "导航仪") {
					GameManager.instance.ShowIphone (true, GameManager.instance.selectedCarID);
				}
				else
				{
					GameManager.instance.iphoneHideComplete ();
				}
			}
			if (thisButton.Name == "后盖开启" && Scene1_UI.CarSeleted == "Passat") {
				GameManager.instance.backParticle.SetActive (true);
			}
		}
		CarStudio.SaveCustumUserCar(Scene1_UI.CarSeleted + "save");
	}

	void ResetCNG()
	{
		GameObject obj = GameObject.Find ("CNG");
		obj.transform.localPosition = new Vector3(obj.transform.localPosition.x,1.355f,obj.transform.localPosition.z);
	}

	void StartSettingAnimation(string str)
	{
		GameObject[] parts = GameObject.FindGameObjectsWithTag("AnimPart");
		foreach (GameObject obj in parts) {
			obj.GetComponent<PartAnimation> ().SettingAnimation (thisButton.Name + str);
		}
	}

	void CameraGoto(int id)
	{
		GameManager.instance.inCameraPosition = true;
		GameManager.instance.nowCamPositionID = id;
		CarControl.instance.camNowPosX = 0;
		CarControl.instance.camNowRotateY = Camera.main.transform.rotation.eulerAngles.y;
		Camera.main.transform.DOMove (GameManager.instance.allCamPosition [id].position, GameManager.instance.cameraMoveTime).SetEase(GameManager.instance.cameraMoveEase).OnComplete(CameraGotoFinish);
		Camera.main.transform.DORotate (GameManager.instance.allCamPosition [id].rotation.eulerAngles, GameManager.instance.cameraMoveTime).SetEase (GameManager.instance.cameraMoveEase);
	}

	void CameraGotoFinish()
	{
		ChangePart ();
		if (GameManager.instance.inCameraPosition) {
			GameManager.instance.cameraRotationY = Camera.main.transform.rotation.eulerAngles.y;
			CarControl.instance.camNowRotateY = Camera.main.transform.rotation.eulerAngles.y;
		}
	}

	void ShowFloatButton()
	{
		if (thisButton.Description != null) {
			UIManager.instance.float3DButton.gameObject.SetActive (true);
			UIManager.instance.float3DButton.SetThisTarget (GameObject.Find(AppData.GetSamples (thisButton.Description).Asset),thisButton);
			//UIManager.instance.float3DButton.SetThisTarget (CarStudio.objects[AppData.GetCarPartData(Scene1_UI.CarSeleted,thisButton.Name)]. thisButton.,thisButton);
		}
	}
}
