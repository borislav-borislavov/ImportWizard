using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.WinForms.Extensions
{
    public static class ListBoxExtensions
    {
        public static void DeSelectItemOnClick(this ListBox listBox)
        {
            var lastSelectedIndex = -1;

            listBox.SelectedIndexChanged += (s, e) =>
            {
                if (lastSelectedIndex == listBox.SelectedIndex && lastSelectedIndex != -1)
                {
                    listBox.SelectedIndex = -1;
                }

                lastSelectedIndex = listBox.SelectedIndex;
            };
        }
    }
}
