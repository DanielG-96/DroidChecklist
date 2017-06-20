using Android.Content;
using Android.Views;
using Android.Util;
using Android.Support.Design.Widget;
using Android.Support.V4.View;

using Java.Interop;

namespace DroidChecklist
{
    [Android.Runtime.Register("com.danielg.DroidChecklist.FABBehavior")]
    public class ScrollAwareFABBehavior : CoordinatorLayout.Behavior
    {
        public ScrollAwareFABBehavior(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public override bool OnStartNestedScroll(CoordinatorLayout coordinatorLayout, Java.Lang.Object child, View directTargetChild, View target, int nestedScrollAxes)
        {
            return nestedScrollAxes == ViewCompat.ScrollAxisVertical || 
                            base.OnStartNestedScroll(coordinatorLayout, child, directTargetChild, target, nestedScrollAxes);
        }

        public override void OnNestedScroll(CoordinatorLayout coordinatorLayout, Java.Lang.Object child, View target, int dxConsumed, int dyConsumed, int dxUnconsumed, int dyUnconsumed)
        {
            base.OnNestedScroll(coordinatorLayout, child, target, dxConsumed, dyConsumed, dxUnconsumed, dyUnconsumed);

            var fab = child.JavaCast<FloatingActionButton>();

            if (dyConsumed > 0 && fab.Visibility == ViewStates.Visible)
                fab.Hide(new FABHideFix());
            else if (dyConsumed < 0 && fab.Visibility != ViewStates.Visible)
                fab.Show();
        }

        /// <summary>
        /// Workaround for a bug where the FloatingActionButton will not show again when scrolling up
        /// </summary>
        class FABHideFix : FloatingActionButton.OnVisibilityChangedListener
        {
            public override void OnHidden(FloatingActionButton fab)
            {
                base.OnHidden(fab);
                fab.Visibility = ViewStates.Invisible;
            }
        }
    }
}