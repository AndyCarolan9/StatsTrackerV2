using StatsTrackerV2.Models;

namespace StatsTrackerV2.Pages;

public partial class CreateMatchPage : ContentPage
{
	public CreateMatchPage(CreateMatchPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}

    void OnAvailableDragStarting(object sender, DragStartingEventArgs e)
    {
        var player = ((BindableObject)sender).BindingContext as Player;

        e.Data.Properties["Player"] = player;
        e.Data.Properties["Source"] = "Available";
    }

    void OnStartingDragStarting(object sender, DragStartingEventArgs e)
    {
        var player = ((BindableObject)sender).BindingContext as Player;

        e.Data.Properties["Player"] = player;
        e.Data.Properties["Source"] = "Starting";
    }

    void OnHomeDropToStarting(object sender, DropEventArgs e)
    {
        if (!e.Data.Properties.TryGetValue("Player", out var data) ||
            !e.Data.Properties.TryGetValue("Source", out var source))
            return;

        var player = data as Player;
        var vm = BindingContext as CreateMatchPageModel;

        if (player == null || vm == null)
            return;

        if ((string)source == "Available")
            vm.MoveToHomeStartingTeam(player);
    }

    void OnHomeDropToAvailable(object sender, DropEventArgs e)
    {
        if (!e.Data.Properties.TryGetValue("Player", out var data))
            return;

        var vm = BindingContext as CreateMatchPageModel;
        vm?.MoveToHomeAvailablePlayers((Player)data);
    }

    void OnHomeReorderDrop(object sender, DropEventArgs e)
    {
        if (!e.Data.Properties.TryGetValue("Player", out var data) ||
            !e.Data.Properties.TryGetValue("Source", out var source))
            return;

        if ((string)source != "Starting")
            return;

        var draggedPlayer = data as Player;
        var targetPlayer =
        ((BindableObject)sender).BindingContext as Player;

        var vm = BindingContext as CreateMatchPageModel;

        if (draggedPlayer == null || targetPlayer == null || vm == null)
            return;

        vm.ReorderHomeStartingTeam(draggedPlayer, targetPlayer);
    }

    void OnAwayDropToStarting(object sender, DropEventArgs e)
    {
        if (!e.Data.Properties.TryGetValue("Player", out var data) ||
            !e.Data.Properties.TryGetValue("Source", out var source))
            return;

        var player = data as Player;
        var vm = BindingContext as CreateMatchPageModel;

        if (player == null || vm == null)
            return;

        if ((string)source == "Available")
            vm.MoveToAwayStartingTeam(player);
    }

    void OnAwayDropToAvailable(object sender, DropEventArgs e)
    {
        if (!e.Data.Properties.TryGetValue("Player", out var data))
            return;

        var vm = BindingContext as CreateMatchPageModel;
        vm?.MoveToAwayAvailablePlayers((Player)data);
    }

    void OnAwayReorderDrop(object sender, DropEventArgs e)
    {
        if (!e.Data.Properties.TryGetValue("Player", out var data) ||
            !e.Data.Properties.TryGetValue("Source", out var source))
            return;

        if ((string)source != "Starting")
            return;

        var draggedPlayer = data as Player;
        var targetPlayer =
        ((BindableObject)sender).BindingContext as Player;

        var vm = BindingContext as CreateMatchPageModel;

        if (draggedPlayer == null || targetPlayer == null || vm == null)
            return;

        vm.ReorderAwayStartingTeam(draggedPlayer, targetPlayer);
    }
}