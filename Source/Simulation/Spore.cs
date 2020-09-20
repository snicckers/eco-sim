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

namespace EcoSim
{
    public class Spore : Entity
    {
        /*
         * What's the point?
         * The spore is an entity that moves around semi-randomely. 
         
         
         */

        /*------------------- Fields -----------------------------------------------*/
        private Vector2 _acceleration;
        private float _maxVel;
        private NicksTimer _directionTimer;

        /*------------------- Accessors --------------------------------------------*/

        /*------------------- Constructors -----------------------------------------*/
        public Spore (Vector2 Pos, string Path) : base(Pos, Path)
        {
            _acceleration = new Vector2(0, 0);
            _velocity = new Vector2(0, 0);
            _maxVel = 5.0f;
            _directionTimer = new NicksTimer(3.0f);
        }

        /*------------------- Update -----------------------------------------------*/
        public override void Update(List<Entity> EntityList, GameTime gameTime)
        {
            this.Behaviour(gameTime);
            this.Acceleration();
            this.Move();
            this._directionTimer.Update(gameTime);

            base.CollisionAndBounds();
            base.CheckForRemoval();
        }

        /*------------------- Draw -------------------------------------------------*/
        public override void Draw()
        {
            base.Draw();
        }

        /*------------------- Methods ----------------------------------------------*/
        private void Behaviour(GameTime gameTime)
        {
            if (_directionTimer.Finished)
            {
                _color = Color.Red;

                Random rnd = new Random();
                float xDir = (float)rnd.Next(-1000, 1000);
                float yDir = (float)rnd.Next(-1000, 1000);

                xDir /= 2000.0f;
                yDir /= 2000.0f;

                base._direction = new Vector2(xDir, yDir);

                _directionTimer.Reset();
            }
            else
            {
                _color = Color.Blue;
            }

        }

        private void Acceleration()
        {

            if (base._direction != null)
            {
                float flatAccel = 0.1f;
                _acceleration = base._direction * flatAccel;

                base._velocity += _acceleration;

                // Clamp the speed:
                float flatSpeed = 3.0f;
                if (base._velocity.X >= flatSpeed)
                    base._velocity.X = flatSpeed;

                if (base._velocity.X <= -flatSpeed)
                    base._velocity.X = -flatSpeed;

                if (base._velocity.Y >= flatSpeed)
                    base._velocity.Y = flatSpeed;

                if (base._velocity.Y <= -flatSpeed)
                    base._velocity.Y = -flatSpeed;
            }
        }

        protected override void Move()
        {
            if (base._direction != null)
            {
                base._position += base._velocity;
            }
        }

    }
}
