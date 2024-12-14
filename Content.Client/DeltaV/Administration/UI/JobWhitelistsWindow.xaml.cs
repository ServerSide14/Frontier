using Content.Client.UserInterface.Controls;
using Content.Shared.Database;
using Content.Shared.DeltaV.Administration;
using Content.Shared.Roles;
using Content.Shared.DeltaV.Whitelist;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Prototypes;
using Content.Shared.Ghost.Roles;

namespace Content.Client.DeltaV.Administration.UI;

/// <summary>
/// An admin panel to toggle whitelists for individual jobs or entire departments.
/// This should generally be preferred to a blanket whitelist (Whitelisted: True) since
/// being good with a batong doesn't mean you know engineering and vice versa.
/// </summary>
[GenerateTypedNameReferences]
public sealed partial class JobWhitelistsWindow : FancyWindow
{
    [Dependency] private readonly IPrototypeManager _proto = default!;

    public Action<ProtoId<JobPrototype>, bool>? OnSetJob;
    public Action<ProtoId<GhostRolePrototype>, bool>? OnSetGhostRole; // Frontier
    public Action<bool>? OnSetGlobal; // Frontier

    public JobWhitelistsWindow()
    {
        RobustXamlLoader.Load(this);
        IoCManager.InjectDependencies(this);

        PlayerName.Text = "???";
        InitializeTierButtons();

        // Frontier: global whitelist button
        Global.Text = Loc.GetString("player-panel-global-whitelist");
        Global.OnPressed += _ => OnSetGlobal?.Invoke(Global.Pressed);
        Global.Modulate = Color.FromHex("#f0c65d");
        // End Frontier
    }

    private void InitializeTierButtons()
    {
        foreach (var tier in _proto.EnumeratePrototypes<WhitelistTierPrototype>())
        {
            var button = new Button
            {
                Text = Loc.GetString(tier.Name),
                Modulate = tier.Color
            };

            button.OnPressed += _ => ApplyWhitelistTier(tier);
            TierButtons.AddChild(button);
        }
    }

    private void ApplyWhitelistTier(WhitelistTierPrototype tier)
    {
        foreach (var jobId in tier.Jobs)
        {
            OnSetJob?.Invoke(jobId, true);
        }

        // Frontier: ghost role prototypes
        foreach (var ghostRoleId in tier.GhostRoles)
        {
            OnSetGhostRole?.Invoke(ghostRoleId, true);
        }
        // End Frontier
    }

    public void HandleState(JobWhitelistsEuiState state)
    {
        PlayerName.Text = state.PlayerName;

        // Frontier: global whitelist
        Global.Pressed = state.GlobalWhitelist;
        // End Frontier

        Departments.RemoveAllChildren();
        foreach (var proto in _proto.EnumeratePrototypes<DepartmentPrototype>())
        {
            // Frontier: skip empty departments
            if (proto.Roles.Count <= 0)
                continue;
            // End Frontier

            var panel = new DepartmentWhitelistPanel(proto, _proto, state.Whitelists, state.GlobalWhitelist);
            panel.OnSetJob += (id, whitelisting) => OnSetJob?.Invoke(id, whitelisting);
            Departments.AddChild(panel);
        }
        // Frontier: ghost role prototypes
        GhostRoles.RemoveAllChildren();
        List<ProtoId<GhostRolePrototype>> ghostRoles = new();
        foreach (var proto in _proto.EnumeratePrototypes<GhostRolePrototype>())
        {
            if (proto.Whitelisted)
                ghostRoles.Add(proto.ID);
        }
        var ghostRolePanel = new GhostRoleSetWhitelistPanel(ghostRoles, Loc.GetString("player-panel-ghost-role-whitelists"), Color.FromHex("#71f0ca"), _proto, state.GhostRoleWhitelists, state.GlobalWhitelist);
        ghostRolePanel.OnSetGhostRole += (id, whitelisting) => OnSetGhostRole?.Invoke(id, whitelisting);
        GhostRoles.AddChild(ghostRolePanel);
        // End Frontier
    }
}
