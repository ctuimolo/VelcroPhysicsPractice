using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace VelcroPhysicsPractice.Scripts
{
    public enum PlayerOrientation
    {
        Right,
        Left
    }

    public class AnimationHandler
    {
        private readonly GameObject _owner;

        private Dictionary<int, Animation> _animationDic { get; }
        private Animation  _currentAnimation;

        private int _drawIndex       = 0;
        private int _animationTimer  = 0;
        private int _state           = 0;

        public PlayerOrientation Facing { get; set; }

        public AnimationHandler( GameObject owner)
        {
            _owner          = owner;
            _animationDic   = new Dictionary<int, Animation>();
        }

        public void AddAnimation(int key, Animation animation)
        {
            _animationDic.Add(key, animation);
        }

        public void ChangeAnimation(int state)
        {
            if(_state != state)
            {
                _state = state;
                _drawIndex = _animationDic[_state].StartIndex;
            }
        }

        public void Set_drawIndex(int set_drawIndex)
        {
            if (set_drawIndex >= _currentAnimation.FrameCount)
                _drawIndex = 0;
            else
                _drawIndex = set_drawIndex;
        }

        public void DrawFrame()
        {
            _currentAnimation = _animationDic[_state];
            Game.SpriteBatch.Draw(
                _currentAnimation.SpriteSheet,
                new Vector2(_owner.Body.BoxCollider.X, _owner.Body.BoxCollider.Y) - _currentAnimation.Offset,
                _currentAnimation.GetDrawRect(_drawIndex),
                Color.White,
                0,
                Vector2.Zero,
                1f,
                Facing == PlayerOrientation.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0f
            );

            if (_currentAnimation.Play)
            {
                _animationTimer++;
                if(_animationTimer >= _currentAnimation.FrameDelay)
                {
                    _animationTimer = 0;
                    _drawIndex++;
                    if (_drawIndex >= _currentAnimation.FrameCount)
                    {
                        if (_currentAnimation.Loop)
                        {
                            _drawIndex = _currentAnimation.LoopIndex;
                        }
                        else
                        {
                            _drawIndex--;
                        }
                    }
                }
            }

            Game.SpriteBatch.DrawString(Debug.Assets.DebugFont, "_drawIndex  : " + _drawIndex, new Vector2(10, 100), Color.Pink);
            _currentAnimation.DrawDebug();
        }
    }
}
