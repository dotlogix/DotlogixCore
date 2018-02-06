using System.Windows;

namespace DotLogix.UI.Animations {
    public class GridLengthAnimationUsingKeyFrames : AnimationUsingKeyFrames<GridLength, GridLengthAnimationHelper,
        GridLengthKeyFrameCollection> {
        #region Freezable
        /// <summary>
        ///     Creates a copy of this GridLengthAnimation
        /// </summary>
        /// <returns> The copy </returns>
        public new GridLengthAnimationUsingKeyFrames Clone() {
            return (GridLengthAnimationUsingKeyFrames)base.Clone();
        }

        /// <summary>
        ///     Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see> .
        /// </summary>
        /// <returns> The new Freezable. </returns>
        protected override Freezable CreateInstanceCore() {
            return new GridLengthAnimationUsingKeyFrames();
        }
        #endregion
    }
}