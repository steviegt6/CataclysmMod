# Custom Content Loading
Cataclysm loads content with a custom loading system that keeps add-ons in mind.

Addons extend the `Addon{TSelf}` class, which contains a circular generic reference to itself (i.e. `ExampleAddon : Addon<ExamleAddon>`). 

Then, addons are registered in the `Cataclysm` class.

Content loading is as simple as defining an autoloadable class decorated with `AddonContentAttribute`.

```cs
[AddonContent(typeof(MyAddon))]
public class MyModItem : ModItem {
}
```