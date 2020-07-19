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
#endregion

namespace EcoSim.Source.Engine
{
    public class BasicTimeControl
    {
        public bool _goodToGo;
        protected int _mSec;
        protected TimeSpan _timer = new TimeSpan();


        public BasicTimeControl(int m)
        {
            _goodToGo = false;
            _mSec = m;
        }
        public BasicTimeControl(int m, bool STARTLOADED)
        {
            _goodToGo = STARTLOADED;
            _mSec = m;
        }

        public int MSec
        {
            get { return _mSec; }
            set { _mSec = value; }
        }
        public int Timer
        {
            get { return (int)_timer.TotalMilliseconds; }
        }
            


        public void UpdateTimer()
        {
            _timer += Globals._gameTime.ElapsedGameTime;
        }

        public void UpdateTimer(float SPEED)
        {
            _timer += TimeSpan.FromTicks((long)(Globals._gameTime.ElapsedGameTime.Ticks * SPEED));
        }

        public virtual void AddToTimer(int MSEC)
        {
            _timer += TimeSpan.FromMilliseconds((long)(MSEC));
        }

        public bool Test()
        {
            if (_timer.TotalMilliseconds >= _mSec || _goodToGo)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            _timer = _timer.Subtract(new TimeSpan(0, 0, _mSec / 60000, _mSec / 1000, _mSec % 1000));
            if (_timer.TotalMilliseconds < 0)
            {
                _timer = TimeSpan.Zero;
            }
            _goodToGo = false;
        }

        public void Reset(int NEWTIMER)
        {
            _timer = TimeSpan.Zero;
            MSec = NEWTIMER;
            _goodToGo = false;
        }

        public void ResetToZero()
        {
            _timer = TimeSpan.Zero;
            _goodToGo = false;
        }

        public virtual XElement ReturnXML()
        {
            XElement xml = new XElement("Timer",
                                    new XElement("mSec", _mSec),
                                    new XElement("timer", Timer));



            return xml;
        }

        public void SetTimer(TimeSpan TIME)
        {
            _timer = TIME;
        }

        public virtual void SetTimer(int MSEC)
        {
            _timer = TimeSpan.FromMilliseconds((long)(MSEC));
        }
    }
}
