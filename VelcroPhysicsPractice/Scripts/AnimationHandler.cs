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
        private readonly SpriteFont _font;
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
            _font           = Game.Assets.Load<SpriteFont>("font");
        }

        public void AddAnimation( int key, 
                                  string spriteSheetDirectory,
                                  int frameWidth,
                                  int frameHeight,
                                  Vector2 offset,
                                  int frameCount,
                                  int setFrameDelay,
                                  int startIndex = 0,
                                  int loopIndex = 0 )
        {
            _animationDic.Add(
                key, 
                new Animation( 
                    this, 
                    spriteSheetDirectory,
                    new Rectangle(0,0,frameWidth,frameHeight),
                    frameCount,
                    setFrameDelay, 
                    offset,
                    startIndex,
                    loopIndex
                )
            );
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
                _owner.CollisionHandler.GetDisplayPosition() - _currentAnimation.Offset,
                _currentAnimation.GetDrawRect(_drawIndex),
                Color.White,
                0,
                Vector2.Zero,
                1f,
                Facing == PlayerOrientation.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0f
            );

            _animationTimer++;
            if(_animationTimer >= _currentAnimation.FrameDelay)
            {
                _animationTimer = 0;
                _drawIndex++;
                if (_drawIndex >= _currentAnimation.FrameCount)
                    _drawIndex = _currentAnimation.LoopIndex;
            }


            Game.SpriteBatch.DrawString(_font, "_drawIndex  : " + _drawIndex, new Vector2(10, 100), Color.Pink);
            _currentAnimation.DrawDebug();
        }
    }
}
