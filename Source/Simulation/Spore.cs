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
        private float _acceleration;
        private float _maxVel;
        private NicksTimer _directionTimer;

        /*------------------- Accessors --------------------------------------------*/

        /*------------------- Constructors -----------------------------------------*/
        public Spore (Vector2 Pos, string Path) : base(Pos, Path)
        {
            _acceleration = 0.0f;
            _velocity = 0.0f;
            _maxVel = 5.0f;
        }

        /*------------------- Update -----------------------------------------------*/
        public override void Update(List<Entity> EntityList, GameTime gameTime)
        {
            Behaviour(gameTime);
            Acceleration();

            base.Move();
            base.CollisionAndBounds();
            base.CheckForRemoval();
        }

        /*------------------- Draw -------------------------------------------------*/
        public override void Draw(List<Entity> EntityList)
        {
            base.Draw(EntityList);
        }

        /*------------------- Methods ----------------------------------------------*/
        private void Behaviour(GameTime gameTime)
        {


            // Change the direction randomely:
            Random rnd = new Random();
            float xDir = (float)rnd.Next(0, 1);
            float yDir = (float)rnd.Next(0, 1);

            base._direction = new Vector2(xDir, yDir);

        }

        private void Acceleration()
        {
            if (base._velocity <= _maxVel)
            {
                
                
            }
        }

        protected override void Move()
        {
            if (base._direction != null)
            {
                base._position += (base._direction * base._velocity);
            }
        }

    }
}
