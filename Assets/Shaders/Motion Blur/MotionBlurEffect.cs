using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

[AddComponentMenu("Image Effects/MotionBlurEffect")]
[RequireComponent(typeof(Camera), typeof(CameraInfo))]
public class MotionBlurEffect : ImageEffectBase {


	Camera camera;
	//Mostly for testing, you can view different buffers
	public enum Output {
		Full,
		Velocity,
		Original
	}
	
	// Keep a hash of BlurObjects for rendering velocity buffer
	protected static HashSet<ObjectBlur> BlurObjects {
		get {
			if (m_blurObjects == null)
				m_blurObjects = new HashSet<ObjectBlur>();
			return m_blurObjects;
		}
	}
	protected static HashSet<ObjectBlur> m_blurObjects;
	
	public static void RegisterObject(ObjectBlur obj) {
		BlurObjects.Add(obj);
	}
	
	public static void DeregisterObject(ObjectBlur obj) {
		BlurObjects.Remove(obj);
	}
	
	// Allows display of original buffers
	public Output mode = Output.Full;
	
	protected Camera m_velocityCamera;
	
	// Set up velocity camera
	virtual protected void Awake() {
		GameObject velocityCameraObject = new GameObject("Velocity Camera (Auto-generated)", typeof(Camera));
		velocityCameraObject.transform.parent = transform;
		m_velocityCamera = velocityCameraObject.GetComponent<Camera>();
		velocityCameraObject.active = false;
		camera = GetComponent<Camera> ();
	}
	
	// This is where the effect is applied
	virtual protected void OnRenderImage(RenderTexture source, RenderTexture destination) {
		
		// Set materials for blurring objects
		foreach (ObjectBlur objectBlur in BlurObjects) {
			objectBlur.PreMotionRender();
		}
		
		// Get RenderTexture for velocity field (note that it needs a depth buffer)
		RenderTexture velocityTexture = RenderTexture.GetTemporary(source.width, source.height, 24);
		
		// Make sure that the velocity camera is the same as the regular one
		m_velocityCamera.CopyFrom(camera);
		// Clear with zero velocity in XY, encoded in RGBA
		// These are the values for EncodeFloatRG(0.5) from UnityCG.cginc
		m_velocityCamera.backgroundColor = new Color(0.4980392f, 0.5f, 0.4980392f, 0.5f);
		// Render with replacement shaders. Blurred objects are actually "replaced"
		// with their own shaders. Other objects render zero velocity.
		m_velocityCamera.targetTexture = velocityTexture;
		m_velocityCamera.RenderWithShader(MotionVectorMaterialFactory.MotionVectorShader, "RenderType");
		m_velocityCamera.targetTexture = null;
		
		// Reset materials
		foreach (ObjectBlur objectBlur in BlurObjects) {
			objectBlur.PostMotionRender();
		}
		
		switch (mode) {
			case Output.Full: // Apply the line interval convolution
				material.SetTexture("_Velocity", velocityTexture);
				
				Graphics.Blit(source, destination, material);
				break;
			case Output.Velocity: // Just show velocity buffer
				Graphics.Blit(velocityTexture, destination);
				break;
			default: // Just show original image
				Graphics.Blit(source, destination);
				break;
		}
		
		// Let Unity clean up the texture when it needs to
		RenderTexture.ReleaseTemporary(velocityTexture);
	}
}