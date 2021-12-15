using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarDriving
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private List<LevelBehaviour> levels;
    
        public LevelBehaviour CurrentLevel { get; private set; }

        public void Load(int index)
        {
            if (levels.Count < 1)
            {
                Debug.LogError("No levels specified");
                return;
            }

            StartCoroutine(LoadRoutine(index % levels.Count));
        }

        private IEnumerator LoadRoutine(int modIndex)
        {
            if(CurrentLevel)
                DestroyImmediate(CurrentLevel.gameObject);

            yield return new WaitForEndOfFrame();

            CurrentLevel = Instantiate(levels[modIndex]);

            yield return new WaitForEndOfFrame();
        
            GameController.Instance.OnLevelLoaded(CurrentLevel);
        }
    }
}

