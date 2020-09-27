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

namespace EcoSim.Source.Engine
{
    class UserInput
    {
        // Turn controls into it's own class
        public void Update(KeyboardState keyState, EntityMediator mediator)
        {
            if ((Globals.Mouse.LeftClickDown()) && (Keyboard.GetState().IsKeyDown(Keys.F)))
            {
                Vector2 InitialPosition = new Vector2(Globals.Mouse.WorldLocation.X, Globals.Mouse.WorldLocation.Y);
                mediator.AddEntity(EntityTypes.e_baseEntity, InitialPosition);
            }

            if ((Globals.Mouse.LeftClickDown()) && (Keyboard.GetState().IsKeyDown(Keys.G)))
            {
                Vector2 InitialPosition = new Vector2(Globals.Mouse.WorldLocation.X, Globals.Mouse.WorldLocation.Y);
                mediator.AddEntity(EntityTypes.e_spore, InitialPosition);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.X))
                mediator.RemoveAll();

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
