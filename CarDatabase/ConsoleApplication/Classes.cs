using System;
using System.Collections.Generic;
using SimpleJSON;

namespace ConsoleApplication
{
    public class Make
    {
        public string Name = "";
        public Uri Url;
        public List<Model> Models = new List<Model>();

        public JSONClass ToJson()
        {
            var models = new JSONArray();

            foreach (var model in Models)
            {
                models.Add(model.ToJson());
            }

            return new JSONClass
            {
                { "name", Name },
                { "models", models }
            };
        }
    }

    public class Model
    {
        public string Name = "";
        public Uri Url;
        public List<Generation> Generations = new List<Generation>();

        public JSONClass ToJson()
        {
            var generations = new JSONArray();

            foreach (var generation in Generations)
            {
                generations.Add(generation.ToJson());
            }

            return new JSONClass
            {
                { "name", Name },
                { "generations", generations }
            };
        }
    }

    public class Generation
    {
        public string Name = "";
        public Uri Url;
        public List<Engine> Engines = new List<Engine>();

        public JSONClass ToJson()
        {
            var engines = new JSONArray();

            foreach (var engine in Engines)
            {
                engines.Add(engine.ToJson());
            }

            return new JSONClass
            {
                { "name", Name },
                { "engines", engines }
            };
        }
    }

    public class Engine
    {
        public string Name = "";
        public string Power = "";
        public string Price = "";
        public string Type = "";
        public string Drive = "";
        public string Acceleration = "";
        public string Speed = "";
        public string Consumption = "";
        public string Production = "";

        public JSONClass ToJson()
        {
            return new JSONClass
            {
                { "name", Name },
                { "power", Power },
                { "price", Price },
                { "type", Type },
                { "drive", Drive },
                { "acceleration", Acceleration },
                { "speed", Speed },
                { "consumption", Consumption },
                { "production", Production },
            };
        }
    }
}