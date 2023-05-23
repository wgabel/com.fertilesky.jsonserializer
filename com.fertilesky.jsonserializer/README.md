## Serializer for unity

A simple serializer for unity data. Uses System.IO to save serialized classes to json file.

### Installation:
In a unity project open package manager and import this package using this git repo url.

### How to use:
To serialize a class and its properties use the `BaseSerializer` static class.

```c#
namespace MyGame.Data
{
  public class Fruit
  {
    public int size;
    public string name;
  }
  
  //Somewhere in your code:
  public class Main()
  {
    class Fruit aFruit = new Fruit(){int size = 3; string name = "ASize3Fruit"};
    var fileName = "FruitData";
    var savepath = BaseSerializer.SaveToJson<Fruit>(aFruit, fileName, @"/SavedGames/Data/", overwrite:true);
  }
  
}
//example savepath in editor could be: C:/Projects/YourGame/Assets/SavedGames/Data/MyGame.Data.Fruit_ASize3Fruit.json

```
  
File will be saved using Unity Application.DataPath, so in editor it will berelative to the project and in runtime it will save to the application data path.
  
### To deserialize
```
var fruit = LoadFromJson<Fruit>(@"C:/Projects/YourGame/Assets/SavedGames/Data/MyGame.Data.Fruit_ASize3Fruit.json");
```

## To develop this package:

- Create a unity project.
- In the `packages` folder Create a folder named `com.fertilesky.jsonserializer` 
- in the created folder run git bash and `git clone` this repo.
- Run unity project.
- Edit the package files and commit your changes to the repository.
- Dont forget to write unit tests for the unity test runner.
