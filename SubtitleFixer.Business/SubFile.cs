using SubtitleFixer.Business;

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SubtitleFixer
{
    public static class SubFile
    {
        private const string MySign =
@"0
00:00:0,0 --> 00:00:5,0
Fixed By Persian Subtitle Fixer
Made By Sepehr Ariamehr
Fallen Star Developers

";
        private static string SubEditedTitle;
        /// <summary>
        /// List Of All Subtitle File Path 
        /// <para>This Subtitle Goes To Be Fixed Then Saved</para>
        /// </summary>
        public static List<string> SubtitleFilePaths { get; set; } = new List<string>();
        /// <summary>
        /// Read Subtitle File  By Windows-1256 Encoding CodePage To Fix Persian Subtitle Display
        /// </summary>
        /// <param name="subtitlePath">Path Of subtitle File</param>
        /// <returns>Fixed Subtitle Text In Windows-1256 CodePage</returns>
        private static string ReadAndFix(string subtitlePath)
        {
            string SubtitleText = string.Empty;
            using (StreamReader reader = new StreamReader(subtitlePath, Encoding.GetEncoding("Windows-1256")))
            {
                SubtitleText =MySign+ reader.ReadToEnd();
            }
            return SubtitleText;
        }
        /// <summary>
        /// Save Subtitle File In <paramref name="SavePath"/> With <paramref name="subtitleText"/> Text Contained
        /// </summary>
        /// <param name="subtitleText">Fixed Text To Saved In Subtitle File</param>
        /// <param name="SavePath">Path And Name Of File You Want To Save It</param>
        /// <param name="isCreatedFile">For New Created File We Must Use Append Value Equal True</param>
        private static void SaveFixed(string subtitleText, string SavePath)
        {
            using (StreamWriter writer = new StreamWriter(SavePath, false, Encoding.UTF8))
            {
                writer.Write(subtitleText);
            }
        }
        /// <summary>
        /// For Every Subtitle In <see cref="SubPathList"/> Fix Encoding By <see cref="ReadAndFix(string)"/> 
        /// <para>Then Save Fixed One With <see cref="SaveFixed(string, string)"/> After It Done Clear <see cref="SubtitleFilePaths"/></para>
        /// </summary>
        public static void FixSubtitlesInList()
        {
            string fileExt, fileName, fileDirectory, tempPath, subText;
            SubEditedTitle = Setting.EditedTitle;
            foreach (string subtitlePath in SubtitleFilePaths)
            {
                fileDirectory = Path.GetDirectoryName(subtitlePath);
                fileName = Path.GetFileNameWithoutExtension(subtitlePath);
                fileExt = Path.GetExtension(subtitlePath);


                if (Setting.IsReplaceMode.Equals(true))
                {
                    SaveFixed(subtitleText: ReadAndFix(subtitlePath), SavePath: subtitlePath);
                }
                else
                {
                    if (subtitlePath.EndsWith("."+SubEditedTitle + fileExt).Equals(false))
                    {
                        tempPath = fileDirectory + "\\" + fileName + "."+SubEditedTitle + fileExt;
                        if (File.Exists(tempPath).Equals(false))
                        {
                            File.Create(tempPath).Close();
                        }

                        subText = ReadAndFix(subtitlePath);
                        SaveFixed(subText, tempPath);
                    }
                    else
                    {
                        SaveFixed(subtitleText: ReadAndFix(subtitlePath), SavePath: subtitlePath);
                    }
                }
            }
            SubtitleFilePaths.Clear();
        }
    }
}
