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

namespace EcoSim.Source.Engine
{
    public class NicksTimer
    {
        /*------------------- Fields -----------------------------------------------*/
        private float _timerDuration;
        private float _elapsedTime;
        private bool _finished;

        /*------------------- Accessors --------------------------------------------*/
        public bool Finished { get => _finished; set => _finished = value; }

        /*------------------- Constructors -----------------------------------------*/
        public NicksTimer(float Duration)
        {
            _timerDuration = Duration;
        }

        /*------------------- Update -----------------------------------------------*/
        public void Update(GameTime GameTime)
        {
            float deltaTime = (float)GameTime.ElapsedGameTime.TotalSeconds;
            _elapsedTime += deltaTime;

            if (_elapsedTime >= _timerDuration)
            {
                _finished = true;
            }
        }
    }
}
