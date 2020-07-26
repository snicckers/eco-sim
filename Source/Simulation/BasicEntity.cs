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
#endregion

namespace EcoSim.Source.Simulation
{
    class BasicEntity
    {
        /*--------- Fields ------------------------------------------*/
        private Vector2 _position, _dimensions;
        private Color _color;
        private Texture2D _texture;
        private BasicEntity _nearestTarget; 
        private bool _drawingLine;
        private float _sightRange;
        private float _scanTimer;
        private static float _scanTime = 0.1f;
        private float _velocity;

        /*---------- Accessors --------------------------------------*/
        public Vector2 Position { get => _position; set => _position = value; }
        public float SightRange { get => _sightRange; }
        public bool DrawingLine { get => _drawingLine; }

        /* Constructors */
        public BasicEntity(Vector2 Position, string Path)
        {
            this.Position = Position;
            _texture = Globals._content.Load<Texture2D>(Path);
            _color = Globals._colorC;
            _dimensions = new Vector2(12, 12);
            _sightRange = 20.0f;
            _velocity = 3.0f;
        }

        /* Update */
        public virtual void Update(List<BasicEntity> EntityList, GameTime gameTime)
        {
            Flee(EntityList, gameTime);
        }   

        /* Draw */
        public void Draw()
        {
            Rectangle rec = new Rectangle((int)Position.X, (int)Position.Y, (int)_dimensions.X, (int)_dimensions.Y);
            Vector2 center = new Vector2(_texture.Bounds.Width / 2, _texture.Bounds.Height / 2);

            Globals._spriteBatch.Draw(_texture, rec, null, _color, 0.0f, center, new SpriteEffects(), 0);
        }

        // Draw the object, and a line to the nearest target
        public void Draw(List<BasicEntity> EntityList)
        {
            // Draw Line to nearest entity if it is not null:
            if (_nearestTarget != null)
            {
                // Make sure that only one line is being drawn (don't let each entity draw a line):
                if (!_nearestTarget._drawingLine)
                {
                    _drawingLine = true;

                    // Draw a line to the nearest target if it is not null:
                    Vector2 Point_A = this.Position;
                    Vector2 Point_B = this._nearestTarget.Position;
                    Globals.DrawLine(Globals._spriteBatch, Point_A, Point_B, Globals._colorE, _texture);

                    // Change entity color if running away
                    _color = Globals._colorD;
                }
                else
                {
                    _drawingLine = false;
                }

            }
            else
            {
                _drawingLine = false;
                _color = Globals._colorC;
            }

            // Draw Self:
            Rectangle rec = new Rectangle((int)Position.X, (int)Position.Y, (int)_dimensions.X, (int)_dimensions.Y);
            Vector2 center = new Vector2(_texture.Bounds.Width / 2, _texture.Bounds.Height / 2);
            Globals._spriteBatch.Draw(_texture, rec, null, _color, 0.0f, center, new SpriteEffects(), 0);
        }

        /* Methods */
        
        // Run away from the nearest target
        // Split this into two different methods? 
        private void Flee(List<BasicEntity> EntityList, GameTime gameTime)
        {

            BasicEntity Target = _nearestTarget; // This prevents unwanted stuttering / update delays
            
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; // Time control.
            _scanTimer += deltaTime;
            
            if (_scanTimer > _scanTime) // Limit the number of times that entities search for the nearest object
            {
                Target = Globals.GetNearest(EntityList, this, this.SightRange);
                _nearestTarget = Target;
                _scanTimer = 0.0f;
            }

            if (Target != null) // Remove this and put it in its own "" method
            {
                Vector2 Direction = Globals.GetUnitVector(_position, _nearestTarget.Position);
                _position -= Direction * _velocity;
            }
        }

        



    }
}
