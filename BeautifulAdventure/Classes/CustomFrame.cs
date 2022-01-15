using System.Windows.Controls;

namespace BeautifulAdventure.Classes
{
    public class CustomFrame
    {
        public Frame Frame;
        public void Navigate(Page page)
        {
            ClearHistory();
            Frame.Navigate(page);
        }

        private void ClearHistory()
        {
            while (Frame.CanGoBack)
                Frame.RemoveBackEntry();
        }
    }
}
