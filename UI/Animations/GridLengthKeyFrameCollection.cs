using System.Windows;

namespace DotLogix.UI.Animations {
    public class GridLengthKeyFrameCollection : FreezableCollection<KeyFrame<GridLength>> {
        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new GridLengthKeyFrameCollection();
        }
        #endregion
    }
}