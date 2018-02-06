using System.Windows;

namespace DotLogix.UI.Animations {
    /// <summary>
    ///     Generic animation class for TValue reference type. Uses TAnimationHelper to animate value. For using in XAML
    ///     subclass it with non-generic class.
    /// </summary>
    public class RefTypeAnimation<TValue, TAnimationHelper> :
    Animation<TValue, TValue, TAnimationHelper>
        where TValue : class
        where TAnimationHelper : class, IAnimationHelper<TValue>, new() {
        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new RefTypeAnimation<TValue, TAnimationHelper>();
        }
        #endregion
    }
}