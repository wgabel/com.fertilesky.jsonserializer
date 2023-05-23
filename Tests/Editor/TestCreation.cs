using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace FertileSky.Serialization.Tests
{
    public class TestCreation
    {
        private readonly string realitveSavePath = @"Tests3/SerializedFiles/Plants";

        [Test]
        public void TestDirectoryPathExists()
        {
            Fruit fruit = new Fruit() { plantName = "some fruit" };
            BaseSerializer.SaveToJson<Fruit>(
                fruit,
                nameof(fruit),
                realitveSavePath,
                overwrite: true);
            var path = BaseSerializer.GetAbsoluteDirectoryPath(realitveSavePath);
            Assert.IsTrue(Directory.Exists(path));
        }

        [Test]
        public void TestFilePathExists()
        {
            Fruit fruit = new Fruit() { plantName = "some fruit" };
            BaseSerializer.SaveToJson(
                fruit,
                nameof(fruit),
                realitveSavePath,
                overwrite: true);
            var path = BaseSerializer.GetPath<Fruit>(realitveSavePath, nameof(fruit));
            Assert.IsTrue(File.Exists(path));
        }

        [Test]
        public void TestFilePathCreationIsCorrect()
        {
            Fruit fruit = new Fruit() { plantName = "some fruit" };
            BaseSerializer.SaveToJson(
                fruit,
                $"{nameof(fruit)}22",
                realitveSavePath,
                overwrite: true);
            var path = BaseSerializer.GetPath<Fruit>(realitveSavePath, $"{nameof(fruit)}22");
            var correctPath = $"{UnityEngine.Application.dataPath}/Tests3/SerializedFiles/Plants/{typeof(Fruit)}_{nameof(fruit)}22.json";
            Assert.AreEqual(path, correctPath);
        }

        [Test]
        public void TestDeserializationOfSimpleObject()
        {
            Fruit fruit = new Fruit() { plantName = "some fruit" };
            string pathFruit = BaseSerializer.SaveToJson(
                fruit,
                $"{nameof(fruit)}22",
                realitveSavePath,
                overwrite: true);
            var loadedFruit = BaseSerializer.LoadFromJson<Fruit>(pathFruit);
            Assert.IsInstanceOf<Fruit>(loadedFruit, "loaded is not of type: Fruit");
        }

        [Test]
        public void TestDeserializedDataOfSimpleObject()
        {
            Fruit fruit = new Fruit() { plantName = "some fruit" };
            string pathFruit = BaseSerializer.SaveToJson(
                fruit,
                $"{nameof(fruit)}22",
                realitveSavePath,
                overwrite: true);
            var loadedFruit = BaseSerializer.LoadFromJson<Fruit>(pathFruit);
            Assert.AreEqual(fruit.plantName, loadedFruit.plantName);
        }

        [Test]
        public void TestDeserializationOfComplexObject()
        {
            var fruitClump = new PlantClump()
            {
                plantName = "ClumpOfFruits",
                plantsInClump = new List<Plant>()
                    {
                        new Fruit(){plantName="innerPlant1-Fruit"},
                        new Apple(){plantName="innerPlant2-Apple"},
                        new Vegetable(){plantName ="innerVegetable"}
                    }
            };
            string pathClump = BaseSerializer.SaveToJson(
                fruitClump,
                fruitClump.plantName,
                realitveSavePath,
                overwrite: true);
            var loadedClump = BaseSerializer.LoadFromJson<PlantClump>(pathClump);
            Assert.IsNotNull(loadedClump);
        }

        [Test]
        public void TestDeserializationOfComplexObjectNestedData()
        {
            var fruitClump = new PlantClump()
            {
                plantName = "ClumpOfFruits",
                plantsInClump = new List<Plant>()
                    {
                        new Fruit(){plantName="innerPlant1-Fruit"},
                        new Apple(){plantName="innerPlant2-Apple"},
                        new Vegetable(){plantName ="innerVegetable"}
                    }
            };
            string pathClump = BaseSerializer.SaveToJson(
                fruitClump,
                fruitClump.plantName,
                realitveSavePath,
                overwrite: true);
            var loadedClump = BaseSerializer.LoadFromJson<PlantClump>(pathClump);
            Assert.AreEqual(fruitClump.plantsInClump.Count, loadedClump.plantsInClump.Count);
        }
    }
}
