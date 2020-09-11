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
#endregion


namespace EcoSim
{
    class NicksMouse
    {
        /*------------------- FIELDS -----------------------------------------------*/
        private MouseState _newMouse, _oldMouse, _firstMouse;
        private Vector2 _newMousePos, _oldMousePos, _firstMousePos, _screenLocation, _worldLocation;
        private bool _dragging, _rightDrag, _scrollUp, _scrollDown;
        private int _previousScrollValue;

        private Vector2 _dimensions;
        private Color _color;
        private Texture2D _texture;

        public MouseState OldMouse { get => _oldMouse; }
        public Vector2 WorldLocation { get => _worldLocation; set => _worldLocation = value; }
        public Vector2 ScreenLocation { get => _oldMousePos; set => _oldMousePos = value; }
        public int PreviousScrollValue { get => _previousScrollValue; }
        public bool ScrollUp { get => _scrollUp; }
        public bool ScrollDown { get => _scrollDown; }

        public NicksMouse(string path)
        {
            _texture = Globals._content.Load<Texture2D>(path);

            _newMouse = Mouse.GetState();
            _oldMouse = _newMouse;
            _firstMouse = _newMouse;

            _newMousePos = new Vector2(_newMouse.Position.X, _newMouse.Position.Y);
            _oldMousePos = new Vector2(_newMouse.Position.X, _newMouse.Position.Y);
            _firstMousePos = new Vector2(_newMouse.Position.X, _newMouse.Position.Y);

            _previousScrollValue = _oldMouse.ScrollWheelValue;

            GetMouseAndAdjust();
        }

        public void Update()
        {
            GetMouseAndAdjust();

            _oldMouse = _newMouse;
            _oldMousePos = GetScreenPos(_newMouse);

            ScrollChange();
            GetWorldPos();
            MouseEffects();
        }

        public void Draw()
        {
            //MUST use world location to draw
            if (_texture != null)
            {
                Rectangle rec = new Rectangle((int)_worldLocation.X, (int)_worldLocation.Y, (int)_dimensions.X, (int)_dimensions.Y);
                Vector2 center = new Vector2(_texture.Bounds.Width / 2, _texture.Bounds.Height / 2);

                Globals._spriteBatch.Draw(_texture, rec, null, _color, 0.0f, center, new SpriteEffects(), 0);
            }
        }

        public virtual void GetMouseAndAdjust()
        {
            _newMouse = Mouse.GetState();
            _newMousePos = GetScreenPos(_newMouse);
        }

        private Vector2 GetScreenPos(MouseState mouse)
        {
            return (new Vector2(mouse.Position.X, mouse.Position.Y));
        }

        private void GetWorldPos()
        {
            // Transform mouse input from view to world position
            Matrix inverse = Matrix.Invert(Globals._camera.GetTransformation());
            Vector2 worldPos = Vector2.Transform(new Vector2(_oldMousePos.X, _oldMousePos.Y), inverse);
            _worldLocation = worldPos;
        }

        public void SetScreenPos()
        {

        }

        public int MouseWheelChange()
        {
            return (_newMouse.ScrollWheelValue - _oldMouse.ScrollWheelValue);
        }

        public void ScrollChange()
        {
            if(_newMouse.ScrollWheelValue < _previousScrollValue)
            {
                _scrollDown = true;
                _scrollUp = false;
            }
            else if(_newMouse.ScrollWheelValue > _previousScrollValue)
            {
                _scrollDown = false;
                _scrollUp = true;
            }
            else
            {
                _scrollDown = false;
                _scrollUp = false;
            }

            _previousScrollValue = _newMouse.ScrollWheelValue;
        }

        // Is the left mouse button pressed within the screen (one-shot)
        public virtual bool LeftClick()
        {
            if    (_newMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed 
                && _oldMouse.LeftButton != Microsoft.Xna.Framework.Input.ButtonState.Pressed
                && _newMouse.Position.X >= 0 && _newMouse.Position.X <= Globals.ScreenWidth 
                && _newMouse.Position.Y >= 0 && _newMouse.Position.Y <= Globals.ScreenHeight)
            { return true; }

            return false;
        }

        // Probably add a timer to this at some point!!!
        public bool LeftClickDown()
        {
            if (_newMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed
                && _newMouse.Position.X >= 0 && _newMouse.Position.X <= Globals.MapWidth
                && _newMouse.Position.Y >= 0 && _newMouse.Position.Y <= Globals.MapHeight)
            { return true; }

            return false;
        }

        // Is the left mouse button pressed for some period of time? (one-shot)
        public virtual bool LeftClickHold()
        {
            bool holding = false;
            
            if (LeftClick())
            {
                holding = true;

                if (Math.Abs(_newMouse.Position.X - _firstMouse.Position.X) > 8 || Math.Abs(_newMouse.Position.Y - _firstMouse.Position.Y) > 8)
                { _dragging = true; }
            }

            return holding;
        }

        private void MouseEffects()
        {
            if (LeftClickDown())
            {
                _dimensions = new Vector2(16, 16);
                _color = Color.Aqua;
            }
            else
            {
                _dimensions = new Vector2(24, 24);
                _color = Color.White;
            }
        }

    }
}
