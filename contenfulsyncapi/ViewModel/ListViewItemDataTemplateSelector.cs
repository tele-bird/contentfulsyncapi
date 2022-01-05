using System;
using Xamarin.Forms;

namespace contenfulsyncapi.ViewModel
{
    public class ListViewItemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SelectedAppearanceTemplate { get; set; }
        public DataTemplate DeselectedAppearanceTemplate { get; set; }

        public ListViewItemDataTemplateSelector()
        {
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            DataTemplate result = null;
            if (((ContentTypeViewModel)item).IsSelected)
            {
                result = SelectedAppearanceTemplate;
            }
            else
            {
                result = DeselectedAppearanceTemplate;
            }
            return result;
        }
    }
}
