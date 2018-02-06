using System.Windows;

namespace DotLogix.UI.Animations {
    public class CornerRadiusAnimationUsingKeyFrames : AnimationUsingKeyFrames<CornerRadius, CornerRadiusAnimationHelper
        , CornerRadiusKeyFrameCollection> {
        #region Freezable
        /// <summary>
        ///     Creates a copy of this CornerRadiusAnimation
        /// </summary>
        /// <returns> The copy </returns>
        public new CornerRadiusAnimationUsingKeyFrames Clone() {
            return (CornerRadiusAnimationUsingKeyFrames)base.Clone();
        }

        /// <summary>
        ///     Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see> .
        /// </summary>
        /// <returns> The new Freezable. </returns>
        protected override Freezable CreateInstanceCore() {
            return new CornerRadiusAnimationUsingKeyFrames();
        }
        #endregion
    }
}