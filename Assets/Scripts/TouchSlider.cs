﻿using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class TouchSlider : Script
    {
        public MonoBehaviour Listener;
        public string ListenerMethodLeft;
        public string ListenerMethodRight;

        private bool _pressed;
        private Vector2 _position;

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
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
}