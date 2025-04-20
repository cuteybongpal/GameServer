using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GameServer.Game;
using GameServer.Game.Components;

namespace GameServer.Game
{
    internal class Entity
    {
        public Vector2 Position = Vector2.Zero;
        public string name = "NewGameObject";
        public List<Component> Components = new List<Component>();
        public PingPong GameRoom;

         
        bool isIntialized = false;
        public virtual void Init()
        {
            if (isIntialized)
                return;
            isIntialized = true;

            foreach (var component in Components)
            {
                component.Entity = this;
            }
        }

        /// <summary>
        /// 컴포넌츠 안에 있는 컴포넌트를 가져온다
        /// 컴포넌트가 여러개 있을 때 인덱스 번호가 가장 작은 걸 반환해준다.
        /// </summary>
        /// <typeparam name="T">컴포넌트를 상속받는 클래스</typeparam>
        /// <returns>리스트 안에 컴포넌트가 있으면 컴포넌트를 반환, 없으면 null을 반환해준다.</returns>
        public T GetComponent<T>() where T : Component
        {
            foreach(var component in Components)
            {
                if (component is T)
                    return component as T;
            }
            return null;
        }

        public Entity()
        {
            Init();
        }
        
        public Entity(string name)
        {
            this.name = name;
            Init();
        }
        public Entity(Vector2 position)
        {
            this.Position = position;
            Init();
        }
        public Entity(Vector2 position, string name)
        {
            this.name = name;
            this.Position = position;
            Init();
        }
    }
}
