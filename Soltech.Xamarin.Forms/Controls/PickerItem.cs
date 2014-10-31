using System;
using System.Collections.Generic;
using System.Reflection;

namespace SolTech.Forms
{
    public class PickerItem<T>
    {
        public String Name { get; private set; }
        public T Item { get; private set; }

        public PickerItem(String name, T item)
        {
            if (item == null) throw new ArgumentNullException("item");
            Name = name;
            Item = item;
        }

        public PickerItem(T item, ILocalizer localizer, String resourceNamespace)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (localizer == null) throw new ArgumentNullException("localizer");
            if (String.IsNullOrEmpty(resourceNamespace)) throw new ArgumentNullException("resourceNamespace");

            Type enumType = typeof(T);
            if (!enumType.GetTypeInfo().IsEnum) throw new ArgumentException("You must provide an enumeration.", "enumeration");

            var entry = enumType.GetRuntimeField(Enum.GetName(enumType, item));
            var description = entry.GetCustomAttribute<DescriptionAttribute>();
            Name = localizer.GetText(resourceNamespace, enumType.Name, description.ResourceId);
            Item = item;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            PickerItem<T> other = obj as PickerItem<T>;
            if (other == null) return false;

            return 
                String.Compare(Name, other.Name, StringComparison.CurrentCulture) == 0 && 
                Item.Equals(other.Item);
        }

        public override int GetHashCode()
        {
            return Item.GetHashCode();
        }

        public static IEnumerable<PickerItem<T>> GetPickerItemList(IEnumerable<T> items)
        {
            var list = new List<PickerItem<T>>();
            if (items != null)
            {
                foreach (var item in items)
                {
                    var pickerEntry = new PickerItem<T> (item.ToString (), item);
                    list.Add (pickerEntry);
                }
            }

            return list;
        }

        public static PickerItem<T> GetPickerItemFromEnum(T item, ILocalizer localizer, String resourceNamespace)
        {
            if (localizer == null) throw new ArgumentNullException("localizer");
            if (String.IsNullOrEmpty(resourceNamespace)) throw new ArgumentNullException("resourceNamespace");

            Type enumType = typeof(T);
            if (!enumType.GetTypeInfo().IsEnum) throw new ArgumentException("You must provide an enumeration.", "enumeration");

            if (!Enum.IsDefined (enumType, item))
            {
                throw new ArgumentOutOfRangeException("item");
            }

            var entry = enumType.GetRuntimeField(Enum.GetName(enumType, item));
            var description = entry.GetCustomAttribute<DescriptionAttribute>();
            var pickerEntry = new PickerItem<T>(localizer.GetText(resourceNamespace, enumType.Name, description.ResourceId), (T)entry.GetValue(null));

            return pickerEntry;
        }

        public static PickerItem<T> GetPickerItemFromEnum(T item)
        {
            Type enumType = typeof(T);
            if (!enumType.GetTypeInfo().IsEnum) throw new ArgumentException("You must provide an enumeration.", "enumeration");

            if (!Enum.IsDefined(enumType, item))
            {
                throw new ArgumentOutOfRangeException("item");
            }

            var entry = enumType.GetRuntimeField(Enum.GetName(enumType, item));
            var description = entry.GetCustomAttribute<DescriptionAttribute>();
            PickerItem<T> pickerEntry;
            if (description != default(DescriptionAttribute))
            {
                pickerEntry = new PickerItem<T>(description.Description, (T)entry.GetValue(null));
            }
            else
            {
                pickerEntry = new PickerItem<T>(enumType.Name, (T)entry.GetValue(null));
            }
            

            return pickerEntry;
        }

        public static IList<PickerItem<T>> GetPickerItemListFromEnum(ILocalizer localizer, String resourceNamespace)
        {
            if (localizer == null) throw new ArgumentNullException("localizer");
            if (String.IsNullOrEmpty(resourceNamespace)) throw new ArgumentNullException("resourceNamespace");

            Type enumType = typeof(T);
            if (!enumType.GetTypeInfo().IsEnum) throw new ArgumentException("You must provide an enumeration.", "enumeration");

            var list = new List<PickerItem<T>>();
            foreach (var entryName in Enum.GetNames(enumType))
            {
                var entry = enumType.GetRuntimeField(entryName);
                var description = entry.GetCustomAttribute<DescriptionAttribute>();
                var pickerEntry = new PickerItem<T>(localizer.GetText(resourceNamespace, enumType.Name, description.ResourceId), (T)entry.GetValue(null));
                list.Add(pickerEntry);
            }

            return list;
        }

        public static IList<PickerItem<T>> GetPickerItemListFromEnum()
        {
            Type enumType = typeof(T);
            if (!enumType.GetTypeInfo().IsEnum) throw new ArgumentException("You must provide an enumeration.", "enumeration");

            var list = new List<PickerItem<T>>();
            foreach (var entryName in Enum.GetNames(enumType))
            {
                var entry = enumType.GetRuntimeField(entryName);
                var description = entry.GetCustomAttribute<DescriptionAttribute>();
                PickerItem<T> pickerEntry;
                if (description != default(DescriptionAttribute))
                {
                    pickerEntry = new PickerItem<T>(description.Description, (T)entry.GetValue(null));
                }
                else
                {
                    pickerEntry = new PickerItem<T>(enumType.Name, (T)entry.GetValue(null));
                }
                list.Add(pickerEntry);
            }

            return list;
        }
    }
}
