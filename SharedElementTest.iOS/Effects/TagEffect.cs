using System;
using SharedElementTest.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName(EffectBase.RESOLUTION_GROUP_NAME)]

[assembly: ExportEffect(typeof(SharedElementTest.iOS.Effects.TagEffect), TagEffect.EFFECT_NAME)]

namespace SharedElementTest.iOS.Effects
{
    public class TagEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var view = Control as UIView;
            if (view == null)
            {
                return;
            }

            view.Tag = SharedElementTest.Effects.TagEffect.GetTag(Element as View);
        }

        protected override void OnDetached()
        {
        }
    }
}
