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
        private NicksTimer _spawnTimer;

        /*------------------- Accessors --------------------------------------------*/

        /*------------------- Constructors -----------------------------------------*/
        public Spore (Vector2 Pos, string Path, EntityTypes Type) : base(Pos, Path, Type)
        {
            base._acceleration = new Vector2(0, 0);
            base._velocity = new Vector2(0, 0);
            base._maxVel = 3.0f;
            _directionTimer = new NicksTimer((float)Globals.GenerateRandomNumber(1, 3));
            _spawnTimer = new NicksTimer((float)Globals.GenerateRandomNumber(3, 9));


            base._color = Color.Red;

            base._accRate = 0.1f;

        }

        /*------------------- Update -----------------------------------------------*/
        public override void Update(List<Entity> EntityList, GameTime gameTime)
        {
            this.Behaviour(EntityList, gameTime);
            this.Move();
            this.CollisionAndBounds();
            this.Reproduction(gameTime);

            base.CheckForRemoval();
        }

        /*------------------- Draw -------------------------------------------------*/
        public override void Draw()
        {
            if (_directionTimer.Finished)
            {
                base._color = Color.Red;
            }
            else
            {
                base._color = Color.Blue;
            }

            base.Draw();
        }

        /*------------------- Methods ----------------------------------------------*/
        protected override void Behaviour(List<Entity> EntityList, GameTime gameTime)
        {
            if (_directionTimer.Finished)
            {
                base._color = Color.Red;

                
                float xDir = (float)Globals.GenerateRandomNumber(-1000, 1000);
                float yDir = (float)Globals.GenerateRandomNumber(-1000, 1000);

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

        protected override void Reproduction(GameTime gameTime)
        {
            if (_spawnTimer.Finished)
            {
                base._spawn = true;
                base._delete = true;
                
            }
            else
            {
                this._spawnTimer.Update(gameTime);
            }

            base.Reproduction(gameTime);
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
