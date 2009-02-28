﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Data;
using System.Windows.Media.Imaging;

using HardySoft.UI.BatchImageProcessor.Model;

namespace HardySoft.UI.BatchImageProcessor.Classes.Converters {
	public class ImagePathToExifConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			string imagePath = (string)value;
			
			if (File.Exists(imagePath)) {
				FileInfo fi = new FileInfo(imagePath);

				if (string.Compare(fi.Extension, ".jpg", true) == 0
					|| string.Compare(fi.Extension, ".jpeg", true) == 0) {
					// EXIF information is avaialble for JPG file only
					return getExifMetaData(imagePath);
				}
			}
			
			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			throw new NotImplementedException("The method or operation is not implemented.");
		}
		
		/*private Dictionary<string, object> getExifMeta(string imagePath) {
			Uri source = new Uri(imagePath);
			
			ExifMetadata meta = new ExifMetadata(source);
			
			Dictionary<string, object> metaInfo = new Dictionary<string,object>();

			Type t = typeof(ExifMetadata);
			PropertyInfo[] pi = t.GetProperties();

			for (int i = 0; i < pi.Length; i++) {
				object[] attr = pi[i].GetCustomAttributes(true);

				for (int j = 0; j < attr.Length; j++) {
					if (attr[j] is ExifDisplayAttribute) {
						ExifDisplayAttribute exifAttri = (ExifDisplayAttribute)attr[j];

						//string displayName = exifAttri.DisplayName;
						string displayName = Resources.LanguageContent.ResourceManager.GetString(exifAttri.DisplayName,
							Thread.CurrentThread.CurrentCulture);
						object propertyValue = pi[i].GetValue(meta, null);

						metaInfo.Add(displayName, propertyValue);
					}
				}
			}

			return metaInfo;
		}*/

		private DataTable getExifMetaData(string imagePath) {
			DataTable dt = new DataTable();
			dt.Columns.Add(new DataColumn("Attribute", typeof(string)));
			dt.Columns.Add(new DataColumn("Value", typeof(string)));

			Uri source = new Uri(imagePath);

			ExifMetadata meta = new ExifMetadata(source);

			Type t = typeof(ExifMetadata);
			PropertyInfo[] pi = t.GetProperties();

			for (int i = 0; i < pi.Length; i++) {
				object[] attr = pi[i].GetCustomAttributes(true);

				for (int j = 0; j < attr.Length; j++) {
					if (attr[j] is ExifDisplayAttribute) {
						ExifDisplayAttribute exifAttri = (ExifDisplayAttribute)attr[j];

						//string displayName = exifAttri.DisplayName;
						string displayName = Resources.LanguageContent.ResourceManager.GetString(exifAttri.DisplayName,
							Thread.CurrentThread.CurrentCulture);
						object propertyValue = pi[i].GetValue(meta, null);

						//metaInfo.Add(displayName, propertyValue);
						dt.Rows.Add(new object[] {displayName,
							propertyValue == null ? string.Empty : propertyValue.ToString()});
					}
				}
			}

			return dt;
		}
	}
}
