using System;
using System.Windows;
using System.Windows.Data;

namespace UI.Common
{
    public class IdToHiddenConverter : IValueConverter
    {
        #region Constructors
        /// <summary>
        /// The default constructor
        /// </summary>
        public IdToHiddenConverter() { }
        #endregion

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;

            if (visibility == Visibility.Visible)
                return string.Empty;
            else
                return "1";
        }
        #endregion
    }
}
