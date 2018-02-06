using System.Windows;
using System.Windows.Media;

namespace DotLogix.UI.Animations {
    public class LinearGradientBrushAnimationUsingKeyFrames : AnimationUsingKeyFrames<LinearGradientBrush,
        LinearGradientBrushAnimationHelper, LinearGradientBrushKeyFrameCollection> {
        #region Freezable
        /// <summary>
        ///     Creates a copy of this LinearGradientBrushAnimation
        /// </summary>
        /// <returns> The copy </returns>
        public new LinearGradientBrushAnimationUsingKeyFrames Clone() {
            return (LinearGradientBrushAnimationUsingKeyFrames)base.Clone();
        }

        /// <summary>
        ///     Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see> .
        /// </summary>
        /// <returns> The new Freezable. </returns>
        protected override Freezable CreateInstanceCore() {
            return new LinearGradientBrushAnimationUsingKeyFrames();
        }
        #endregion
    }
}