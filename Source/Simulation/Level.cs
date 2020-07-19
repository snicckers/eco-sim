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
using EcoSim.Source.Engine;
#endregion

namespace EcoSim.Source.Simulation
{
    public class Level
    {

        /*------------------- FIELDS -----------------------------------------------*/
        private Cursor _cursor;
        private EntityMediator _mediator;
        private EntityFactory _factory;

        /*------------------- INITIALIZE -------------------------------------------*/
        public Level()
        {
            _cursor = new Cursor("Primitives\\Square");
            Globals._mouse = new MouseControl();
            
            _mediator = new EntityMediator();
            _factory = new EntityFactory();

            _mediator.AddEntity(_factory.Factory(EntityTypes.e_baseEntity, new Vector2(200.0f, 200.0f)));
            _mediator.AddEntity(_factory.Factory(EntityTypes.e_baseEntity, new Vector2(200.0f, 250.0f)));
        }

        /*------------------- UPDATE -----------------------------------------------*/
        public virtual void Update(GameTime gameTime)
        {
            /* Mouse & Keyboard: */
            // Poll for current keyboard state:
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            Globals._mouse.Update();

            if ((mouseState.LeftButton == ButtonState.Pressed) && (Keyboard.GetState().IsKeyDown(Keys.E)))
            {
                Vector2 InitialPosition = new Vector2(_cursor.Position.X, _cursor.Position.Y);
                _mediator.AddEntity(_factory.Factory(EntityTypes.e_baseEntity, InitialPosition));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                _mediator.RemoveAll();
            }

            //--- Game Updates Go Here:
            _mediator.Update(gameTime);
            _cursor.Update(new Vector2(Globals._mouse.newMouse.X, Globals._mouse.newMouse.Y));

            //--- End Input During Update
            Globals._mouse.UpdateOld();
        }   

        /*------------------- DRAW -------------------------------------------------*/
        public virtual void Draw()
        {
            // Draw Order: Last item is drawn on top. 
            _mediator.Draw();
            _cursor.Draw(); 
        }

        /*------------------- METHODS ----------------------------------------------*/

    }
}
