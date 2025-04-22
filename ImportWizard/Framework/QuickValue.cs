using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.WinForms.Framework
{
    /// <summary>
    /// A quick way to load and save values.
    /// QuickValue Version 2.0
    /// </summary>
    public static class QuickValue
    {
        private static readonly object _lock = new object();
        private static string _settingsPath = System.IO.Path.Combine(Environment.CurrentDirectory, "QuickValues.xml");

        public static string GetFullName(this System.Windows.Forms.Control control)
        {
            if (control.Parent != null)
                return $"{GetFullName(control.Parent)}_{control.Name}";
            else
                return control.Name;
        }

        /// <summary>
        /// Get a generic type by id.
        /// Currently supports Classes, List<Class> and primitive types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T Get<T>(string id)
        {
            if (typeof(T) == typeof(string))
                return (T)Convert.ChangeType(Get(id), typeof(T));

            if (typeof(T).InheritsFrom(typeof(IEnumerable<>)))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                var data = Get(id);

                if (string.IsNullOrEmpty(data))
                    return default;

                using (var sr = new System.IO.StringReader(data))
                {
                    return (T)serializer.Deserialize(sr);
                }
            }

            return (T)Convert.ChangeType(Get(id), typeof(T));
        }

        /// <summary>
        /// Get a string value by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string Get(string id)
        {
            lock (_lock)
            {
                System.Xml.Linq.XDocument doc = LoadDocument();

                var foundElement = doc.Descendants()
                    .FirstOrDefault(element => (string)element.Attribute("id") == id);

                if (foundElement == null)
                    Set(id, string.Empty);

                return foundElement?.Value;
            }
        }

        public static void Set(string id, object value)
        {
            if (value == null)
                value = string.Empty;

            if (value is string)
            {
                Set(id, value.ToString());
            }
            else if (value.GetType().InheritsFrom(typeof(IEnumerable<>)))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(value.GetType());
                using (var sw = new System.IO.StringWriter())
                {
                    serializer.Serialize(sw, value);

                    Set(id, sw.ToString());
                }
            }
            else
            {
                Set(id, value.ToString());
            }
        }

        /// <summary>
        /// Set a string value to an id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public static void Set(string id, string value)
        {
            lock (_lock)
            {
                System.Xml.Linq.XDocument doc = LoadDocument();

                var foundElement = doc.Descendants()
                    .FirstOrDefault(element => (string)element.Attribute("id") == id);

                if (foundElement != null)
                    foundElement.SetValue(value);
                else
                {
                    var rootElement = doc.FirstNode as System.Xml.Linq.XElement;
                    var setting = new System.Xml.Linq.XElement("Value");
                    setting.SetAttributeValue("id", id);
                    setting.SetValue(value);

                    rootElement.Add(setting);
                }

                doc.Save(_settingsPath);
            }
        }

        private static System.Xml.Linq.XDocument LoadDocument()
        {
            System.Xml.Linq.XDocument doc;

            if (!System.IO.File.Exists(_settingsPath))
            {
                doc = new System.Xml.Linq.XDocument();
                var rootElement = new System.Xml.Linq.XElement("Values");
                doc.Add(rootElement);
                doc.Save(_settingsPath);
            }
            else
                doc = System.Xml.Linq.XDocument.Load(_settingsPath);

            return doc;
        }

        private static bool InheritsFrom(this Type t1, Type t2)
        {
            if (null == t1 || null == t2)
                return false;

            if (null != t1.BaseType &&
                t1.BaseType.IsGenericType &&
                t1.BaseType.GetGenericTypeDefinition() == t2)
            {
                return true;
            }

            if (InheritsFrom(t1.BaseType, t2))
                return true;

            return
                (t2.IsAssignableFrom(t1) && t1 != t2)
                ||
                t1.GetInterfaces().Any(x =>
                  x.IsGenericType &&
                  x.GetGenericTypeDefinition() == t2);
        }

        #region TextBox
        public static void Load(this System.Windows.Forms.TextBox txt)
        {
            var value = Get(GetFullName(txt));

            if (value != null)
                txt.Text = value;
        }

        public static void LoadAndHook(this System.Windows.Forms.TextBox txt)
        {
            txt.Load();
            txt.TextChanged += (s, e) => txt.Save();
        }

        public static void Save(this System.Windows.Forms.TextBox txt)
        {
            if (txt.Text != null)
                Set(GetFullName(txt), txt.Text);
        }
        #endregion

        #region RichTextBox
        public static void Load(this System.Windows.Forms.RichTextBox txt)
        {
            var value = Get(GetFullName(txt));

            if (value != null)
                txt.Text = value;
        }

        public static void Save(this System.Windows.Forms.RichTextBox txt)
        {
            if (txt.Text != null)
                Set(GetFullName(txt), txt.Text);
        }
        #endregion

        #region CheckBox
        public static void Load(this System.Windows.Forms.CheckBox check)
        {
            var value = Get(GetFullName(check));

            if (value != null)
            {
                if (value.ToLower() == "true")
                {
                    check.Checked = true;
                }
                else
                {
                    check.Checked = false;
                }
            }
        }

        public static void Save(this System.Windows.Forms.CheckBox check)
        {
            Set(GetFullName(check), check.Checked.ToString());
        }

        public static void LoadAndHook(this System.Windows.Forms.CheckBox check)
        {
            check.Load();
            check.CheckedChanged += (s, e) => check.Save();
        }
        #endregion

        #region NumericUpDown
        public static void Load(this System.Windows.Forms.NumericUpDown nud)
        {
            var value = Get(GetFullName(nud));

            if (value != null)
                nud.Value = decimal.Parse(value);
        }

        public static void LoadAndHook(this System.Windows.Forms.NumericUpDown nud)
        {
            nud.Load();
            nud.ValueChanged += (s, e) => nud.Save();
        }

        public static void Save(this System.Windows.Forms.NumericUpDown nud)
        {
            Set(GetFullName(nud), nud.Value);
        }
        #endregion
    }
}
