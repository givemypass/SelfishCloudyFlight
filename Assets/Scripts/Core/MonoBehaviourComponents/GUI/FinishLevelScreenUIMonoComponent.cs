using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Components.MonoBehaviourComponents
{
    public class FinishLevelScreenUIMonoComponent : MonoBehaviour
    {
        private const float DELAY = 2f;
        
        public Button Reset;
        public Button Next;

        public Action OnReady;

        private void Awake()
        {
            Next.interactable = false;
            Reset.interactable = false;
            var cancellationToken = gameObject.GetCancellationTokenOnDestroy();
            UniTask.Delay((int)(DELAY * 1000), true, PlayerLoopTiming.Update, cancellationToken).ContinueWith(() =>
            {
                Next.interactable = true;
                Reset.interactable = true;
                OnReady?.Invoke();
            }).Forget();
        }
    }
}