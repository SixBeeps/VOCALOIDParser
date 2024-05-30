# VOCALOIDParser
A .NET library for working with VOCALOID5 projects

You can check the documentation >>> [here](https://sixbeeps.github.io/VOCALOIDParser/d3/dcc/md__r_e_a_d_m_e.html)

## Motivation

There doesn't exist (to my knowledge) any libraries for working with Vocaloid projects, despite it being one of the largest vocal generation tools out there. As popular as it is, however, the software itself is made to only create and play vocals curated by a person. With a library that can work with its native format, more programatic things can be achieved. Personally, I would love to see a tool for exporting lyrics as subtitles, but that's not possible.

At least, not without VOCALOIDParser.

With this library, anyone with a valid VOCALOID5 project can make their own programs using .vpr files. The possibilities are endless.

## Installation and Usage
Add the package to your project through NuGet See more information about adding VOCALOIDParser to your project on the [nuget.org page](https://www.nuget.org/packages/VOCALOIDParser/latest). It is built using .NET 8.0.
Current version: `VOCALOIDParser.1.1.0`

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
