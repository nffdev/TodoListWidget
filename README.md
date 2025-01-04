# To-Do List Widget
A simple and lightweight to-do list widget for Windows. It allows users to add tasks, mark them as completed, and hide the widget in the system tray.

## Features
- Add tasks to the to-do list with a simple input box.
- Mark tasks as completed using checkboxes (strikes through completed tasks).
- Close the widget to the system tray without exiting the application.
- Drag and move the widget freely across the screen.

## Requirements
- Visual Studio 2019 or higher
- .NET Framework 4.7 or higher

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/nffdev/todolist-widget.git
   ```
2. Open the project in Visual Studio.
3. Compile the project using the Release or Debug configuration.

## Use 

1. Compile and run the application.
2. The to-do list widget will appear as a small window.
3. To add a task, type in the input box and either press the Enter key or click the "Add" button.
4. You can hide the widget by clicking the "Hide" option in the system tray.
5. To remove a task, simply click on the task.

## TECHNICAL DETAILS

- **NotifyIcon**: Provides the system tray icon functionality, enabling users to hide and restore the widget.
- **FlowLayoutPanel**: Used to arrange and display tasks dynamically in the widget.
- **CheckBox**: Used to mark tasks as completed. When checked, the task's font becomes struck through.
- **GraphicsPath**: Used to draw rounded corners for the widget window.
- **SetParent**: Makes the widget window a child of the desktop, allowing it to always appear on top without interfering with other open applications.
- **Drag-and-drop**: Allows users to move the widget freely across the screen by dragging it.

## Resources

- [Official Microsoft documentation on Windows Forms](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/?view=netdesktop-9.0)
- [NotifyIcon Class Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.notifyicon?view=windowsdesktop-9.0)

## Demo
