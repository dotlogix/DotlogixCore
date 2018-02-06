using System.Windows;
using System.Windows.Media.Animation;

namespace DotLogix.UI.Animations {
    /// <summary>
    ///     Animates from Value of the previous key frame to the Value of this key frame using linear interpolation.
    /// </summary>
    public class LinearKeyFrame<TValue, TAnimationHelper> :
    KeyFrame<TValue>
        where TAnimationHelper : class, IAnimationHelper<TValue>, new() {
        protected static readonly IAnimationHelper<TValue> AnimationHelper = SingletonOf<TAnimationHelper>.Instance;

        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new LinearKeyFrame<TValue, TAnimationHelper>();
        }
        #endregion

        #region KeyFrame
        protected override TValue InterpolateValueCore(TValue baseValue, double keyFrameProgress) {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if(keyFrameProgress == 0.0)
                return baseValue;
            if(keyFrameProgress == 1.0)
            // ReSharper restore CompareOfFloatsByEqualityOperator
                return Value;
            return AnimationHelper.InterpolateValue(baseValue, Value, keyFrameProgress);
        }
        #endregion

        #region Constructors
        public LinearKeyFrame() { }
        public LinearKeyFrame(TValue value) : base(value) { }
        public LinearKeyFrame(TValue value, KeyTime keyTime) : base(value, keyTime) { }
        #endregion
    }
}