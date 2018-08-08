using System;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using SharedElementTest.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(NavigationPage), typeof(TransitionNavigationRenderer))]

namespace SharedElementTest.iOS.Renderers
{
    public class TransitionNavigationRenderer : NavigationRenderer, IUINavigationControllerDelegate
    {
        public TransitionNavigationRenderer() : base()
        {
            Delegate = this;
        }

        [Export("navigationController:animationControllerForOperation:fromViewController:toViewController:")]
        public IUIViewControllerAnimatedTransitioning GetAnimationControllerForOperation(UINavigationController navigationController, UINavigationControllerOperation operation, UIViewController fromViewController, UIViewController toViewController)
        {
            var fromView = fromViewController.View.ViewWithTag(10);
            if (fromView == null)
            {
                return null;
            }
            var toView = toViewController.View.ViewWithTag(10);
            if (toView == null)
            {
                return null;
            }
            return new AnimatedPageRendererAnimatedTransitioning(fromView, toView, operation);
        }
    }

    public class AnimatedPageRendererAnimatedTransitioning : UIViewControllerAnimatedTransitioning
    {
        public AnimatedPageRendererAnimatedTransitioning(UIView fromView, UIView toView, UINavigationControllerOperation operation)
        {
            _fromView = fromView;
            _toView = toView;
            _operation = operation;
        }

        private UIView _fromView;
        private UIView _toView;
        private UINavigationControllerOperation _operation;

        public override async void AnimateTransition(IUIViewControllerContextTransitioning transitionContext)
        {
            var containerView = transitionContext.ContainerView;

            var fromViewController = transitionContext.GetViewControllerForKey(UITransitionContext.FromViewControllerKey);
            var toViewController = transitionContext.GetViewControllerForKey(UITransitionContext.ToViewControllerKey);

            var fromViewSnapshot = _fromView.SnapshotView(true);

            // This needs to be added to the hierarchy for the frame is correct, but we don't want it visible yet.
            containerView.InsertSubview(toViewController.View, 0);
            containerView.AddSubview(fromViewSnapshot);

            // Without this, the snapshots include the following "recent" changes
            await Task.Yield();
            _toView.Alpha = 0f;
            _fromView.Alpha = 0f;

            var fromFrame = _fromView.ConvertRectToView(_fromView.Frame, containerView);
            var toFrame = _toView.ConvertRectToView(_toView.Frame, containerView);


            containerView.InsertSubview(toViewController.View, 1);
            var toViewInitialX = _operation == UINavigationControllerOperation.Pop ? -toViewController.View.Frame.Width : toViewController.View.Frame.Width;
            toViewController.View.Frame = new CGRect(toViewInitialX, fromViewController.View.Frame.Y, toViewController.View.Frame.Width, toViewController.View.Frame.Height);

            fromViewSnapshot.Frame = fromFrame;

            UIView.AnimateNotify(0.250, () =>
            {
                fromViewSnapshot.Frame = toFrame;
                fromViewSnapshot.Alpha = 1;
                toViewController.View.Frame = new CGRect(fromViewController.View.Frame.X, fromViewController.View.Frame.Y, toViewController.View.Frame.Width, toViewController.View.Frame.Height);
            }, x =>
            {
                _toView.Alpha = 1;
                _fromView.Alpha = 1;
                fromViewSnapshot.RemoveFromSuperview();
                transitionContext.CompleteTransition(x);
            });
        }

        public override double TransitionDuration(IUIViewControllerContextTransitioning transitionContext) => 0.250;
    }
}
