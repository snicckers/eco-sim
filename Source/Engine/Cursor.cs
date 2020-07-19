#region Include
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
#endregion

namespace EcoSim.Source.Engine
{
    public class Cursor
    {
        /* Class responsible for managing ONLY the mouse. 
         * It does nothing else and should be used nowhere else. */
        
        private Vector2 _position;
        private Vector2 _dimensions;
        private Color _color;
        private Texture2D _texture;

        public Vector2 Position { get => _position; }

        public Cursor(String Path)
        {
            _texture = Globals._content.Load<Texture2D>(Path);
        }

        public void Update(Vector2 Pos) 
        {
            _position = Pos;
            MouseEffects();
        }

        public void Draw()
        {
            if (_texture != null)
            {
                Rectangle rec = new Rectangle((int)Position.X, (int)Position.Y, (int)_dimensions.X, (int)_dimensions.Y);
                Vector2 center = new Vector2(_texture.Bounds.Width / 2, _texture.Bounds.Height / 2);

                Globals._spriteBatch.Draw(_texture, rec, null, _color, 0.0f, center, new SpriteEffects(), 0);
            }

        }

        private void MouseEffects()
        {
            if (Globals._mouse.LeftClickHold())
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
