using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform target;
	public float smoothing = 5f;
	public bool Shaking;
	private float ShakeDecay;
	private float ShakeIntensity;
	private Vector3 offset;
	private Vector3 OriginalPos;
	private Quaternion OriginalRot;

	void Start()
	{
		offset = transform.position - target.position;
		Shaking = false;
	}

	void FixedUpdate()
	{
		if(!Shaking)
        {
			Vector3 targetCamPos = target.position + offset;
			transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
		}
		else if (ShakeIntensity > 0)
		{
			transform.position = OriginalPos + Random.insideUnitSphere * ShakeIntensity;
			transform.rotation = new Quaternion(OriginalRot.x + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
									  OriginalRot.y + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
									  OriginalRot.z + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
									  OriginalRot.w + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f);

			ShakeIntensity -= ShakeDecay;
		}
		else if (Shaking)
		{
			Shaking = false;
		}
	}

	public void DoShake()
	{
		OriginalPos = transform.position;
		OriginalRot = transform.rotation;

		ShakeIntensity = 0.01f;
		ShakeDecay = 0.001f;
		Shaking = true;
	}
}
