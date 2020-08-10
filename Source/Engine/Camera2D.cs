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
        private const float _zoomUpperLimit = 3.5f;
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
                float leftBarrier = (float)_viewportWidth *
                       .5f / _zoom;
                float rightBarrier = _worldWidth -
                       (float)_viewportWidth * .5f / _zoom;
                float topBarrier = _worldHeight -
                       (float)_viewportHeight * .5f / _zoom;
                float bottomBarrier = (float)_viewportHeight *
                       .5f / _zoom;
                _pos = value;
                //if (_pos.X < leftBarrier)
                //    _pos.X = leftBarrier;
                //if (_pos.X > rightBarrier)
                //    _pos.X = rightBarrier;
                //if (_pos.Y > topBarrier)
                //    _pos.Y = topBarrier;
                //if (_pos.Y < bottomBarrier)
                //    _pos.Y = bottomBarrier;
            }
        }

        #endregion

        public Matrix GetTransformation()
        {
            _pos = Globals._cursor.Position + (_zoom / _newZoom) * (_pos - Globals._cursor.Position);
            _zoom = _newZoom;

            _transform =
               Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
               Matrix.CreateRotationZ(Rotation) *
               Matrix.CreateScale(new Vector3(_zoom, _zoom, 1));

            return _transform;
        }
    }
}
