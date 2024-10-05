namespace EFSharpLab.Models.Settings

open System.Reflection

[<CLIMutable>]
type MyCustomSettings =
    { AppId: int option
      AppName: string }

    member this.Version = Assembly.GetEntryAssembly().GetName().Version.ToString()
