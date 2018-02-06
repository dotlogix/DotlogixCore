using System.Windows;
using System.Windows.Media;

namespace DotLogix.UI.Animations {
    public class RadialGradientBrushKeyFrameCollection : FreezableCollection<KeyFrame<RadialGradientBrush>> {
        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new RadialGradientBrushKeyFrameCollection();
        }
        #endregion
    }
}