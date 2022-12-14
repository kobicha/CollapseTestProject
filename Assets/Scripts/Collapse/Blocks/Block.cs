using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Collapse.Blocks {
    
    /**
     * Block behavior - default handling of inputs, triggers and animations
     */
    public abstract class Block : MonoBehaviour {
        
        private const float INTRO_ANIMTION_DELAY = .3f;
        private const float TRIGGER_ANIMTION_DELAY_MULTIPLIER = .05f;
        private const float INTRO_ANIMATION_DURATION = .2f;
        private const float INTERACTION_ANIMATION_DURATION = .1f;
        private const float TRIGGER_ANIMATION_DURATION = .2f;
        private const float INTERACTION_SCALE_FACTOR = 1.2f;

        // Public props used by BoardManager
        public BlockType Type;
        public Vector2Int GridPosition;

        protected bool IsTriggered;
        public bool IsAnimating => DOTween.IsTweening(transform, true);
        
        /**
         * Start
         */
        private void Start() {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, INTRO_ANIMATION_DURATION).SetDelay(Random.value * INTRO_ANIMTION_DELAY);
        }

        /**
         * OnMouseEnter
         */
        private void OnMouseEnter() {
            if (IsTriggered) return;
            transform.DOKill();
            transform.DOScale(Vector3.one * INTERACTION_SCALE_FACTOR, INTERACTION_ANIMATION_DURATION).SetEase(Ease.OutQuad);
        }

        /**
         * OnMouseExit
         */
        private void OnMouseExit() {
            if (IsTriggered) return;
            transform.DOKill();
            transform.DOScale(Vector3.one, INTERACTION_ANIMATION_DURATION).SetEase(Ease.OutQuad);
        }

        /**
         * OnMouseUp
         */
        protected virtual void OnMouseUp() {
            if (IsTriggered) return;
            BoardManager.Instance.TriggerMatch(this);
        }

        /**
         * Trigger the block
         */
        public virtual void Triger(float delay) {
            if (IsTriggered) return;
            IsTriggered = true;
            var delayAmount = CalculateTriggerDelay(delay);
            transform.DOPunchScale(Vector3.one, TRIGGER_ANIMATION_DURATION).SetEase(Ease.InElastic).SetDelay(delayAmount).OnComplete(DestroyBlock);
            
            BoardManager.Instance.ClearBlockFromGrid(this);
        }

        private float CalculateTriggerDelay(float delay)
        {
            var delayAmount = delay * TRIGGER_ANIMTION_DELAY_MULTIPLIER;
            return delayAmount;
        }

        protected virtual void DestroyBlock()
        {
            transform.DOKill();
            Destroy(gameObject);
        }
    }
}