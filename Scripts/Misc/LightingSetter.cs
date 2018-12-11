using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]

public class LightingSetter : MonoBehaviour 
{
	//addFog
	public static LightingSetter GC;
	Object currentObject;

	public Camera mainCamera;
	public Color cameraBackround;
	public List<Material> sky;
	public int currentSkybox;

	public bool skybox;
	public bool color;
	public bool gradient;
	public bool rotating;
	public float rotateSpeed;
	float rotateVal;

	//skyColor is also ambientColor
	 
	public Color skyBoxColor;
	public Color skyColor;
	public Color equatorColor;
	public Color groundColor;

	public float ambientIntensity;
	public float reflectionIntensity;
	public int reflectionBounces;

	void Start()
	{
		if(mainCamera == null)
		{
			mainCamera = Camera.main;
		}
		GC = this.GetComponent<LightingSetter>();
	}

	public void UpdateSettings () 
	{

		Material nullSky = null;
		if(skybox)
		{
			RenderSettings.ambientMode = AmbientMode.Skybox;
			RenderSettings.skybox = sky[currentSkybox];
			RenderSettings.skybox.SetColor("_Tint", skyBoxColor);
			if(rotating)
			{
				rotateVal += rotateSpeed * Time.deltaTime;
				RenderSettings.skybox.SetFloat("_Rotation", rotateVal);
			}
			if(rotateVal >= 360)
			{
				rotateVal = 0;
			}
		}
		else
		{
			RenderSettings.skybox = nullSky;
		}
		if(color)
		{
			RenderSettings.ambientMode = AmbientMode.Flat;
		}
		if(gradient)
		{
			RenderSettings.ambientMode = AmbientMode.Trilight;
		}

		RenderSettings.ambientSkyColor = skyColor;
		RenderSettings.ambientEquatorColor = equatorColor;
		RenderSettings.ambientGroundColor = groundColor;
		
		RenderSettings.ambientIntensity = ambientIntensity;
		RenderSettings.reflectionIntensity = reflectionIntensity;
		RenderSettings.reflectionBounces = reflectionBounces;
		
		mainCamera.backgroundColor = cameraBackround;

		if(currentSkybox >= sky.Count)
		{
			currentSkybox =  sky.Count;
		}
	}
		
}
