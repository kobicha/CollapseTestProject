using System;
using DG.Tweening;
using UnityEngine;

namespace Collapse.Blocks {
    /**
     * Bomb specific behavior
     */
    public class Bomb : Block {
        
        private const float BOMB_EXPLOSION_ANIMTION_DELAY_MULTIPLIER = .05f;
        
        [SerializeField]
        private Transform Sprite;

        [SerializeField]
        private Vector3 ShakeStrength;

        [SerializeField]
        private int ShakeVibrato;

        [SerializeField]
        private float ShakeDuration;

        private Vector3 origin;

        private void Awake() {
            origin = Sprite.localPosition;
        }

        protected override void OnMouseUp() {
            base.OnMouseUp();
         
        }
        
        /**
         * Convenience for shake animation with callback in the end
         */
        private void Shake(float delay, Action onComplete = null) {
            Sprite.DOKill();
            Sprite.localPosition = origin;
            var delayAmount = delay * BOMB_EXPLOSION_ANIMTION_DELAY_MULTIPLIER;
            Sprite.DOShakePosition(ShakeDuration, ShakeStrength, ShakeVibrato, fadeOut: false)
                .SetDelay(delayAmount).onComplete += () => {
                onComplete?.Invoke();
            };
        }

        public override void Triger(float delay) {
            if (IsTriggered) return;
            IsTriggered = true;
            BoardManager.Instance.TriggerBomb(this);
            Shake(delay, DestroyBlock);
        }
    }
}