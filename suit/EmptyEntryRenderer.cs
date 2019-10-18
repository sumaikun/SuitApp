using System;
using suit;
using suit.Android;

[assembly: ExportRenderer(typeof(EmptyEntry), typeof(EmptyEntryRenderer))]

namespace suit.Android
{
    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
        base.OnElementChanged(e);

        if (Control != null)
        {
            GradientDrawable gd = new GradientDrawable();
            gd.SetColor(global::Android.Graphics.Color.Transparent);
            this.Control.SetBackgroundDrawable(gd);
            this.Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
            Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.White));
        }
    }
}
  