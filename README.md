# VOCALOIDParser
A .NET library for working with VOCALOID5 projects

You can check the documentation >>> [here](https://vocaloidparserdocs.sixbeeps.repl.co/d3/dcc/md__r_e_a_d_m_e.html)

## Motivation

There doesn't exist (to my knowledge) any libraries for working with Vocaloid projects, despite it being one of the largest vocal generation tools out there. As popular as it is, however, the software itself is made to only create and play vocals curated by a person. With a library that can work with its native format, more programatic things can be achieved. Personally, I would love to see a tool for exporting lyrics as subtitles, but that's not possible.

At least, not without VOCALOIDParser.

With this library, anyone with a valid VOCALOID5 project can make their own programs using .vpr files. The possibilities are endless.

## What VOCALOIDParser can and can't do

VOCALOIDParser can read the contents of a .vpr file and provide a .NET-friendly representation of them. The goal is to be able to interpret _every single feature_ of the VOCALOID5 file format, so that it can be completely understood. For your convenience, there are a few helper functions here and there so that you don't need to implement them yourself.

I will not implement exporting projects, though I understand why that sound counterintuitive. Many libraries for working with files allow not just reading files, but also writing to them. Don't get me wrong, I would love to incorporate exporting in this library. However, due to it being very easily abused, I must respect the proprietariness of the format and leave it out.

A list of what needs to be implemented TBD, this project is still a work-in-progress.

## Installation and Usage
Add the package to your project through NuGet See more information about adding VOCALOIDParser to your project on the [nuget.org page](https://www.nuget.org/packages/VOCALOIDParser/1.0.0-alpha). Currently, only .NET 6.0 and above is supported.
Current version: `VOCALOIDParser.1.0.1-alpha`

Example of usage:
```csharp
using System;
using SixBeeps.VOCALOIDParser;

public class Example {
  public static void Main(string[] args) {
    // Load the project
    var project = VocaloidProject.CreateFromVpr("C:/path/to/projects/AwesomeSauce.vpr");

    // Go through each track and output its name
    foreach (var track in project.Tracks) {
      Console.WriteLine(track.Name);
    }
  }
}
```
