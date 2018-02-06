using System.Windows;
using System.Windows.Media;

namespace DotLogix.UI.Animations {
    public class RadialGradientBrushAnimationUsingKeyFrames : AnimationUsingKeyFrames<RadialGradientBrush,
        RadialGradientBrushAnimationHelper, RadialGradientBrushKeyFrameCollection> {
        #region Freezable
        /// <summary>
        ///     Creates a copy of this RadialGradientBrushAnimation
        /// </summary>
        /// <returns> The copy </returns>
        public new RadialGradientBrushAnimationUsingKeyFrames Clone() {
            return (RadialGradientBrushAnimationUsingKeyFrames)base.Clone();
        }

        /// <summary>
        ///     Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see> .
        /// </summary>
        /// <returns> The new Freezable. </returns>
        protected override Freezable CreateInstanceCore() {
            return new RadialGradientBrushAnimationUsingKeyFrames();
        }
        #endregion
    }
}