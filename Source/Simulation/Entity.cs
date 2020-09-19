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
    public class Entity
    {
        /*------------------- Fields -----------------------------------------------*/
        protected Vector2 _position, _dimensions, _direction;
        protected Color _color;
        protected Texture2D _texture;
        private bool _drawingLine;
        private float _sightRange;
        private bool _delete;

        // Behaviour:
        private bool _fleeing;
        protected float _velocity;
        protected Entity _nearestTarget;

        // Timers:
        private float _scanTimer;
        private static float _scanTime = 0.1f;
        private float _fleeTimer;
        private float _fleeTime;

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
            _color = Globals._blueSapphire;
            _dimensions = new Vector2(12, 12);
            _direction = new Vector2(0, 0);
            _sightRange = 40.0f;
            _velocity = 4.0f;

            Random rnd = new Random();
            _fleeTime = (float)rnd.Next(0, 2); // Radomize the time that entities runs away

            //Behaviour:
            _fleeing = false;
        }

        /*------------------- Update -----------------------------------------------*/
        public virtual void Update(List<Entity> EntityList, GameTime gameTime)
        {
            Behaviour(EntityList, gameTime);
            CollisionAndBounds();
            Move();
            CheckForRemoval();
        }

        /*------------------- Draw -------------------------------------------------*/
        public virtual void Draw()
        {
            Rectangle rec = new Rectangle((int)Position.X, (int)Position.Y, (int)_dimensions.X, (int)_dimensions.Y);
            Vector2 center = new Vector2(_texture.Bounds.Width / 2, _texture.Bounds.Height / 2);

            Globals._spriteBatch.Draw(_texture, rec, null, _color, 0.0f, center, new SpriteEffects(), 0);
        }

        // Draw the object, and a line to the nearest target
        public virtual void Draw(List<Entity> EntityList)
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
                    Globals.DrawLine(Globals._spriteBatch, Point_A, Point_B, Globals._ghostWhite, _texture, 1);

                    // Change entity color if running away
                    _color = Globals._cyanProcess;
                }
                else
                {
                    _drawingLine = false;
                }

            }
            else
            {
                _drawingLine = false;
                _color = Globals._blueSapphire;
            }

            // Draw Self:
            Rectangle rec = new Rectangle((int)Position.X, (int)Position.Y, (int)_dimensions.X, (int)_dimensions.Y);
            Vector2 center = new Vector2(_texture.Bounds.Width / 2, _texture.Bounds.Height / 2);
            Globals._spriteBatch.Draw(_texture, rec, null, _color, 0.0f, center, new SpriteEffects(), 0);
        }

        /*------------------- Methods ----------------------------------------------*/

        // Run away from the nearest target
        protected virtual void Behaviour(List<Entity> EntityList, GameTime gameTime)
        {
            // Scan for enemies:
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; // Time control.
            _scanTimer += deltaTime;

            if (_scanTimer > _scanTime) // Limit the number of times that entities search for the nearest object
            {
                _nearestTarget = Globals.GetNearest(EntityList, this, this.SightRange);
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

                if (_fleeTimer >= _fleeTime)
                {
                    _direction = new Vector2(0, 0);
                    _fleeTimer = 0.0f;
                    _fleeing = false;
                }
            } 
            else if ((!_fleeing) && (_direction != new Vector2(0, 0))) 
            {
                _direction = new Vector2(0, 0); // Strange errors can pop up if you don't do this
            }
        }

        // Reverse entity direction if it's tyring to move outside of map bounds
        protected virtual void CollisionAndBounds()
        {
            int bounds = 20;

            // Horizontal wall collision:
            if ((_position.X + 0) <= bounds) 
            {
                _direction.X *= -1.0f;
                _position.X += 5;
            }

            if ((Globals.MapWidth - _position.X) <= bounds)
            {
                _direction.X *= -1.0f;
                _position.X -= 5;
            }

            // Vertical Wall Collision:
            if (_position.Y <= bounds)
            {
                _direction.Y *= -1.0f;
                _position.Y += 5;
            }

            if ((Globals.MapHeight - _position.Y) <= bounds)
            {
                _direction.Y *= -1.0f;
                _position.Y -= 5;
            }
        }

        // Move 
        protected virtual void Move() // Pass in a Vector 3 instead?
        {
            if (_nearestTarget != null) // Remove this and put it in its own "" method
            {
                _direction = Globals.GetUnitVector(_position, _nearestTarget.Position);
            }

            _position -= _direction * _velocity; // Run away
        }

        // Mark the object for deletion if it is outside the bounds of the map:
        protected void CheckForRemoval()
        {
            if (_position.X < 0 || _position.X > Globals.MapWidth ||
                _position.Y < 0 || _position.Y > Globals.MapHeight)
            {
                _delete = true;
            }    
        }

    }
}
