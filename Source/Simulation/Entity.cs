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
    class Entity
    {
        /*------------------- Fields -----------------------------------------------*/
        private Vector2 _position, _dimensions, _direction;
        private Color _color;
        private Texture2D _texture;
        private bool _drawingLine;
        private float _sightRange;
        private bool _delete;

        // Behaviour:
        private bool _fleeing;
        private float _velocity;
        private Entity _nearestTarget;

        // Timers:
        private float _scanTimer;
        private static float _scanTime = 0.1f;
        private float _fleeTimer;
        private static float _fleeTime = 2.0f;

        /*------------------- Accessors --------------------------------------------*/
        public Vector2 Position { get => _position; set => _position = value; }
        public float SightRange { get => _sightRange; }
        public bool DrawingLine { get => _drawingLine; }
        public bool Delete { get => _delete; }

        /*------------------- Constructors -----------------------------------------*/
        public Entity(Vector2 Position, string Path)
        {
            this.Position = Position;
            _texture = Globals._content.Load<Texture2D>(Path);
            _color = Globals._colorG_B;
            _dimensions = new Vector2(12, 12);
            _direction = new Vector2(0, 0);
            _sightRange = 40.0f;
            _velocity = 3.0f;

            //Behaviour:
            _fleeing = false;
        }

        /*------------------- Update -----------------------------------------------*/
        public virtual void Update(List<Entity> EntityList, GameTime gameTime)
        {
            Behaviour(EntityList, gameTime);
            CheckForRemoval();
        }

        /*------------------- Draw -------------------------------------------------*/
        public void Draw()
        {
            Rectangle rec = new Rectangle((int)Position.X, (int)Position.Y, (int)_dimensions.X, (int)_dimensions.Y);
            Vector2 center = new Vector2(_texture.Bounds.Width / 2, _texture.Bounds.Height / 2);

            Globals._spriteBatch.Draw(_texture, rec, null, _color, 0.0f, center, new SpriteEffects(), 0);
        }

        // Draw the object, and a line to the nearest target
        public void Draw(List<Entity> EntityList)
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
                    Globals.DrawLine(Globals._spriteBatch, Point_A, Point_B, Globals._colorE, _texture, 1);

                    // Change entity color if running away
                    _color = Globals._colorG_A;
                }
                else
                {
                    _drawingLine = false;
                }

            }
            else
            {
                _drawingLine = false;
                _color = Globals._colorG_C;
            }

            // Draw Self:
            Rectangle rec = new Rectangle((int)Position.X, (int)Position.Y, (int)_dimensions.X, (int)_dimensions.Y);
            Vector2 center = new Vector2(_texture.Bounds.Width / 2, _texture.Bounds.Height / 2);
            Globals._spriteBatch.Draw(_texture, rec, null, _color, 0.0f, center, new SpriteEffects(), 0);
        }

        /*------------------- Methods ----------------------------------------------*/

        // Run away from the nearest target
        private void Behaviour(List<Entity> EntityList, GameTime gameTime)
        {
            Entity Target = _nearestTarget; // This prevents unwanted stuttering / update delays

            // Scan for enemies:
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; // Time control.
            _scanTimer += deltaTime;

            if (_scanTimer > _scanTime) // Limit the number of times that entities search for the nearest object
            {
                Target = Globals.GetNearest(EntityList, this, this.SightRange);
                _nearestTarget = Target;
                _scanTimer = 0.0f;
            }

            // Sets fleeing timer
            if (_nearestTarget != null)
            {
                _fleeing = true;
            }

            // Run away / fleeing timer
            if (_fleeing)
            {
                float deltaFleeTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                _fleeTimer += deltaFleeTime;

                if (_fleeTimer > _fleeTime)
                {
                    _direction = new Vector2(0, 0);
                    _fleeTimer = 0.0f;
                    _fleeing = false;
                }
            }

            CollisionAndBounds();
            Move();
        }

        // Reverse entity direction if it's tyring to move outside of map bounds
        private void CollisionAndBounds()
        {
            int bounds = 20;
            int halfBounds = bounds / 2;

            // Horizontal wall collision:
            if ((_position.X + 0) <= bounds)
                _direction.X *= -1.0f;
            if ((Globals.MapWidth - _position.X) <= bounds)
                _direction.X *= -1.0f;

            // Vertical Wall Collision:
            if (_position.Y <= bounds)
                _direction.Y *= -1.0f;
            if ((Globals.MapHeight - _position.Y) <= bounds)
                _direction.Y *= -1.0f;

            // Horizontal wall relocation:
            if ((_position.X + 0) <= halfBounds / 2)
                _position.X += bounds;
            if ((Globals.MapWidth - _position.X) <= halfBounds / 2)
                _position.X -= bounds;

            // Vertical Wall relocation:
            if (_position.Y <= halfBounds)
                _position.Y += bounds;
            if ((Globals.MapHeight - _position.Y) <= halfBounds)
                _position.Y -= bounds;
        }

        // Move 
        // This is a bit limited in its current state. 
        private void Move() // Pass in a Vector 3 instead?
        {
            if (_nearestTarget != null) // Remove this and put it in its own "" method
            {
                _direction = Globals.GetUnitVector(_position, _nearestTarget.Position);
            }

            _position -= _direction * _velocity;
        }

        // Mark the object for deletion if it is outside the bounds of the map:
        private void CheckForRemoval()
        {
            if (_position.X < 0 || _position.X > Globals.MapWidth ||
                _position.Y < 0 || _position.Y > Globals.MapHeight)
            {
                _delete = true;
            }    
        }

    }
}
