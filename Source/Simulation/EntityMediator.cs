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
        /*------------------- FIELDS -----------------------------------------------*/
        // Consider using a dictionary?
        private List<Entity> _entities = new List<Entity>();
        private EntityFactory _factory = new EntityFactory();

        internal EntityFactory Factory { get => _factory; set => _factory = value; }

        /*------------------- Constructors -----------------------------------------*/
        public EntityMediator()
        {
            
        }

        /*------------------- Update -----------------------------------------------*/
        public void Update(GameTime gameTime)
        {
            List<Entity> ToBeRemoved = new List<Entity>();
            List<Entity> ToBeAdded = new List<Entity>();

            foreach (Entity e in _entities)
            {
                e.Update(_entities, gameTime);

                if (e.Spawn)
                {

                    if (e.Type == EntityTypes.e_baseEntity)
                    {
                        Vector2 InitialPosition = new Vector2(e.Position.X, e.Position.Y);
                        ToBeAdded.Add(this.Factory.Build(EntityTypes.e_baseEntity, InitialPosition));
                    }

                    if (e.Type == EntityTypes.e_spore)
                    {
                        Vector2 InitialPosition = new Vector2(e.Position.X, e.Position.Y);
                        ToBeAdded.Add(this.Factory.Build(EntityTypes.e_flora, InitialPosition));
                    }

                    if (e.Type == EntityTypes.e_flora)
                    {
                        Vector2 InitialPosition = new Vector2(e.Position.X, e.Position.Y);
                        ToBeAdded.Add(this.Factory.Build(EntityTypes.e_spore, InitialPosition));
                    }
                    
                }

                if (e.Delete)
                    ToBeRemoved.Add(e);
            }

            foreach (Entity e in ToBeAdded)
            {
                _entities.Add(e);
            }

            foreach (Entity e in ToBeRemoved)
            {
                _entities.Remove(e);
            }
        }

        /*------------------- Draw -------------------------------------------------*/
        public void Draw()
        {
            foreach (Entity e in _entities)
            {
                if (e != null)
                {
                    if (e.Type == EntityTypes.e_baseEntity)
                    {
                        e.Draw(_entities);
                    }
                    if (e.Type == EntityTypes.e_spore)
                    {
                        e.Draw();
                    }

                    if (e.Type == EntityTypes.e_flora)
                        e.Draw();


                }
            }
        }

        /*------------------- Methods ----------------------------------------------*/
        public void AddEntity(Entity ENTITY)
        {
            _entities.Add(ENTITY);
        }

        public void AddEntity(EntityTypes Type, Vector2 Pos)
        {
            _entities.Add(this.Factory.Build(Type, Pos));
        }

        public void RemoveEntity(Entity Entity, List<Entity> ToBeRemoved)
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
