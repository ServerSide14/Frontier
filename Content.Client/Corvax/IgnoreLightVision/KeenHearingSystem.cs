using Content.Client.Corvax.Overlays;
using Content.Shared.Corvax.IgnoreLightVision;
using Robust.Client.Graphics;

namespace Content.Client.Corvax.IgnoreLightVision;

public sealed class KeenHearingSystem : AddIgnoreLightVisionOverlaySystem<KeenHearingComponent>
{
    protected override Type GetOverlayType()
    {
        return typeof(KeenHearingOverlay);
    }
    protected override Overlay GetOverlayFromConstructor(float radius, float closeRadius)
    {
        return new KeenHearingOverlay(radius, closeRadius);
    }
}
