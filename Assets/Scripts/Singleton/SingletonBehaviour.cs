using UnityEngine;
using UnityEngine.SceneManagement;

namespace CarDriving
{
    ///<summary>
    /// Base class for a singleton MonoBehaviour that gets destroyed if current scene changes.
    /// 
    /// If you want to use OnApplicationQuit method in implementator, use 'new' keyword in method signature
    /// and call 'base.OnApplicationQuit()' in the first line of your method.
    /// 
    /// Be aware this will not prevent a non singleton constructor such as `T myT = new T();`
    /// To prevent that, add `protected T () {}` to your singleton class.
    /// This type inherits from MonoBehaviour so we can use Coroutines.
    /// </summary>
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Original: http://wiki.unity3d.com/index.php/Singleton
        // This is a modified version.

        private static T _instance;

        private static readonly object Lock = new object();

        protected SingletonBehaviour()
        {
        }
        
        /// Returns the singleton instance of <see cref="T"/> for this scene. Please double check you are in the correct scene before calling this.
        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning(string.Format(
                        "[SceneSingleton ({0})] Application is quitting! Returning null instead of '{1}'.",
                        SceneManager.GetActiveScene().name, typeof(T)));
                    return null;
                }

                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T) FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError(string.Format(
                                "[SceneSingleton ({0})] Something went really wrong - there should never be more than 1 singleton! Reopening the scene might fix it.",
                                SceneManager.GetActiveScene().name));
                        }
                        else if (_instance == null)
                        {
                            var container = new GameObject();
                            _instance = container.AddComponent<T>();
                            container.name = string.Format("(scene singleton) {0}", typeof(T));

                            Debug.Log(string.Format(
                                "[SceneSingleton ({0})] An instance of {1} is needed in the scene, so '{2}' was created.",
                                SceneManager.GetActiveScene().name, typeof(T), container));
                        }
                        else
                        {
                            Debug.Log(string.Format("[SceneSingleton ({0})] Using instance already created: {1}",
                                SceneManager.GetActiveScene().name, _instance.gameObject.name));
                        }
                    }

                    return _instance;
                }
            }
        }

        private static bool _applicationIsQuitting = false;
        
        /// When Unity quits, it destroys objects in a random order.
        /// If any script calls Instance after Singleton have been destroyed,
        /// it will create a buggy ghost object that will stay on the Editor scene
        /// even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        protected void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }
    }
}


