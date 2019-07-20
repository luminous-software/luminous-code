## Public Releases

## v1.4.2 - 2019-07-20

- minor fixes

## v1.4.1 - 2019-06-20

- updated AsyncPackageBase

## v1.4.0 - 2019-04-24

- ported the methods/properties from PackageBase to AsyncPackageBase
- ported the methods/properties from CommandBase to AsyncCommandBase
- ported the methods/properties from DynamicCommand to AsyncDynamicCommand
- fixed bug in `ContextIsActive`
- deprecated PackageBase, CommandBase, StaticCommand, DynamicCommand
- consolidated all extension methods under the `Luminous.Code.Extentions` or `Luminous.Code.VisualStudio.Extensions` namespace

## v1.3.0 - 2019-04-08
- added PackageBase.OpenManageExtensions
- added PackageBase.OpenFileInBrowser
- added PackageBase.OpenTextFile command
- added PackageBase.OpenFile method
- added PackageBase.OpenFolder method

## v1.2.1 - 2019-04-04

- fixed OpenFile/OpenFileInBrowser displaying incorrect problem message
- added AsyncDynamicCommand
- added AsyncCommandBase
- added AsyncPackageBase

## v1.0.3 - 2018-01-26

- Add `ShowNewToolWindow<T>` to `PackageBase`

## v1.0.2 - 2017-11-24

- minor changes

## v1.0.1 - 2017-04-09

- Initial public release
  - Core
    - ```Exception``` Extensions
    - ```String``` Extensions
    - ```String``` Concatenation
  - Visual Studio
    - ```CommandKeys``` class
    - ```CommandStatuses``` enum
    - ```CommandBase``` class
      - ```StaticCommand``` class
      - ```DynamicCommand``` class
    - ```CommandResult``` class
      - ```SuccessResult``` class
      - ```CancelledResult``` class
      - ```InformationResult``` class
      - ```ProblemResult``` class
    - ```ServiceProvider``` Extensions
    - ```IWpfTextViewHost``` Extensions
    - ```PackageBase``` class
    - ```ProjectItems``` Extensions
    - ```Project``` Extensions
    - ```SelectedItems``` Extensions
    - ```SolutionExplorerItemType``` enum
    - ```SolutionItemKind``` class
    - ```IVsHierarchyNodeInfo``` Class

