using contenfulsyncapi.Model;
using Xamarin.Forms;

namespace contenfulsyncapi.ViewModel
{
    public class EntryShellDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UpdatedAppearanceTemplate { get; set; }
        public DataTemplate NotUpdatedAppearanceTemplate { get; set; }

        public EntryShellDataTemplateSelector()
        {
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((EntryShell)item).Updated ? UpdatedAppearanceTemplate : NotUpdatedAppearanceTemplate;
        }
    }
}
