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
#endregion

namespace EcoSim.Source.Simulation
{
    public enum EntityTypes
    {
        e_predator,
        e_prey,
        e_flora,
        e_baseEntity
    }

    class EntityFactory
    {
        public Entity Factory(EntityTypes Type, Vector2 Position)
        {
            switch (Type)
            {
                // Create Flora Entity;
                case EntityTypes.e_flora:
                    return null;

                // Create Predator Entity:
                case EntityTypes.e_predator:
                    return null;

                // Create Prey Entity
                case EntityTypes.e_prey:
                    return null;

                // Create Base Entity 
                case EntityTypes.e_baseEntity:
                    return new Entity(Position, "Primitives\\Square");

                // Null
                default:
                    return null;
            }
        }
    }
}
