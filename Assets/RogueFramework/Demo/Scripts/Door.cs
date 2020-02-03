using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueFramework.Demo
{
    public class Door : Interactable
    {
        [SerializeField] Sprite openSprite = default;
        [SerializeField] new SpriteRenderer renderer = null;

        public override void Interact(Actor actor)
        {
            renderer.sprite = openSprite;

            Entity.BlocksVision = false;
            Entity.BlocksMovement = false;

            enabled = false;
        }
    }
}
