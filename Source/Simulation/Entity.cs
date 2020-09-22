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
        protected Vector2 _position, _dimensions, _direction, _acceleration;
        protected Color _color;
        protected Texture2D _texture;
        private bool _drawingLine;
        protected float _sightRange, _maxVel, _accRate;
        private bool _delete;

        // Behaviour:
        private bool _fleeing;
        protected Vector2 _velocity;
        protected Entity _nearestTarget;
        private bool _spawn;

        // Timers:
        private float _scanTimer;
        private static float _scanTime = 0.1f;
        private float _fleeTimer;
        private float _fleeTime;
        private bool _slowDown;

        /*------------------- Accessors --------------------------------------------*/
        public Vector2 Position { get => _position; set => _position = value; }
        public float SightRange { get => _sightRange; }
        public bool DrawingLine { get => _drawingLine; }
        public bool Delete { get => _delete; }
        public bool Spawn { get => _spawn; set => _spawn = value; }

        /*------------------- Constructors -----------------------------------------*/
        public Entity(Vector2 Position, string Path)
        {
            this.Position = Position;
            _texture = Globals._content.Load<Texture2D>(Path);
            _color = Globals._blueSapphire;
            _dimensions = new Vector2(12, 12);
            _direction = new Vector2(0, 0);
            _sightRange = 30.0f;
            _velocity = new Vector2(0.0f, 0.0f);
            _maxVel = 2.0f;
            _accRate = 0.05f;

            Random rnd = new Random();
            _fleeTime = (float)rnd.Next(1, 3); // Radomize the time that entities runs away

            //Behaviour:
            _fleeing = false;
        }

        /*------------------- Update -----------------------------------------------*/
        public virtual void Update(List<Entity> EntityList, GameTime gameTime)
        {
            Behaviour(EntityList, gameTime);
            Move();
            CollisionAndBounds();
            CheckForRemoval();


            Random r = new Random();
            int Grow = r.Next(1, 100);

            if (Grow >= 90)
            {
                _dimensions.X += 1;
                _dimensions.Y += 1;
                _sightRange += 0.5f;
            }

            if (_dimensions.X >= 40)
            {

                _spawn = true;
                _delete = true;
            }
            
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

            // Sets fleeing timer & direction
            if (_nearestTarget != null)
            {
                _fleeing = true;
            }

            // Run away / fleeing timer
            if (_fleeing)
            {
                float deltaFleeTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                _fleeTimer += deltaFleeTime;

                if (_nearestTarget != null)
                {
                    _direction = -1.0f * Globals.GetUnitVector(_position, _nearestTarget.Position);
                }

                if (_fleeTimer >= _fleeTime)
                {
                    _slowDown = true;
                    //_direction *= 1.0f;
                    _fleeTimer = 0.0f;
                    _fleeing = false;
                }
            }

            if (_slowDown)
            {
                _acceleration = new Vector2(0.0f, 0.0f);
                _velocity.X /= 1.13f;
                _velocity.Y /= 1.13f;

                if (Math.Abs(_velocity.X) <= 0.5f && Math.Abs(_velocity.Y) <= 0.5f)
                {
                    _direction = new Vector2(0, 0);
                    _velocity = new Vector2(0, 0);
                    _slowDown = false;
                }

            }

            else if (!_fleeing && !_slowDown && _direction != new Vector2(0, 0))
            {
                _direction = new Vector2(0, 0);
                //_acceleration = new Vector2(0, 0);
                _velocity = new Vector2(0, 0);              
            }

        }

        // Reverse entity direction if it's tyring to move outside of map bounds
        protected virtual void CollisionAndBounds()
        {
            int bounds = 20;

            // Horizontal wall collision:
            if ((this._position.X + 0) <= bounds)
            {
                this._direction.X *= -1.0f;
                this._acceleration.X *= -1.0f;
                this._velocity.X *= -1.0f;
                this._position.X += 5;
            }

            if ((Globals.MapWidth - _position.X) <= bounds)
            {
                this._direction.X *= -1.0f;
                this._acceleration.X *= -1.0f;
                this._velocity.X *= -1.0f;
                this._position.X -= 5;
            }

            // Vertical Wall Collision:
            if (this._position.Y <= bounds)
            {
                this._direction.Y *= -1.0f;
                this._acceleration.Y *= -1.0f;
                this._velocity.Y *= -1.0f;
                this._position.Y += 5;
            }

            if ((Globals.MapHeight - _position.Y) <= bounds)
            {
                this._direction.Y *= -1.0f;
                this._acceleration.Y *= -1.0f;
                this._velocity.Y *= -1.0f;
                this._position.Y -= 5;
            }
        }

        // Acceleration
        protected virtual void Move()
        {
            _acceleration = _direction * _accRate;
            _velocity += _acceleration;
            _position += _velocity;

            if (_velocity.X >= _maxVel)
                _velocity.X = _maxVel;

            if (_velocity.X <= -_maxVel)
                _velocity.X = -_maxVel;

            if (_velocity.Y >= _maxVel)
                _velocity.Y = _maxVel;

            if (_velocity.Y <= -_maxVel)
                _velocity.Y = -_maxVel;
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
