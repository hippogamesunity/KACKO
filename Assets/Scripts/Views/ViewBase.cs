using Assets.Scripts.Common;

namespace Assets.Scripts.Views
{
    public abstract class ViewBase : Script
    {
        public TweenPanel TweenPanel;

        public static ViewBase Previous;
        public static ViewBase Current;

        public void Open(TweenDirection direction)
        {
            if (Current != null && !ReferenceEquals(Current, this))
            {
                Current.Close(direction == TweenDirection.Right ? TweenDirection.Left : TweenDirection.Right);
            }

            TaskScheduler.Kill(4000);
            Initialize();
            TweenPanel.Hide(direction, 0);
            TweenPanel.Show(direction);
            enabled = true;
            Current = this;
        }

        public void Close(TweenDirection direction)
        {
            Cleanup();
            TweenPanel.Hide(direction);
            TaskScheduler.CreateTask(() => TweenPanel.gameObject.SetActive(false), 4000, TweenPanel.DefaultTimeout);
            enabled = false;
        }

        protected virtual void Initialize()
        {
        }

        protected virtual void Cleanup()
        {
        }
    }
}