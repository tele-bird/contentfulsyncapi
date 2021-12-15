using System;
using System.Globalization;
using contenfulsyncapi.Model;
using Xamarin.Forms;

namespace contenfulsyncapi.Util
{
    public class BoolToContentfulApiConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ContentfulApi)
            {
                return (ContentfulApi)value == 0 ? false : true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return (bool)value ? ContentfulApi.GraphQL : ContentfulApi.SyncAPI;
            }

            return ContentfulApi.SyncAPI;
        }
    }
}
