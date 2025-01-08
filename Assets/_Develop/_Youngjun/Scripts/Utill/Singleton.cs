using UnityEngine;
using UnityEngine.Pool;


namespace Noah
{
    public class Singleton <T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;

        public static T Instance
        {
            get 
            {
                return instance;
            }
        }

        public static bool InstanceExist
        {
            get 
            {
                return instance != null;
            }
        }

        protected virtual void Awake()
        {
            if(InstanceExist)
            {
                Destroy(gameObject);
                return;
            }
      
            instance = (T)this;

            
        }

        protected virtual void OnDestroy() 
        {
            if(instance == this)
            {
                instance = null;
            }
        }



        public IObjectPool<GameObject> Pool { get; private set; }

    }
}

