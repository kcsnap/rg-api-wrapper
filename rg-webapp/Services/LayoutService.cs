using RgWebapp.Components.Layout;

namespace RgWebapp.Services
{
    public class LayoutService
    {
        public LayoutService()
        {

        }

        public Type AuthMessage { get; private set; } = typeof(AuthMessage);

        public Dictionary<string, object>? MenuParameters { get; private set; }

        public event EventHandler? AuthMessageChanged;

        public void ChangeAuth()
        { 
            AuthMessageChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
