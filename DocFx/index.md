# Introduction

A lightweight front end framework to design scenes and draw them to a generic canvas in C#/.NET. 

## Description

StudioLaValse.Drawable allows for the simple definition of scenes that can be reused and re-rendered in different environments. 
It provides a strict separation of models and views without necessarily adhering to the MVVM pattern. 
The library uses a performant internal visual tree that supports partial re-rendering and offers native support for WPF, Avalonia, WinForms, PDF, Skia, and SVG. 
Additionally, it allows for dynamic scenes that users can interact with using mouse and keyboard inputs.

## Getting Started

### Dependencies

* StudioLaValse.Geometry (https://github.com/Studio-La-Valse/Geometry)

### Installing

You can install StudioLaValse.Drawable using the NuGet feed or by cloning/forking the repository:

#### Using Nuget
To install StudioLaValse.Drawable via NuGet, run the following command in your package manager console:

```sh
Install-Package StudioLaValse.Drawable
```

or use the Nuget Package Manager.

#### Cloning or Forking
Alternatively, you can clone or fork the repository from GitHub:

1. Clone the repository:
```sh
git clone https://github.com/yourusername/StudioLaValse.Drawable.git
```
2. Navigate to the project directory:
```sh
cd StudioLaValse.Drawable
```
3. Install dependencies: 
```sh
dotnet restore
```

## Why not just use MVVM?
While the MVVM (Model-View-ViewModel) pattern is a well-established approach for building user interfaces, it may not be the ideal solution for every scenario, especially when it comes to complex, dynamic visualizations. 
Consider for example the effort you have to go through just to connect two ellipses with a line from center to center (try it!), or exporting your components to PDF or SVG for a static webview. 

- Separation of Models and Views:

StudioLaValse.Drawable still allows for a strict separation of models and views without strictly adhering to the MVVM pattern. 
This flexibility enables you to create and manage visual representations of models in a way that best suits your application's requirements.

- Versatile Rendering:

StudioLaValse.Drawable supports multiple rendering targets, including WPF, Avalonia, WinForms, PDF, Skia, and SVG. 
This versatility allows your scenes to be rendered to different environments without being tied to a specific UI framework.

- Dynamic and Interactive Scenes:

The library is designed to handle dynamic scenes where users can interact with the visual elements using mouse and keyboard inputs. 
Ofcourse interactivity is supported for MVVM, but most of the time resorting to code behind for custom components is the way to go and at that point you are fully locked into the UI framework - which I personally don't like.

If the above points do not resonate with you, that's fine and it's probaly best to stick with a strict MVVM pattern, especially if you're working with  WPF or Avalonia.

## Help

Please get in touch if you need help or have any questions. You can also refer to the documentation: https://drawable.lavalse.net/

## Authors

[@Roel Westrik](https://github.com/roelwestrik)

## License

This project is licensed under the GNU GENERAL PUBLIC LICENSE - see the LICENSE.md file for details
