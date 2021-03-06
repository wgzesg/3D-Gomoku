﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	//
	// VARIABLES
	//

	public float turnSpeed = 4.0f;      // Speed of camera turning when mouse moves in along an axis
	public float panSpeed = 4.0f;       // Speed of the camera when being panned
	public float zoomSpeed = 4.0f;

	private Vector3 mouseOrigin;    // Position of cursor when mouse dragging starts
	private bool isPanning;     // Is the camera being panned?
	private bool isRotating;    // Is the camera being rotated?


	private Vector3 startingPos;
	private Quaternion startingRot;

    //
    // UPDATE
    //

    private void Start()
    {
		startingPos = transform.position;
		startingRot = transform.localRotation;

	}

    void Update()
	{

		// Get the middle mouse button
		if (Input.GetMouseButtonDown(2))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isPanning = true;
		}

		// Get the right mouse button
		if (Input.GetMouseButtonDown(1))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isRotating = true;
		}

		// Disable movements on button release
		if (!Input.GetMouseButton(1)) isRotating = false;
		if (!Input.GetMouseButton(2)) isPanning = false;

		// Rotate camera along X and Y axis
		if (isRotating)
		{
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

			transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
			transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
		}

		// Move the camera on it's XY plane
		if (isPanning)
		{
			Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

			Vector3 move = new Vector3(-pos.x * panSpeed, -pos.y * panSpeed, 0);
			transform.Translate(move, Space.Self);
		}
		
		float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel");
		Vector3 position = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
		Vector3 Updatemove = position.y * ScrollWheelChange * zoomSpeed * transform.forward;
		transform.Translate(Updatemove, Space.World);
	}


	public void OnResetCam()
    {
		transform.position = startingPos;
		transform.localRotation = startingRot;
	}
}