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
        private static int _screenHeight, _screenWidth;
        public static ContentManager _content;
        public static SpriteBatch _spriteBatch;
        public static GameTime _gameTime;
        public static Camera2D _camera;
        public static MouseControl _mouse;
        public static float _initialZoom = 0.6f;
        public static Cursor _cursor;

        // Camera:
        private static int _mapHeight = 2000;
        private static int _mapWidth = 4000;

        // Colors: (make this its own class)
        public static Color _colorA = new Color(51, 34, 68);
        public static Color _colorB = new Color(85, 102, 170);
        public static Color _colorC = new Color(170, 119, 153);
        public static Color _colorD = new Color(221, 153, 153);
        public static Color _colorE = new Color(255, 221, 153);

        // Colors: Dark Actionable
        public static Color _colorDA_A = new Color(69, 172, 229);
        public static Color _colorDA_B = new Color(45, 115, 153);
        public static Color _colorDA_C = new Color(30, 77, 102);

        // Colors: Status Green
        public static Color _colorG_A = new Color(180, 227, 79);
        public static Color _colorG_B = new Color(127, 174, 27);
        public static Color _colorG_C = new Color(66, 88, 14);

        // Colors: Status Red
        public static Color _colorSR_A = new Color(249, 165, 159);
        public static Color _colorSR_D = new Color(243, 67, 54);
        public static Color _colorSR_C = new Color(180, 0, 0);

        // Colors: Greys
        public static Color _colorGrey_A = new Color(12, 20, 25);
        public static Color _colorGrey_B = new Color(103, 126, 140);
        public static Color _colorGrey_C = new Color(182, 195, 204);

        public static int ScreenHeight { get => _screenHeight; set => _screenHeight = value; }
        public static int ScreenWidth { get => _screenWidth; set => _screenWidth = value; }
        public static int MapHeight { get => _mapHeight; set => _mapHeight = value; }
        public static int MapWidth { get => _mapWidth; set => _mapWidth = value; }


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
        public static void DrawLine(SpriteBatch Batch, Vector2 begin, Vector2 end, Color color, Texture2D Tex, int width)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            Batch.Draw(Tex, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        // Draw borders around the outside of the map
        public static void DrawBorders(Color color)
        {
            Texture2D _texture = Globals._content.Load<Texture2D>("Primitives\\Square");
            Vector2 TopLeft = new Vector2(0.0f, 0.0f);
            Vector2 TopRight = new Vector2(Globals.MapWidth, 0.0f);
            Vector2 BottomLeft = new Vector2(0.0f, Globals.MapHeight);
            Vector2 BottomRight = new Vector2(Globals.MapWidth, Globals.MapHeight);

            Rectangle rec = new Rectangle(Globals.MapWidth / 2, Globals.MapHeight / 2, Globals.MapWidth, Globals.MapHeight);
            Vector2 center = new Vector2(_texture.Bounds.Width / 2, _texture.Bounds.Height / 2);
            Globals._spriteBatch.Draw(_texture, rec, null, color, 0.0f, center, new SpriteEffects(), 0);

            Globals.DrawLine(Globals._spriteBatch, TopLeft, TopRight, Globals._colorE, _texture, 5);
            Globals.DrawLine(Globals._spriteBatch, TopLeft, BottomLeft, Globals._colorE, _texture, 5);
            Globals.DrawLine(Globals._spriteBatch, TopRight, BottomRight, Globals._colorE, _texture, 5);
            Globals.DrawLine(Globals._spriteBatch, BottomLeft, BottomRight, Globals._colorE, _texture, 5);
        }
    }
}

