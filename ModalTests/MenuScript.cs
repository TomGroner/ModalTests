using Stride.Core;
using Stride.Engine;
using Stride.Engine.Events;
using Stride.UI;
using Stride.UI.Controls;
using Stride.UI.Panels;

namespace ModalTests
{
  public class MenuScript : SyncScript
  {
    bool menuOpen;
    EventReceiver<Raycaster.ClickResult> playerClick;

    [DataMember]
    public UIComponent UI { get; set; }

    [DataMember]
    public PlayerInputScript PlayerInput { get; set; }

    [DataMember]
    public UILibrary ControlsLibrary { get; set; }

    public override void Start()
    {
      var menuControl = ControlsLibrary.InstantiateElement<Grid>("FantasyGrid");
      var modal = new ModalElement { Content = menuControl };

      UI.Page = new UIPage { RootElement = modal };
      modal.OutsideClick += (sender, e) => Hide();
      playerClick = new EventReceiver<Raycaster.ClickResult>(PlayerInput.OnClickable);      
      Hide();
    }

    public override void Update()
    {
      if (menuOpen) return;

      if (playerClick.TryReceive(out Raycaster.ClickResult data))
      {
        if (!data.IsEmpty)
        {
          Show();
        }
        else
        { 
          // in my game I care here, in this sample I don't :)
        }
      } 
    }

    protected void Show()
    {
      PlayerInput.SetMenuOpen(menuOpen = true);
      UI.Page.RootElement.Visibility = Visibility.Visible;      
    }

    protected void Hide()
    {
      PlayerInput.SetMenuOpen(menuOpen = false);
      UI.Page.RootElement.Visibility = Visibility.Hidden;      
    }
  }
}