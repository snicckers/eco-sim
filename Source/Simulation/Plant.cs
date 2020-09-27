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
    class Plant : Entity
    {
        protected NicksTimer _spawnTimer;

        public Plant (Vector2 Pos, string Path, EntityTypes Type) : base(Pos, Path, Type)
        {
            base._acceleration = new Vector2(0, 0);
            base._velocity = new Vector2(0, 0);
            base._maxVel = 0.0f;
            base._dimensions = new Vector2(32, 32);
            base._color = Color.Green;
            base._accRate = 0.0f;

            this._spawnTimer = new NicksTimer((float)Globals.GenerateRandomNumber(9, 18));
        }

        public override void Update(List<Entity> EntityList, GameTime gameTime)
        {



            this.Reproduction(gameTime);
            base.CheckForRemoval();
            //base.Update(EntityList, gameTime);
        }

        public override void Draw()
        {
            base.Draw();
        }

        protected override void Reproduction(GameTime gameTime)
        {
            if (_spawnTimer.Finished)
            {
                int chance = Globals.GenerateRandomNumber(0, 100);
                if (chance >= 60)
                {
                    this._spawnTimer.Reset();
                    base._spawn = true;
                    //base._delete = true;
                }
                else if (chance <= 20)
                {
                    base._delete = true;
                }

            }
            else
            {
                this._spawnTimer.Update(gameTime);
                base._spawn = false;
            }

            base.Reproduction(gameTime);
        }

    }
}
