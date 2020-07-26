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
        private Cursor _cursor;
        private EntityMediator _mediator;
        private EntityFactory _factory;

        /*------------------- INITIALIZE -------------------------------------------*/
        public Level()
        {
            _cursor = new Cursor("Primitives\\Square");

            _mediator = new EntityMediator();
            _factory = new EntityFactory();

            _mediator.AddEntity(_factory.Factory(EntityTypes.e_baseEntity, new Vector2(200.0f, 200.0f)));
            _mediator.AddEntity(_factory.Factory(EntityTypes.e_baseEntity, new Vector2(200.0f, 250.0f)));
        }

        /*------------------- UPDATE -----------------------------------------------*/
        public virtual void Update(GameTime gameTime)
        {
            //--- User Input
            // Poll for current keyboard state:
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            Controls(mouseState, keyState);

            //--- Game Updates Go Here:
            _mediator.Update(gameTime);

            //--- Mouse Updates
            // Transform mouse input from view to world position
            Matrix inverse = Matrix.Invert(Globals._camera.GetTransformation());
            Vector2 mousePos = Vector2.Transform(new Vector2(mouseState.X, mouseState.Y), inverse);
            _cursor.Update(mousePos);

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
        private void Controls(MouseState mouseState, KeyboardState keyState)
        {
            if ((mouseState.LeftButton == ButtonState.Pressed) && (Keyboard.GetState().IsKeyDown(Keys.F)))
            {
                Vector2 InitialPosition = new Vector2(_cursor.Position.X, _cursor.Position.Y);
                _mediator.AddEntity(_factory.Factory(EntityTypes.e_baseEntity, InitialPosition));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                _mediator.RemoveAll();
            }

            //int previousScroll;
            //// Adjust zoom if the mouse wheel has moved
            //if (mouseState.ScrollWheelValue > previousScroll)
            //    Globals._camera.Zoom += zoomIncrement;
            //else if (mouseState.ScrollWheelValue < previousScroll)
            //    Globals._camera.Zoom -= zoomIncrement;

            //previousScroll = mouseState.ScrollWheelValue;

            // Move the camera when the arrow keys are pressed
            Vector2 movement = Vector2.Zero;
            Viewport vp = Globals._spriteBatch.GraphicsDevice.Viewport;

            if (keyState.IsKeyDown(Keys.Left))
                movement.X--;
            if (keyState.IsKeyDown(Keys.Right))
                movement.X++;
            if (keyState.IsKeyDown(Keys.Up))
                movement.Y--;
            if (keyState.IsKeyDown(Keys.Down))
                movement.Y++;

            Globals._camera.Pos += movement * 20;
        }

    }
}
