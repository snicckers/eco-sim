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

namespace EcoSim
{
    class Globals
    {

        /*------------------- FIELDS -----------------------------------------------*/
        // Game World:
        public static int _screenHeight, _screenWidth;
        public static ContentManager _content;
        public static SpriteBatch _spriteBatch;
        public static GameTime _gameTime;
        public static Camera2D _camera;
        public static float _initialZoom = 0.6f;

        // Camera:
        public static int _mapHeight = 5000;
        public static int _mapWidth = 3000;

        // Colors: (make this its own class)
        public static Color _colorA = new Color(51, 34, 68);
        public static Color _colorB = new Color(85, 102, 170);
        public static Color _colorC = new Color(170, 119, 153);
        public static Color _colorD = new Color(221, 153, 153);
        public static Color _colorE = new Color(255, 221, 153);

        /*------------------- METHODS ----------------------------------------------*/
        // Return Distance (Between 2 points)
        public static float GetDistance(Vector2 reference, Vector2 target)
        {
            return (float)Math.Sqrt(Math.Pow(reference.X - target.X, 2) +
                                     Math.Pow(reference.Y - target.Y, 2));
        }

        // Return Unit Vector (Direction)
        public static Vector2 GetUnitVector(Vector2 reference, Vector2 target)
        {
            Vector2 direction = target - reference;
            float magnitude = (float)Math.Sqrt((direction.X * direction.X) + (direction.Y * direction.Y));
            direction.X = direction.X / magnitude;
            direction.Y = direction.Y / magnitude;

            return direction;
        }

        // Return the nearest entity to the reference point from a list of entities
        public static BasicEntity GetNearest(List<BasicEntity> EntityList, BasicEntity Reference, float Range)
        {
            BasicEntity BestTarget = null;
            float Distance = Range * Range; // Because our distance comparison has no square root

            if (EntityList != null)
            {
                foreach (BasicEntity Entity in EntityList)
                {
                    Vector2 Difference = Entity.Position - Reference.Position;
                    float CurrentDistance = Difference.X * Difference.X + Difference.Y * Difference.Y;

                    if (CurrentDistance < Distance)
                    {

                        if (CurrentDistance > 0.0f)
                        {
                            BestTarget = Entity;
                            Distance = CurrentDistance;
                        }
                    }
                }
            }
            
            return BestTarget;
        }

        // Draw a line between two points
        public static void DrawLine(SpriteBatch Batch, Vector2 begin, Vector2 end, Color color, Texture2D Tex, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            Batch.Draw(Tex, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
