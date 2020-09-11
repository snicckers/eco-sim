#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using EcoSim.Source.Simulation;
using EcoSim.Source;
using EcoSim.Source.Engine;
using EcoSim;
#endregion

// Source: https://adambruenderman.wordpress.com/2011/04/05/create-a-2d-camera-in-xna-gs-4-0/
namespace EcoSim.Source.Engine
{
    public class Camera2D
    {
        /*------------------- FIELDS -----------------------------------------------*/
        private const float _zoomUpperLimit = 30.0f;
        private const float _zoomLowerLimit = .25f;

        private float _newZoom;
        private float _zoom;
        private Matrix _transform;
        private Vector2 _pos; // center of camera
        private float _rotation;
        private int _viewportWidth;
        private int _viewportHeight;
        private int _worldWidth;
        private int _worldHeight;

        /*------------------- Constructors -----------------------------------------*/
        public Camera2D(Viewport viewport, int worldWidth, int worldHeight, float initialZoom)
        {
            _zoom = initialZoom;
            _newZoom = initialZoom;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
            _viewportWidth = viewport.Width;
            _viewportHeight = viewport.Height;
            _worldWidth = worldWidth;
            _worldHeight = worldHeight;
        }

        /*------------------- Methods ----------------------------------------------*/
        #region Properties

        public float Zoom
        {
            get { return _newZoom; }
            set {
                _newZoom = value;
                if (_newZoom < _zoomLowerLimit)
                    _newZoom = _zoomLowerLimit;
                if (_newZoom > _zoomUpperLimit)
                    _newZoom = _zoomUpperLimit; 
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public void Move(Vector2 amount)
        {
            _pos += amount;
        }

        public Vector2 Pos
        {
            get { return _pos; }
            set
            {
                float leftBarrier = (float)_viewportWidth * 0.5f / _zoom;
                float rightBarrier = _worldWidth - (float)_viewportWidth * .5f / _zoom;
                float topBarrier = _worldHeight - (float)_viewportHeight * .5f / _zoom;
                float bottomBarrier = (float)_viewportHeight * 0.5f / _zoom;
                _pos = value;
            }
        }

        public void MouseMove()
        {
            int bounds = 25;
            int scrollSpeed = 25;

            if (Globals.Mouse.ScreenLocation.X <= bounds) // Left
                this.Move(new Vector2(-scrollSpeed, 0));

            if (Globals.Mouse.ScreenLocation.X >= Globals.ScreenWidth - bounds) // Right
                this.Move(new Vector2(scrollSpeed, 0));

            if (Globals.Mouse.ScreenLocation.Y <= bounds) // Up
                this.Move(new Vector2(0, -scrollSpeed));

            if (Globals.Mouse.ScreenLocation.Y >= Globals.ScreenHeight - bounds) // Down
                this.Move(new Vector2(0, scrollSpeed));
        }

        #endregion

        public Matrix GetTransformation()
        {
            // Zoom "towards" mouse. Set the center of the camera to be slightly closer to the mouse each scan when zooming
            _pos = Globals.Mouse.WorldLocation + (_zoom / _newZoom) * (_pos - Globals.Mouse.WorldLocation);
            _zoom = _newZoom;

            _transform =
               Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
               Matrix.CreateRotationZ(Rotation) *
               Matrix.CreateScale(new Vector3(_zoom, _zoom, 1));

            return _transform;
        }
    }
}
