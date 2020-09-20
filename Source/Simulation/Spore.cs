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
        private NicksTimer _directionTimer;

        /*------------------- Accessors --------------------------------------------*/

        /*------------------- Constructors -----------------------------------------*/
        public Spore (Vector2 Pos, string Path) : base(Pos, Path)
        {
            base._acceleration = new Vector2(0, 0);
            base._velocity = new Vector2(0, 0);
            base._maxVel = 3.0f;
            _directionTimer = new NicksTimer(1.0f);
            base._color = Globals._colorSR_C;

            base._accRate = 0.1f;

        }

        /*------------------- Update -----------------------------------------------*/
        public override void Update(List<Entity> EntityList, GameTime gameTime)
        {
            this.Behaviour(EntityList, gameTime);
            this.Move();
            
            this.CollisionAndBounds();
            base.CheckForRemoval();
        }

        /*------------------- Draw -------------------------------------------------*/
        public override void Draw()
        {
            base.Draw();
        }

        /*------------------- Methods ----------------------------------------------*/
        protected override void Behaviour(List<Entity> EntityList, GameTime gameTime)
        {
            if (_directionTimer.Finished)
            {
                base._color = Color.Red;

                Random rnd = new Random();
                float xDir = (float)rnd.Next(-1000, 1000);
                float yDir = (float)rnd.Next(-1000, 1000);

                xDir /= 2000.0f;
                yDir /= 2000.0f;

                base._direction = new Vector2(xDir, yDir);

                this._directionTimer.Reset();
            }
            else
            {
                base._color = Globals._colorSR_C;
                this._directionTimer.Update(gameTime);
            }

        }

        protected override void CollisionAndBounds()
        {

            base.CollisionAndBounds();
        }

        protected override void Move()
        {

            base.Move();
        }

    }
}
