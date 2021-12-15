using Xamarin.Forms;

namespace contenfulsyncapi.ViewModel
{
    public class EntryItemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UpdatedAppearanceTemplate { get; set; }
        public DataTemplate NotUpdatedAppearanceTemplate { get; set; }

        public EntryItemDataTemplateSelector()
        {
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((ResultViewModel)item).Updated ? UpdatedAppearanceTemplate : NotUpdatedAppearanceTemplate;
        }
    }
}
