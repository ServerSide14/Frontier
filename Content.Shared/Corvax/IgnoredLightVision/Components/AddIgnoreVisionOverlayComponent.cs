using Robust.Shared.Serialization;

namespace Content.Shared.Corvax.IgnoreLightVision;

public abstract partial class AddIgnoreLightVisionOverlayComponent : Component
{
    [DataField]
    public IgnoreLightVisionOverlayState State = IgnoreLightVisionOverlayState.Off;
    [DataField]
    public float VisionRadius = 8f;
    [DataField]
    public float HighSensitiveVisionRadius = 2f;
    [DataField]
    public bool AddAction = false;

    public AddIgnoreLightVisionOverlayComponent(float radius, float closeRadius)
    {
        VisionRadius = radius;
        HighSensitiveVisionRadius = closeRadius;
    }
}

[Serializable, NetSerializable]
public sealed class AddIgnoreLightVisionOverlayState : ComponentState
{
    public IgnoreLightVisionOverlayState State { get; init; }
    public float VisionRadius { get; init; }
    public float HighSensitiveVisionRadius { get; init; }
}

[Serializable, NetSerializable]
public enum IgnoreLightVisionOverlayState
{
    Error,
    Off,
    Half,
    Full
}
