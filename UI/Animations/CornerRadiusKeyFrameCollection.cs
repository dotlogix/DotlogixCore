using System.Windows;

namespace DotLogix.UI.Animations {
    public class CornerRadiusKeyFrameCollection : FreezableCollection<KeyFrame<CornerRadius>> {
        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new CornerRadiusKeyFrameCollection();
        }
        #endregion
    }
}