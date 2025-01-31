using Content.Shared.Actions;
using Robust.Shared.GameStates;

namespace Content.Shared.Corvax.IgnoreLightVision;

[RegisterComponent, NetworkedComponent]
public sealed partial class KeenHearingComponent : AddIgnoreLightVisionOverlayComponent
{
    public TimeSpan? ToggleTime;

    public KeenHearingComponent(float radius, float closeRadius) : base(radius, closeRadius) { }
}

[DataDefinition]
public sealed partial class UseKeenHearingEvent : InstantActionEvent
{
    [DataField]
    public float? Duration;
}
