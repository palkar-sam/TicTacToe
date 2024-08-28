namespace Views
{
    public interface IView
    {
        public void SetVisibility(bool isVisible);
        public void OnInitialize();
        public void OnShow();
    }
}