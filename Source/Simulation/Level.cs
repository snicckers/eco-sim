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
        private EntityMediator _mediator;
        private EntityFactory _factory;

        /*------------------- INITIALIZE -------------------------------------------*/
        public Level()
        {
            Globals.Mouse = new NicksMouse("Primitives\\Square");

            _mediator = new EntityMediator();
            _factory = new EntityFactory();
        }

        /*------------------- UPDATE -----------------------------------------------*/
        public virtual void Update(GameTime gameTime)
        {
            // User Input:
            // Poll for current keyboard state:
            KeyboardState keyState = Keyboard.GetState();
            Controls(keyState);

            // Game Updates Go Here:
            _mediator.Update(gameTime, _factory);

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
        // Turn controls into it's own class
        private void Controls(KeyboardState keyState)
        {
            if ((Globals.Mouse.LeftClickDown()) && (Keyboard.GetState().IsKeyDown(Keys.F)))
            {
                Vector2 InitialPosition = new Vector2(Globals.Mouse.WorldLocation.X, Globals.Mouse.WorldLocation.Y);
                _mediator.AddEntity(_factory.Factory(EntityTypes.e_baseEntity, InitialPosition));
            }

            if ((Globals.Mouse.LeftClickDown()) && (Keyboard.GetState().IsKeyDown(Keys.G)))
            {
                Vector2 InitialPosition = new Vector2(Globals.Mouse.WorldLocation.X, Globals.Mouse.WorldLocation.Y);
                _mediator.AddEntity(_factory.Factory(EntityTypes.e_spore, InitialPosition));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.X))
                _mediator.RemoveAll();

            if (keyState.IsKeyDown(Keys.E) || Globals.Mouse.ScrollUp)
                Globals._camera.Zoom += 0.1f;

            if (keyState.IsKeyDown(Keys.Q) || Globals.Mouse.ScrollDown)
                Globals._camera.Zoom -= 0.1f;

            // Move the camera when the arrow keys are pressed
            Vector2 movement = Vector2.Zero;
           
            if (keyState.IsKeyDown(Keys.A))
                movement.X--;
            if (keyState.IsKeyDown(Keys.D))
                movement.X++;
            if (keyState.IsKeyDown(Keys.W))
                movement.Y--;
            if (keyState.IsKeyDown(Keys.S))
                movement.Y++;

            if (keyState.IsKeyDown(Keys.Space))
            {
                Globals._camera.Pos = new Vector2(0, 0);
            }

            Globals._camera.Pos += movement * 20;
        }
    }
}
