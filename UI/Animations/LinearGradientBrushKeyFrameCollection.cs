using System.Windows;
using System.Windows.Media;

namespace DotLogix.UI.Animations {
    public class LinearGradientBrushKeyFrameCollection : FreezableCollection<KeyFrame<LinearGradientBrush>> {
        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new LinearGradientBrushKeyFrameCollection();
        }
        #endregion
    }
}