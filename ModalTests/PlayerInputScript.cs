using Stride.Core;
using Stride.Engine;
using Stride.Engine.Events;
using Stride.Physics;

namespace ModalTests
{
  public class PlayerInputScript : SyncScript
  {
    bool menuOpen;

    [DataMember]
    public CameraComponent Camera { get; set; }

    [DataMemberIgnore]
    public EventKey<Raycaster.ClickResult> OnClickable = new EventKey<Raycaster.ClickResult>("Input", "PlayerInput");

    public override void Update()
    {
      if (!menuOpen && Input.HasMouse && Input.IsMouseButtonReleased(Stride.Input.MouseButton.Left))
      {
        if (Raycaster.CheckForHits(Input.MousePosition, Camera, this.GetSimulation(), out var result))
        {
          OnClickable.Broadcast(result);
        }
        else
        {
          OnClickable.Broadcast(Raycaster.ClickResult.Empty);
        }
      }
    }

    public void SetMenuOpen(bool menuOpen)
    {
      this.menuOpen = menuOpen;
    }
  }
}