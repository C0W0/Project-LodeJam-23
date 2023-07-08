using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance;

	[SerializeField]
	private float upLimit, downLimit, leftLimit, rightLimit;

	private Camera _cam;
	private Vector3 _touchPos;
	private Transform _objToFollow;

	private void Awake()
	{
		Instance = this;
		_cam = GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update()
	{
		if (_objToFollow != null)
		{
			Vector3 pos = _objToFollow.position;
			Vector3 originalPos = _cam.transform.position;
			_cam.transform.position = new Vector3(
				Mathf.Clamp(pos.x, leftLimit, rightLimit),
				Mathf.Clamp(pos.y, downLimit, upLimit),
				originalPos.z
			);
		}
	}

	public void FollowObject(Transform obj)
	{
		_objToFollow = obj;
		Focus(obj.position);
	}

	public void UnfollowObject()
	{
		_objToFollow = null;
	}

	public void Focus(Vector3 position)
	{
		print(position);
		Vector3 newPos = new Vector3(
			Mathf.Clamp(position.x, leftLimit, rightLimit),
			Mathf.Clamp(position.y, downLimit, upLimit),
			transform.position.z);

		LeanTween.move(gameObject, newPos, 0.2f);
	}
}
