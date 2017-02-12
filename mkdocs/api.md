## Core API

`Luminous.Code.Core` is a plain C# class library project that contains the lower level classes, methods and extension methods that get used by more 
specialised projects.

>Note the lack of `Core` in the namespace names. I decided that there was no benefit
to including it in the namespaces because it sometimes made the namespaces sound a bit odd.

### String Extensions Namespace

The `Luminous.Code.StringExtensions` namespace is fairly self-explanatory.
It contains a set of extension methods that extend the `string` namespace.

---
## Visual Studio API

`Luminous.Code.VisualStudio` is a plain C# class library project that contains the lower level classes, methods and extension methods that pertain to creating
Visual Studio extensions.

It has a project reference to the `Luminous.Code.Core` project, as it relies on some of the code in it.

### Packages Namespace

The `Luminous.Code.VisualStudio.Packages` namespace contains all of the plumbing code that
a Visual Studio package needs to interact with the Visual Studio IDE.

### 'LuminousPackage' Class

The `Luminous.Code.VisualStudio.Packages.LuminousPackage` class is an abstract base class that you inherit your own
packages from.  It abstracts away the `Microsoft.VisualStudio.Shell.Package` and command interact with the Visual Studio IDE.

### Commands Namespace

The `Luminous.Code.VisualStudio.Commands` namespace contains all of the plumbing code that
a command needs to interact with the Visual Studio IDE (via the command's parent package).

### 'LuminousCommand' Class

The `Luminous.Code.VisualStudio.Commands.LuminousCommand` class is an abstract class, which acts as the
common base class for the `StaticCommand` class and the `DynamicCommand` class.

It contains all of the plumbing code that a command needs to interact easily with
its parent package, which has the ability to communicate with the IDE.

### 'StaticCommand' Class

The `Luminous.Code.VisualStudio.Commands.StaticCommand` class is useful for commands whose text doesn't need to change,
and which are always visible and always enabled.

### 'DynamicCommand' Class

The `Luminous.Code.VisualStudio.Commands.DynamicCommand` class can be used for commands whose *text* may need to change, and/or which may
need to dynamically determine if the command needs to be *visible* or *enabled*. 
Three sensibly-named overridable properties are provided to make this easy and flexible.

Both the command's `Visible` property and `Enabled` property are automatically calculated based on the values of
`CanExecute` and `IsActive`.

#### 'CanExecute' Property

If `CanExecute` returns `false`, the command cannot be executed at all.
The command's `Visible` and `Enabled` properties will both be set to `false`.

For example, command classes that inherit from `DynamicCommand` can override this
property to return, say, a package-wide value that can be set in *Tools* | *Options*,
or use some other method of determining if the command's functionality should be turned
off.

#### 'IsActive' Property

If `IsActive` returns `false`, the command's `Enabled` property is set to false.
A common use for this property is to be able to check some complicated *context*  that
can't be set in the package's *VSCT* file.

#### Text Property

The `Text` property, as its name suggests, sets the command's display text. Its initial value is `null`,
which instructs the famework to use the value set in the *VSCT* file.

---
