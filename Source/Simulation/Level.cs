#region Includes
using EcoSim.Source.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace EcoSim.Source.Simulation
{
    public class Level
    {

        /*------------------- FIELDS -----------------------------------------------*/
        private EntityMediator _mediator = new EntityMediator();

        /*------------------- INITIALIZE -------------------------------------------*/
        public Level()
        {
            Globals.Mouse = new NicksMouse("Primitives\\Square");
            Globals.Input = new UserInput();
        }

        /*------------------- UPDATE -----------------------------------------------*/
        public virtual void Update(GameTime gameTime)
        {
            // User Input:
            KeyboardState keyState = Keyboard.GetState();
            Globals.Input.Update(keyState, _mediator);
  

            // Game Updates Go Here:
            _mediator.Update(gameTime);

            // End Input During Update:
            Globals.Mouse.Update();

        }

        /*------------------- DRAW -------------------------------------------------*/
        public virtual void Draw()
        {
            Globals.DrawBorders(Globals._richBlack); // This must be drawn first

            // Draw Order: Last item is drawn on top. 
            _mediator.Draw();
            Globals.Mouse.Draw();
        }

        /*------------------- METHODS ----------------------------------------------*/
        
    }
}
