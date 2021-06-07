
namespace WinMediaBox.Classes.MediaActions
{
    public class MediaActionBase : ActionBase
    {
        private bool _isActive;
        public bool isActive 
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                if (_isActive)
                {
                    SessionExiting.isAnyMediaActionActive = _isActive;
                    return;
                }
                SessionExiting.isAnyMediaActionActive = _isActive;
            } 
        }
    }
}
