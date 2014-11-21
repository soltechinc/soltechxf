<div><h1><a href="http://www.soltech.net"><img alt="SolTech" src="http://www.soltech.net/images/logo.png"></a>
SolTech Xamarin Forms Toolkit</h1></div>
<p>From http://www.soltech.net</p>
=========

The SolTech Xamarin Forms Toolkit is a set of helpful extensions to the Xamarin Forms framework. The binaries are available via NuGet (package name <a href="https://www.nuget.org/packages/SolTech.Xamarin.Forms/">SolTech.Xamarin.Forms</a>) with the source available at <a href="https://github.com/soltechinc/soltechxf">this GitHub repository</a>.

## Controls
### BoundPicker
A subclass of the Xamarin Forms Picker control that supports databinding.

#### Properties
##### ItemsSource
A one-way bound property that allows you to expose an IEnumerable as the source for the list.

##### SelectedItem
A two-way bound property that allows you to set/get the currently selected item.

### Toggle Selector
A hyper-simplified A/B picker that uses native controls to pick an either/or selector. This is different from the Xamarin Forms Switch control in that the developer can control the Left/Right text and it is rendered using native "selector" controls:

Platform | Native Control
------------- | -------------
iOS | UISegmentedControl
Android | Radio Button
Windows Phone | Radio Button

#### Properties
##### LeftText
A one-way bound property that allows you to set the text to display for the Left-hand side.

##### RightText
A one-way bound property that allows you to set the text to display for the Right-hand side.

##### SelectedItem
A two-way bound property that allows you to get/set the currently-selected item.

### ImageButton
A subclass of the Xamarin Forms Image control that supports a Click event.

#### Properties
##### Command
A one-way bound property that allows you to connect an ICommand to the Click event.

##### CommandParameter
A one-way bound property that allows you to supply a parameter to the ICommand.

### LimitedEntry
A subclass of the Xamarin Forms Entry control that allows the developer to place an upper limit on the amount of text that can be entered.

#### Properties
##### MaxLength
A one-way bound property that allows you to set the maximum number of characters the native entry control will allow.

### StatefulButton
A somewhat confusingly-named button that allows you to specify the background color to use when in the Enabled/Disabled state.

#### Properties
##### EnabledColor
A one-way bound property that allows you to set the background color to use then the button's IsEnabled property is set to true.

##### EnabledColor
A one-way bound property that allows you to set the background color to use then the button's IsEnabled property is set to false.

## Helper Classes
### PickerItem<T>
The Picker (and therefore, the BoundPicker) default to using the ToString() of the items supplied to it. When using the Picker control, this requires a developer to maintain a lookup table of string/position -> item. The PickerItem<T> class is a helper that can be used to wrap non-string items (e.g., enumerations, value types, complex objects) so that the picker can display a user-friendly string while associating the currently selected Picker item to a semantically-useful item.

#### Properties
##### Name
The String to display for this item as the picker entry.

##### Item
The strongly-typed item to associate with the entry.

#### Methods

##### GetPickerItemFromEnum
A helper method that can create a PickerItem<T> for an enumeration value. When combined with SolTech.Forms.DescriptionAttribute, the resulting PickerItem<T> can use a human-readable name; in addition, with the use of ILocalizer those names can be localized.

##### GetPickerItemListFromEnum
A helper method that can create alist of PickerItem<T> for an enumeration. When combined with SolTech.Forms.DescriptionAttribute, the resulting list of PickerItem<T> can use human-readable names; in addition, with the use of ILocalizer those names can be localized.

### DescriptionAttribute
An attribute that allows a developer to decorate enumerations with localized names for use in a BoundPicker. Intended for use with the BoundPicker's GetPickerItemFromEnum/GetPickerItemListFromEnum methods and the ILocalizer service.

#### Properties
##### Description
A String that describes this item for the localization team.

##### Resource ID
A resource identifier to use when performing lookups.

### ILocalizer
An interface that establishes a contract for localizing strings.

#### Methods
##### GetText
Gets the text for the supplied item using the supplied resource namespace, resource name, and key; the resource namespace and name are concatenated to load the list of resources from a RESX file, with the key being used to extract the named string from the RESX file.

### EmbeddedResourceLocalizer
An implementation of ILocalizer that uses embedded resource files (.RESX) to perform localization.
