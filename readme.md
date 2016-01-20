#### Do not use this as an example of how to do a proper RxUI app! This is a quick and dirty hack to give an example of how I've had my ViewModels collect feedback before continuing some process.

#### Description
Lets the user delete a file, but first prompts the user to enter the file's full name, just to make sure they are serious.
The confirmation box is dynamic and appears under the file listing, but this is an implementation detail.
The fairly ugly layout was created with speed in mind, nothing more. It could have easily
been a pop-up modal dialog, or it could have appeared within the file list itself (all fancy-like
as a part of the file item in the listview).

I only implemented the `ConfirmEventView` as a separate `IViewFor<>` for ease of use in this example. In a _real_ application,
`ConfirmEventViewModel<string>` would likely be reused in several places, meaning the design would require
using view contracts, or just doing the layout in-line and keeping the `Visibility` `Collapsed` when necessary.

#### Highlights:
- The views are very simple. Controls bind to Reactive properties, or to ReactiveCommands.
- The logic to delete a file is self-contained within a single lambda within the ViewModel constructor.

#### Lowlights:
- The `ConfirmEventViewModel<T>` type is ... kind of ugly. It's still pretty app-specific, even with that lovely generic there.
- Dealing with the _Typo, try again_ scenario is klunky.

#### Usage:
1. select the fake file you want to "delete"
2. click on the delete button
3. enter the file name by hand to confirm, then click on "do it"
4. you can back out by clicking on any other filename, or the "nope" button
5. if you have a typo, it'll tell you and let you try again
