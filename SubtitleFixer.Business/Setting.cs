using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleFixer.Business
{
    public static class Setting
    {
        private static string editedTitle;
        public static string EditedTitle
        {
            get
            {
                LoadSetting();
                return editedTitle;
            }
            set
            {
                editedTitle = value;
                SaveSetting();
            }
        }
        /// <summary>
        /// Convert List<string> Type To LowerCase string 
        /// </summary>
        /// <param name="list">List Of Strings You Want To Convert</param>
        /// <returns>Lower Cased String</returns>
        public static string StringListToString(List<string> list)
        {
            string joinedString = string.Join(" ", list.ToArray());
            return joinedString.ToLower().Trim();
        }

        private static List<string> extensionList = new List<string>();
        public static List<string> ExtensionList
        {
            get
            {
                LoadSetting();
                return extensionList;
            }
            set
            {
                extensionList = value;
                SaveSetting();
            }
        }
        /// <summary>
        /// This Variable Keep Is New Fixed File Replace With Old One Or Not
        /// </summary>
        private static bool isReplaceMode;
        /// <summary>
        /// This Property Get/Set <see cref="isReplaceMode"/> Value
        /// <list type = "bullet|number|table" >
        ///     <listheader>
        ///          <term>Get</term>
        ///     
        ///         <description>
        ///             Load Application Setting By <see cref="LoadSetting"/> Method And 
        ///             Save It In <see cref="isReplaceMode"/> Variable Then Return Variable To Where Property Called
        ///         </description>
        ///     </listheader>
        ///     <item>
        ///          <term>Set</term>
        ///              <description>
        ///                 Set Value In <see cref="isReplaceMode"/> Variable Then
        ///                 Save variable Value In Application Setting variable By <see cref="SaveSetting"/>
        ///              </description>
        ///     </item>
        /// </list>
        /// </summary>
        public static bool IsReplaceMode
        {
            get
            {
                LoadSetting();
                return isReplaceMode;
            }
            set
            {
                isReplaceMode = value;
                SaveSetting();
            }
        }

        /// <summary>
        /// Save <see cref="isReplaceMode"/> Value In Application Setting Variable
        /// </summary>
        private static void SaveSetting()
        {
            Properties.Settings.Default.EditedTitle = editedTitle.Trim();
            Properties.Settings.Default.Extensions = StringListToString(extensionList);
            Properties.Settings.Default.ReplaceSub = isReplaceMode;
            Properties.Settings.Default.Save();
        }
        /// <summary>
        /// Load Setting From Application Setting Variable And Set It In <see cref="isReplaceMode"/> Variable
        /// </summary>
        private static void LoadSetting()
        {
            editedTitle = Properties.Settings.Default.EditedTitle.Trim();
            extensionList = Properties.Settings.Default.Extensions.Split(' ').ToList();
            isReplaceMode = Properties.Settings.Default.ReplaceSub;
        }
    }
}
