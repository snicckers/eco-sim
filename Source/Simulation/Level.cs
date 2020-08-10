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
            Globals._cursor = new Cursor("Primitives\\Square");
            Globals._mouse = new MouseControl();

            _mediator = new EntityMediator();
            _factory = new EntityFactory();

            // Testing:
            //_mediator.AddEntity(_factory.Factory(EntityTypes.e_baseEntity, new Vector2(-200.0f, -200.0f)));
            //_mediator.AddEntity(_factory.Factory(EntityTypes.e_baseEntity, new Vector2(-200.0f, -250.0f)));
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
            Globals._cursor.Update(mousePos);

            //--- End Input During Update
            Globals._mouse.UpdateOld();
            
        }

        /*------------------- DRAW -------------------------------------------------*/
        public virtual void Draw()
        {
            // Draw Order: Last item is drawn on top. 
            _mediator.Draw();
            Globals._cursor.Draw();

            DrawBorders();
        }

        /*------------------- METHODS ----------------------------------------------*/
        // Should this be made into its own class?
        private void Controls(MouseState mouseState, KeyboardState keyState)
        {
            if ((mouseState.LeftButton == ButtonState.Pressed) && (Keyboard.GetState().IsKeyDown(Keys.F)))
            {
                Vector2 InitialPosition = new Vector2(Globals._cursor.Position.X, Globals._cursor.Position.Y);
                _mediator.AddEntity(_factory.Factory(EntityTypes.e_baseEntity, InitialPosition));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                _mediator.RemoveAll();
            }

            if (keyState.IsKeyDown(Keys.E))
                Globals._camera.Zoom += 0.01f;

            if (keyState.IsKeyDown(Keys.Q))
                Globals._camera.Zoom -= 0.01f;

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

            Globals._camera.Pos += movement * 20;
        }

        // Should this be put into globals or made into its own class???
        // Probably put into Globals
        private void DrawBorders()
        {
            Texture2D _texture = Globals._content.Load<Texture2D>("Primitives\\Square"); 
            Vector2 TopLeft = new Vector2(0.0f, 0.0f);
            Vector2 TopRight = new Vector2(Globals.MapWidth, 0.0f);
            Vector2 BottomLeft = new Vector2(0.0f, Globals.MapHeight);
            Vector2 BottomRight = new Vector2(Globals.MapWidth, Globals.MapHeight);

            Globals.DrawLine(Globals._spriteBatch, TopLeft, TopRight, Globals._colorE, _texture, 5);
            Globals.DrawLine(Globals._spriteBatch, TopLeft, BottomLeft, Globals._colorE, _texture, 5);
            Globals.DrawLine(Globals._spriteBatch, TopRight, BottomRight, Globals._colorE, _texture, 5);
            Globals.DrawLine(Globals._spriteBatch, BottomLeft, BottomRight, Globals._colorE, _texture, 5);
        }
    }
}
