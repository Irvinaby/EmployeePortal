using System;
using System.Windows;
using System.Windows.Data;

namespace UI.Common
{
    public class IdToVisibleConverter : IValueConverter
    {
        #region Constructors
        /// <summary>
        /// The default constructor
        /// </summary>
        public IdToVisibleConverter() { }
        #endregion

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value == null)
            {
                return Visibility.Hidden;
            }
            if (!string.IsNullOrEmpty(value.ToString()))
                return Visibility.Visible;
            else
                return Visibility.Hidden;
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
