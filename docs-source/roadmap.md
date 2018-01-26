### Product Backlog

The **_product backlog_** is a list of **_product backlog items_** (also known as _PBI's_ for short).
The _PBI's_ listed below are **features** or **enhancements** that or may not be added into _Time Savers_.

However, no code has been written for them yet, and they could be removed from the _product backlog_ at any time. 

>There are no product backlog items at the moment.

---

### Contributing

If you'd like to implement one of the the _PBI's_ from the _product backlog_,
feel free to open an [Issue on GitHub][github-issue-pbi] (with the same name that the PBI has here), 
and we can start a discussion there. We can then work toward developing a pull request,
so the implemented feature can be incorporated into *Time Savers*.

Don't forget to check out the [Contribution Guidelines][contribution-guidelines].

[github-issue-pbi]: https://github.com/luminous-software/luminous-code/issues/new?title=Contribute%20to%20PBI%3A%20
[contribution-guidelines]: contributing.md 

---

### Beta Features

Beta features are _PBI's_ that have actually had some code written for them,
and are waiting for that code to be thoroughly tested. 
You can participate in the testing phase by downloading the _CI build_ mentioned below.
You can add it on your own project, and confirm that it does what it's supposed to do.
If you do find a problem, you can create a [Bug Report on GitHub][github-issue-bug-report].

Once they've passed testing they'll be included in the **next public release** of _Time Savers_.

>There are no beta features that need testing at the moment.

[github-issue-bug-report]: https://github.com/luminous-software/luminous-code/issues/new?title=Bug%20Report%3A%20

---

### Bug Fixes

Once bug fixes have been fully tested, they'll be included in the *next public release*.

>There are no bug fixes that need testing at the moment

---

### Continuous Integration

The CI build is the build from the continous integration process. 
The resultant Nuget file is then made available for you to download.

![(VSTS Badge)][vsts-badge-url]
![(Build status)][appveyor-status]

If both build badges above are green, the latest CI build is ready to be downloaded and installed  to test.

[vsts-badge-url]: https://lumiinus.visualstudio.com/_apis/public/build/definitions/c31b2195-e4da-4ad9-a64c-e1712d313703/15/badge
[appveyor-status]: https://ci.appveyor.com/api/projects/status/tsf4rxwtgtcub741?svg=true
[appveyor-url]: https://ci.appveyor.com/project/luminous-software/time-savers