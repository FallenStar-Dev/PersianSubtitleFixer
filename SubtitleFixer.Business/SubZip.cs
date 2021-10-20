using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using SubtitleFixer.Business;

namespace SubtitleFixer
{
    public static class SubZip
    {
        private const string MySign =
@"0
00:00:0,0 --> 00:00:5,0
Fixed By Persian Subtitle Fixer
Made By Sepehr Ariamehr
Fallen Star Developers

";
        /// <summary>
        /// List Of All Dropped Zip File Have Subtitle
        /// </summary>
        public static List<string> ZipSubtitlePaths = new List<string>();
        private static List<string> SupportedFormat;
        private static string SubEditedTitle;
        private static void ZipWork(string subtitlePath)
        {

            using (FileStream zipToOpen = new FileStream(subtitlePath, FileMode.Open, FileAccess.ReadWrite))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {

                    foreach (ZipArchiveEntry archiveEntry in archive.Entries.ToList())
                    {
                        ReadAndFix(archive, archiveEntry);
                    }

                }
            }
        }

        private static void ReadAndFix(ZipArchive archive, ZipArchiveEntry archiveEntry)
        {
            string entryExt, SubtitleText, EditedArchiveName, TempArchiveName;
            bool IsEditedOne, EditedFileExist;
            entryExt = Path.GetExtension(archiveEntry.Name).ToLower();
            if (SupportedFormat.Contains(entryExt))
            {
                SubtitleText = string.Empty;

                using (StreamReader reader = new StreamReader(archiveEntry.Open(), Encoding.GetEncoding("Windows-1256")))
                {
                    SubtitleText =MySign+reader.ReadToEnd();
                }
                if (Setting.IsReplaceMode.Equals(true))
                    using (StreamWriter writer = new StreamWriter(archiveEntry.Open(), Encoding.UTF8))
                    {
                        writer.Write(SubtitleText);
                    }
                else
                {
                    TempArchiveName = Path.GetFileNameWithoutExtension(archiveEntry.Name);
                    EditedArchiveName = TempArchiveName + "." + SubEditedTitle + entryExt;
                    IsEditedOne = archiveEntry.Name.EndsWith("." + SubEditedTitle + entryExt);


                    if (IsEditedOne.Equals(false))
                    {
                        EditedFileExist = archive.Entries.Any(z => z.FullName.Equals(EditedArchiveName));

                        if (EditedFileExist.Equals(false))
                        {
                            archive.CreateEntry(EditedArchiveName);
                            ZipArchiveEntry editedEntry = archive.GetEntry(EditedArchiveName);
                            using (StreamWriter writer = new StreamWriter(editedEntry.Open(), Encoding.UTF8))
                            {
                                writer.Write(SubtitleText);
                            }
                        }
                        else
                        {
                            ZipArchiveEntry editedEntry = archive.GetEntry(EditedArchiveName);
                            using (StreamWriter writer = new StreamWriter(editedEntry.Open(), Encoding.UTF8))
                            {
                                writer.Write(SubtitleText);
                            }
                        }
                    }
                    else
                    {
                        using (StreamWriter writer = new StreamWriter(archiveEntry.Open(), Encoding.UTF8))
                        {
                            writer.Write(SubtitleText);
                        }
                    }
                }
            }
        }

        public static void FixSubtitles(List<string> importedSupporetedFormat)
        {
            SubEditedTitle = Setting.EditedTitle;
            SupportedFormat = importedSupporetedFormat;
            foreach (string zipItem in ZipSubtitlePaths)
            {
                ZipWork(zipItem);
            }
        }

    }
}
