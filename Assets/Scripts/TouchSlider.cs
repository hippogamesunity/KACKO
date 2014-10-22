﻿using System;
using UnityEngine;

public class TouchSlider : MonoBehaviour
{
    public MonoBehaviour Listener;
    public string ListenerMethodLeft;
    public string ListenerMethodRight;

    private bool _pressed;
    private Vector2 _position;

	public void Update()
    {
        if (Input.GetMouseButtonDown(0) && collider.bounds.Contains(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            _position = Input.mousePosition;
            _pressed = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _pressed = false;
        }

        if (_pressed)
        {
            var delta = _position.x - Input.mousePosition.x;

            if (Math.Abs(delta) > 200 * Camera.main.aspect)
            {
                Listener.SendMessage(delta > 0 ? ListenerMethodLeft : ListenerMethodRight);
                _pressed = false;
            }
        }
	}
}