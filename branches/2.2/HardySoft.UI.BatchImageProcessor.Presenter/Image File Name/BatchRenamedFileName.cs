﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using HardySoft.UI.BatchImageProcessor.Model;
using HardySoft.CC;

namespace HardySoft.UI.BatchImageProcessor.Presenter {
	/// <summary>
	/// This class is used to generate regaular output file with batch rename enabled.
	/// </summary>
	class BatchRenamedFileName : IFilenameProvider {
		public string GetFileName(string sourceFile, ProjectSetting ps, uint? imageIndex) {
			string fileNameWithoutExtension, extension, newFileName;
			FileInfo fi = new FileInfo(sourceFile);
			string fileName = fi.Name;
			fileNameWithoutExtension = fileName.Remove(fileName.IndexOf(fi.Extension));
			extension = fi.Extension;

			if (! string.IsNullOrEmpty(ps.RenamingSetting.OutputFileNamePrefix)
				|| ! string.IsNullOrEmpty(ps.RenamingSetting.OutputFileNameSuffix)) {
				newFileName = ps.RenamingSetting.OutputFileNamePrefix 
					+ ((imageIndex.HasValue ? imageIndex.Value : 0) + ps.RenamingSetting.StartNumber).ToString("D" + ps.RenamingSetting.NumberPadding) 
					+ ps.RenamingSetting.OutputFileNameSuffix;
			} else {
				newFileName = fileNameWithoutExtension + "_"
					+ ((imageIndex.HasValue ? imageIndex.Value : 0) + ps.RenamingSetting.StartNumber).ToString("D" + ps.RenamingSetting.NumberPadding);
			}
			newFileName = newFileName + extension;
			newFileName = Formatter.FormalizeFolderName(ps.OutputDirectory) + newFileName;

			switch (ps.RenamingSetting.FileNameCase) {
				case OutputFileNameCase.LowerCase:
					newFileName = newFileName.ToLower();
					break;
				case OutputFileNameCase.UpperCase:
					newFileName = newFileName.ToUpper();
					break;
			}
			return newFileName;
		}
	}
}