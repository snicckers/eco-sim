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
        /* --------- Fields ------------------------------------------ */
        private Vector2 _position, _dimensions;
        private Color _color;
        private Texture2D _texture;
        private BasicEntity _nearestTarget;
        private bool _drawingLine;
        private float _sightRange;
        private float _scanTimer;
        private static float _scanTime = 0.1f;
        private float _velocity;
        private bool _delete;

        /* ---------- Accessors -------------------------------------- */
        public Vector2 Position { get => _position; set => _position = value; }
        public float SightRange { get => _sightRange; }
        public bool DrawingLine { get => _drawingLine; }
        public bool Delete { get => _delete; }

        /* ---------- Constructors ----------------------------------- */
        public BasicEntity(Vector2 Position, string Path)
        {
            this.Position = Position;
            _texture = Globals._content.Load<Texture2D>(Path);
            _color = Globals._colorG_B;
            _dimensions = new Vector2(12, 12);
            _sightRange = 20.0f;
            _velocity = 3.0f;
        }

        /* ---------- Update ----------------------------------------- */
        public virtual void Update(List<BasicEntity> EntityList, GameTime gameTime)
        {
            Flee(EntityList, gameTime);
            CheckForRemoval();
        }

        /*---------- Draw -------------------------------------------- */
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

        /* ---------- Methods ---------------------------------------- */

        // Run away from the nearest target
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

            Move();
        }

        // Move 
        // This is a bit limited in its current state. 
        private void Move() // Pass in a Vector 3 instead?
        {
            Vector2 Direction = new Vector2(0.0f, 0.0f);

            if (_nearestTarget != null) // Remove this and put it in its own "" method
            {
                Direction = Globals.GetUnitVector(_position, _nearestTarget.Position);
                
            }

            // If close to map boundry, set direction to opposite that of boundry
            //else if ()
            //{

            //}

            _position -= Direction * _velocity;

            // How do we make it move away from the boundary?
            // What if the line is not a vector?
            // How do I get a direction from a line
            // Can I get an xy point that is a point on the line, closest to the entity? 
            // That seems like the best way to do it
        }

        // Check the distance...
        private void IsCloseToLine()
        {

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
