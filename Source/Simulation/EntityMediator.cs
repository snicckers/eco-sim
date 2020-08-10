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
    class EntityMediator
    {
        /* Fields */
        // Consider using a dictionary?
        private List<BasicEntity> _entities = new List<BasicEntity>();


        /* Constructors */
        public EntityMediator()
        {
            // Ehh leave this blank for now.
        }

        
        /* Update */
        public void Update(GameTime gameTime)
        {
            List<BasicEntity> ToBeRemoved = new List<BasicEntity>();

            foreach (BasicEntity e in _entities)
            {
                e.Update(_entities, gameTime);

                if (e.Delete)
                    ToBeRemoved.Add(e);
                    //RemoveEntity(e, ToBeRemoved);
            }

            foreach (BasicEntity e in ToBeRemoved)
            {
                _entities.Remove(e);
            }

        }

        /* Draw */
        public void Draw()
        {
            foreach (BasicEntity e in _entities)
            {
                e.Draw(_entities);
            }
        }

        /* Methods */
        public void AddEntity(BasicEntity ENTITY)
        {
            _entities.Add(ENTITY);
        }

        public void RemoveEntity(BasicEntity Entity, List<BasicEntity> ToBeRemoved)
        {
            ToBeRemoved.Add(Entity);
            //_entities.Remove(Entity);
        }

        public void RemoveAll()
        {
            _entities.Clear();
        }
    }
}
