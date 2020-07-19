using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using EcoSim.Source.Simulation;
using EcoSim.Source.Engine;

namespace EcoSim
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        /*------------------- FIELDS -----------------------------------------------*/
        GraphicsDeviceManager _graphics;
        Level _world;

        /*--------------------------------------------------------------------------*/
        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }

        /*------------------- INITIALIZE -------------------------------------------*/
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Globals._screenWidth = 800;
            Globals._screenHeight = 600;
            Window.AllowUserResizing = true;

            _graphics.PreferredBackBufferWidth = Globals._screenWidth;
            _graphics.PreferredBackBufferHeight = Globals._screenHeight;
            _graphics.ApplyChanges();

            
            base.Initialize();
        }

        /*------------------- LOAD CONTENT -----------------------------------------*/
        protected override void LoadContent()
        {
            Globals._content = this.Content;
            Globals._spriteBatch = new SpriteBatch(GraphicsDevice);

            
            //--- Load World
            _world = new Level();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /*------------------- UPDATE -----------------------------------------------*/
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Globals._gameTime = gameTime; // Ah. Note to self: Get rid of this idiot's code. 

           
            _world.Update(gameTime);

            base.Update(gameTime);
        }

        /*------------------- DRAW -------------------------------------------------*/
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Globals._colorA);

            Globals._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
           
            _world.Draw();

            Globals._spriteBatch.End();

            base.Draw(gameTime);
        }
    }


    #if WINDOWS || LINUX
        /// <summary>
        /// The main class.
        /// </summary>
        public static class Program
        {
            /// <summary>
            /// The main entry point for the application.
            /// </summary>
            /*[STAThread]*/
            static void Main()
            {
                using (var game = new Main())
                    game.Run();
            }
        }
    #endif
}
